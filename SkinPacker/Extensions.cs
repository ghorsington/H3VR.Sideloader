using System;
using System.Windows.Forms;

namespace SkinPacker
{
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

            control.Validated += (sender, args) =>
            {
                var result = validator();
                errorProvider.SetError(control, result);
            };
        }
    }
}