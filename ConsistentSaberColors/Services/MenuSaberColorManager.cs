using IPA.Utilities;
using SiraUtil.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ConsistentSaberColors.Services
{
    public class MenuSaberColorManager : MonoBehaviour
    { 
        [Inject] protected PlayerDataServicesProvider _dataProvider;
        [Inject] protected static SiraLog _log;

        private static PlayerData playerData;
        private static readonly Tuple<Color, Color> defaultPair = new Tuple<Color, Color>(
            new Color(0.784314f, 0.078431f, 0.078431f, 1.0f), 
            new Color(0.156863f, 0.556863f, 0.823529f, 1.0f));
        private static Tuple<Color, Color> saberColorPair;
        private static SetSaberGlowColor[] leftSideSabers;
        private static SetSaberGlowColor[] rightSideSabers;

        [Inject] public void Construct(PlayerDataServicesProvider dataProvider, SiraLog log)
        {
            _log = log;
            _dataProvider = dataProvider;

            StartCoroutine(GetSaberObjectsAndCache());
        }

        private IEnumerator GetSaberObjectsAndCache()
        {
            leftSideSabers = GameObject.Find("ControllerLeft/MenuHandle")?.GetComponentsInChildren<SetSaberGlowColor>();
            rightSideSabers = GameObject.Find("ControllerRight/MenuHandle")?.GetComponentsInChildren<SetSaberGlowColor>();

            yield return new WaitUntil(() => leftSideSabers != null && rightSideSabers != null);
            SetupPlayerData(_dataProvider.currentPlayerDataModel);
            saberColorPair = RefreshColorsData();
            Plugin.HarmonyID.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
        }

        private PlayerData SetupPlayerData(PlayerDataModel dataModel)
        {
            // Make sure our PlayerData isn't null!
            // If it is and we try to read from it, the game will wipe any existing PlayerData.
            if (dataModel == null)
            {
                saberColorPair = defaultPair;
                throw new NullReferenceException("Cannot find a suitable PlayerData to read from!");
            }

            return playerData = dataModel.playerData;
        }

        public static Tuple<Color, Color> RefreshColorsData()
        {
            Color leftSaber;
            Color rightSaber;

            if (playerData == null)
            {
                throw new NullReferenceException("No suitable PlayerData to read from!");
            }

            if (playerData.colorSchemesSettings.overrideDefaultColors)
            {
                var dict = playerData.colorSchemesSettings.GetField<Dictionary<string, ColorScheme>, ColorSchemesSettings>("_colorSchemesDict");
                var key = playerData.colorSchemesSettings.selectedColorSchemeId;
                var value = dict[key];

                leftSaber = value.saberAColor;
                rightSaber = value.saberBColor;
            }
            else
            {
                leftSaber = defaultPair.Item1;
                rightSaber = defaultPair.Item2;
            }

            saberColorPair = new Tuple<Color, Color>(leftSaber, rightSaber);
            UpdateSaberColors(saberColorPair);

            return null;
        }

        private static void UpdateSaberColors(Tuple<Color, Color> colorPair)
        {
            _log.Debug("Updating Saber Colors...");

            foreach (var obj in leftSideSabers)
            {
                var colorScheme = obj!.GetField<ColorManager, SetSaberGlowColor>("_colorManager").GetField<ColorScheme, ColorManager>("_colorScheme");
                colorScheme.SetField("_saberAColor", colorPair.Item1);
                obj.SetColors();
                //_log.Logger.Info($"Set Color: {colorPair.Item1} on {obj.name} ({obj.transform.parent.parent.name})");
            }

            foreach (var obj in rightSideSabers)
            {
                var colorScheme = obj!.GetField<ColorManager, SetSaberGlowColor>("_colorManager").GetField<ColorScheme, ColorManager>("_colorScheme");
                colorScheme.SetField("_saberBColor", colorPair.Item2);
                obj.SetColors();
                //_log.Logger.Info($"Set Color: {colorPair.Item2} on {obj.name} ({obj.transform.parent.parent.name})");
            }
        }
    }
}
