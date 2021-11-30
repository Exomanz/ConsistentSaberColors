using ConsistentSaberColors.Installers;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using IPAConfig = IPA.Config.Config;
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
        internal static IPALogger Log { get; private set; } = null!;
        internal static Harmony HarmonyID { get; private set; } = new Harmony("bs.Exomanz.saber-colors")!;
        internal static SaberColorConfig Config { get; private set; } = null!;

        public static bool fpfc = false;

        [Init] public Plugin(IPALogger logger, IPAConfig config, Zenjector zenject)
        {
            Log = logger!;
            Config = config!.Generated<SaberColorConfig>();

            string[] args = Environment.GetCommandLineArgs();

            if (args.Any(arg => arg!.ToLower().Contains("fpfc")))
            {
                Log.Notice("FPFC is enabled. PlayerData backup will still occur (if enabled), but the main color service will not.");
                fpfc = true;
            }

            zenject.OnApp<PlayerDataServicesProviderInstaller>().WithParameters(Log, Config);
            zenject.On<ColorManagerInstaller>().Register<SaberColorManagerInstaller>();
        }

        [OnEnable] public void Enable()
        {
            HarmonyID.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnDisable] public void Disable()
        {
            HarmonyID.UnpatchSelf();
        }
    }
}
