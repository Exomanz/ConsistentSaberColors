using ConsistentSaberColors.Services;
using UnityEngine;
using Zenject;

namespace ConsistentSaberColors.Installers
{
    public class SaberColorManagerInstaller : Installer
    {
        public override void InstallBindings()
        {
            // Prevents multiple managers from being made.
            // The installer is still ran twice though, and there's not much
            // I'm able to do about that :sadge:.
            var amt = Object.FindObjectsOfType<MenuSaberColorManager>().Length;
            if (amt == 1) return;

            Container.Bind<MenuSaberColorManager>().FromNewComponentOn
                (new GameObject("MenuSaberColorManager")).AsSingle().NonLazy();
        }
    }
}
