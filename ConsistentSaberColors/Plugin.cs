using HarmonyLib;
using IPA;
using IPALogger = IPA.Logging.Logger;
using UnityEngine.SceneManagement;

namespace ConsistentSaberColors
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static IPALogger Log { get; private set; }
        internal static Harmony HarmonyID { get; private set; }

        [Init] public Plugin(IPALogger logger)
        {
            Log = logger;
            SceneManager.activeSceneChanged += ActiveSceneChanged;
        }

        [OnEnable] public void Enable()
        {
            if (HarmonyID is null) HarmonyID = new Harmony("bs.Exomanz.csc");
            HarmonyID.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
        }
        [OnDisable] public void Disable()
        {
            HarmonyID.UnpatchAll(HarmonyID.Id);
            HarmonyID = null;
        }

        private void ActiveSceneChanged(Scene arg0, Scene arg1)
        {
            if (arg1.name == "MainMenu") MenuSaberColorManager.UpdateColors();
        }
    }
}
