using ConsistentSaberColors.Services;
using SiraUtil;
using Zenject;

namespace ConsistentSaberColors.Installers
{
    public class PlayerDataServicesProviderInstaller : Installer
    {
        private readonly IPA.Logging.Logger _logger;

        public PlayerDataServicesProviderInstaller(IPA.Logging.Logger logger) => _logger = logger;

        public override void InstallBindings()
        {
            Container.Bind<PlayerDataServicesProvider>().ToSelf().AsCached().NonLazy();
            Container.BindLoggerAsSiraLogger(_logger);
        }
    }
}
