using ConsistentSaberColors.Services;
using IPA.Logging;
using SiraUtil;
using Zenject;

namespace ConsistentSaberColors.Installers
{
    public class PlayerDataServicesProviderInstaller : Installer<PlayerDataServicesProviderInstaller>
    {
        private readonly Logger _logger;
        private readonly SaberColorConfig _config;

        public PlayerDataServicesProviderInstaller(Logger logger, SaberColorConfig config)
        {
            _logger = logger;
            _config = config;
        }

        public override void InstallBindings()
        {
            Container.Bind<SaberColorConfig>().FromInstance(_config).AsCached();
            Container.BindInterfacesAndSelfTo<PlayerDataServicesProvider>().AsCached().NonLazy();
            Container.BindLoggerAsSiraLogger(_logger);
        }
    }
}
