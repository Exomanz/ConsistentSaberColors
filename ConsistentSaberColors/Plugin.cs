using ConsistentSaberColors.Services;
using HarmonyLib;
using IPA;
using IPALogger = IPA.Logging.Logger;
using SiraUtil.Zenject;
using Zenject;

namespace ConsistentSaberColors
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static IPALogger Log { get; private set; }
        internal static Harmony HarmonyID { get; private set; }

        [Init] public Plugin(IPALogger logger, Zenjector zenject)
        {
            Log = logger;

            zenject.OnApp<SaberColorManagerInstaller>();
        }

        [OnEnable] public void Enable()
        {
            // We cannot patch here, since that will call updates too early and wipe any PlayerData,
            // so instead, we'll just prepare the HarmonyID.
            if (HarmonyID is null) HarmonyID = new Harmony("bs.Exomanz.saber-colors");
        }

        [OnDisable] public void Disable()
        {
            HarmonyID.UnpatchAll(HarmonyID.Id);
            HarmonyID = null;
        }
    }

    public class SaberColorManagerInstaller : Installer
    {
        public override void InstallBindings() =>
            Container.Bind<MenuSaberColorManager>().FromNewComponentOn(
                new UnityEngine.GameObject("MenuSaberColorManager")).AsSingle().NonLazy();
    }
}
