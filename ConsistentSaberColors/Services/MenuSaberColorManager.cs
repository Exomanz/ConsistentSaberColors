using IPA.Utilities;
using SiraUtil.Tools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace ConsistentSaberColors.Services
{
    /// <summary>
    /// Base class for updating menu saber colors. All data is retrieved from the
    /// player's currently selected <see cref="ColorScheme"/> and their <see cref="PlayerDataModel"/>.
    /// </summary>
    public class MenuSaberColorManager : MonoBehaviour
    {
        public static MenuSaberColorManager Instance { get; private set; } = null!;

        [Inject] protected PlayerDataServicesProvider _dataProvider = null!;
        [Inject] protected SiraLog _log = null!;

        private PlayerData playerData = null!;
        private SetSaberGlowColor[] leftSideSabers = null!;
        private SetSaberGlowColor[] rightSideSabers = null!;
        private Dictionary<string, ColorScheme> dictionary = null!;
        private string key = null!;

        public void Start()
        {
            Instance = this;
            GetSaberObjectsAndCacheAsync(_dataProvider.currentPlayerDataModel);
        }

        public async void GetSaberObjectsAndCacheAsync(PlayerDataModel dataModel)
        {
            VRController[] controllers = FindObjectsOfType<VRController>();

            leftSideSabers = controllers![1].GetComponentsInChildren<SetSaberGlowColor>();
            rightSideSabers = controllers![0].GetComponentsInChildren<SetSaberGlowColor>();

            await SetupPlayerData(dataModel!, out bool success);

            if (!success)
            {
                throw new ArgumentException("Cannot find a PlayerData to read from." +
                    "\nLeaving the colors as-is and aborting the method." +
                    "\nNOTE: As long as you have data backups enabled, your data is safe!");
            }

            RefreshColorsData();
        }

        private Task SetupPlayerData(PlayerDataModel dataModel, out bool success)
        {
            // Make sure our PlayerData isn't null!
            // If it's null, the game generates a new one on the fly and overwrites the existing one regardless of whether it exists or not.
            if (dataModel is null)
            {
                success = false;
                return Task.CompletedTask;
            }

            playerData = dataModel?.playerData!;
            success = true;

            return Task.CompletedTask;
        }

        public void RefreshColorsData()
        {
            if (playerData!.colorSchemesSettings.overrideDefaultColors)
            {
                dictionary = playerData!.colorSchemesSettings?.GetField<Dictionary<string, ColorScheme>, ColorSchemesSettings>("_colorSchemesDict")!;
                key = playerData!.colorSchemesSettings?.selectedColorSchemeId!;
            }

            UpdateSaberColors();
        }

        private void UpdateSaberColors()
        {
            var colorScheme = dictionary![key!];
#if DEBUG
            _log.Logger.Notice("Updating Saber Colors...");
#endif
            foreach (var obj in leftSideSabers)
            {
                var colorManager = obj?.GetField<ColorManager, SetSaberGlowColor>("_colorManager");
                colorManager?.SetField("_colorScheme", colorScheme);
                obj?.SetColors();
            }

            foreach (var obj in rightSideSabers)
            {
                var colorManager = obj?.GetField<ColorManager, SetSaberGlowColor>("_colorManager");
                colorManager?.SetField("_colorScheme", colorScheme);
                obj?.SetColors();
            }
        }
    }
}
