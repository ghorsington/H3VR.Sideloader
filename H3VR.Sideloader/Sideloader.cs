using BepInEx;

namespace H3VR.Sideloader
{
    [BepInPlugin("horse.coder.h3vr.sideloader", NAME, VERSION)]
    [BepInDependency("gravydevsupreme.xunity.resourceredirector")]
    public class Sideloader : BaseUnityPlugin
    {
        internal const string VERSION = "1.0.0";
        internal const string NAME = "H3VR Sideloader";
    }
}