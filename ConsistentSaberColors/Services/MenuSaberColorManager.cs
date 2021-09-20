using IPA.Utilities;
using SiraUtil.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
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
        [Inject] protected PlayerDataServicesProvider _dataProvider;
        [InjectOptional] protected static SiraLog _log;

        private static PlayerData playerData;
        private static SetSaberGlowColor[] leftSideSabers;
        private static SetSaberGlowColor[] rightSideSabers;

        // Fixes overwrites of color schemes
        private static Dictionary<string, ColorScheme> dictionary;
        private static string key;

        public void Start()
        {
            StartCoroutine(GetSaberObjectsAndCache(_dataProvider.currentPlayerDataModel));
        }

        private IEnumerator GetSaberObjectsAndCache(PlayerDataModel dataModel)
        {
            leftSideSabers = GameObject.Find("ControllerLeft/MenuHandle")?.GetComponentsInChildren<SetSaberGlowColor>();
            rightSideSabers = GameObject.Find("ControllerRight/MenuHandle")?.GetComponentsInChildren<SetSaberGlowColor>();
            yield return new WaitUntil(() => leftSideSabers != null && rightSideSabers != null);

            SetupPlayerData(dataModel);
            yield return new WaitUntil(() => playerData != null);

            RefreshColorsData();
        }

        private PlayerData SetupPlayerData(PlayerDataModel dataModel)
        {
            // Make sure our PlayerData isn't null!
            // If it is and we try to read from it, the game will wipe any existing PlayerData.
            if (dataModel == null)
                throw new NullReferenceException("Cannot find a suitable PlayerData to read from!\n" +
                    "Aborting the method and leaving colors as default");

            return playerData = dataModel.playerData;
        }

        public static void RefreshColorsData()
        {
            if (playerData == null)
            {
                throw new NullReferenceException("No suitable PlayerData to read from!");
            }

            if (playerData.colorSchemesSettings.overrideDefaultColors)
            {
                dictionary = playerData.colorSchemesSettings.GetField<Dictionary<string, ColorScheme>, ColorSchemesSettings>("_colorSchemesDict");
                key = playerData.colorSchemesSettings.selectedColorSchemeId;
            }

            UpdateSaberColors();
        }

        private static void UpdateSaberColors()
        {
            var colorScheme = dictionary[key];
            //_log.Debug("Updating Saber Colors...");

            foreach (var obj in leftSideSabers)
            {
                var colorManager = obj!.GetField<ColorManager, SetSaberGlowColor>("_colorManager");
                colorManager.SetField("_colorScheme", colorScheme);
                obj.SetColors();
                //_log.Logger.Info($"Set Color: {colorPair.Item1} on {obj.name} ({obj.transform.parent.parent.name})");
            }

            foreach (var obj in rightSideSabers)
            {
                var colorManager = obj!.GetField<ColorManager, SetSaberGlowColor>("_colorManager");
                colorManager.SetField("_colorScheme", colorScheme);
                obj.SetColors();
                //_log.Logger.Info($"Set Color: {colorPair.Item2} on {obj.name} ({obj.transform.parent.parent.name})");
            }
        }
    }
}
