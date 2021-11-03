using ConsistentSaberColors.Installers;
using HarmonyLib;
using IPA;
using IPALogger = IPA.Logging.Logger;
using SiraUtil.Zenject;
using System;
using System.Linq;
using System.Reflection;

namespace ConsistentSaberColors
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static IPALogger Log { get; private set; }
        internal static Harmony HarmonyID { get; private set; } = new Harmony("bs.Exomanz.saber-colors");

        [Init] public Plugin(IPALogger logger, Zenjector zenject)
        {
            Log = logger;
            bool fpfc = false;

            string[] args = Environment.GetCommandLineArgs();
            if (args.Any(x => x.ToLower().Contains("fpfc")))
            {
                fpfc = true;
                Log.Notice("FPFC is enabled. PlayerData backup will still occur, but the main color service will not be enabled.");
            }

            zenject.OnApp<PlayerDataServicesProviderInstaller>().WithParameters(Log);

            if (fpfc) return;
            zenject.On<ColorManagerInstaller>().Register<SaberColorManagerInstaller>();
        }

        [OnEnable] public void Enable()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), HarmonyID.Id);
        }

        [OnDisable] public void Disable()
        {
            HarmonyID.UnpatchSelf();
        }
    }
}
