using System;
using System.IO;
using System.Windows.Forms;

namespace SkinPacker
{
    public static class PathUtils
    {
        public static readonly char[] PathSeparators = {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};

        public static bool IsFullPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path)
                   && path.IndexOfAny(Path.GetInvalidPathChars()) == -1
                   && Path.IsPathRooted(path)
                   && !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
        }
    }

    public static class Extensions
    {
        public static void AddTooltip(this Control control, string title, params string[] lines)
        {
            var tooltip = new ToolTip
            {
                IsBalloon = true,
                AutoPopDelay = 30000,
                ToolTipIcon = ToolTipIcon.Info,
                ToolTipTitle = title
            };

            tooltip.SetToolTip(control, string.Join(Environment.NewLine, lines));
        }

        public static void AddValidator(this Control control, Func<string> validator)
        {
            var errorProvider = new ErrorProvider {BlinkStyle = ErrorBlinkStyle.NeverBlink};
            control.Validating += (sender, args) =>
            {
                var result = validator();
                errorProvider.SetError(control, result);
                if (result != string.Empty)
                    args.Cancel = true;
            };
        }
    }
}