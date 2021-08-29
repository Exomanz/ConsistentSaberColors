using IPA.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

#pragma warning disable CS0649
namespace ConsistentSaberColors.Services
{
    /// <summary>
    /// Master class for saber color managing in the menu.
    /// All functionality here could very easily be done without using <see cref="MonoBehaviour"/>
    /// but for the sake of debugging, I kept it as such. This may change in the future.
    /// </summary>
    public class MenuSaberColorManager : MonoBehaviour
    {
        #region Useful Stuff
        [Inject] protected PlayerDataModel dataModel;
        private static PlayerData playerData;
        private static readonly Tuple<Color, Color> defaultPair = new Tuple<Color, Color>(
            new Color(0.784314f, 0.078431f, 0.078431f, 1.0f), 
            new Color(0.156863f, 0.556863f, 0.823529f, 1.0f));
        private static Tuple<Color, Color> saberColorPair;
        #endregion

        public void Start()
        {
            SetupPlayerData(dataModel);
        }

        private PlayerData SetupPlayerData(PlayerDataModel dataModel)
        {
            playerData = dataModel.GetField<PlayerData, PlayerDataModel>("_playerData");

            // Make sure our PlayerData isn't null!
            // If it is and we try to read from it, the game will wipe any existing PlayerData.
            if (playerData is null || dataModel is null)
            {
                saberColorPair = defaultPair;
                throw new NullReferenceException("Cannot find a suitable PlayerData to read from!\nSetting default colors and exiting...");
            }

            RefreshColorsData();
            Plugin.HarmonyID.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
            SceneManager.activeSceneChanged += ActiveSceneChanged;
            return dataModel.playerData;
        }

        private void ActiveSceneChanged(Scene arg0, Scene arg1)
        {
            if (arg1.name == "MainMenu")
            {
                UpdateSaberColors();
                SceneManager.activeSceneChanged -= ActiveSceneChanged;
            }
        }

        private static Tuple<Color, Color> RefreshColorsData()
        {
            Color leftSaber;
            Color rightSaber;

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

            return saberColorPair = new Tuple<Color, Color>(leftSaber, rightSaber);
        }

        public static void UpdateSaberColors()
        {
            RefreshColorsData();

            var left = GameObject.Find("ControllerLeft/MenuHandle")?.GetComponentsInChildren<SetSaberGlowColor>();
            var right = GameObject.Find("ControllerRight/MenuHandle")?.GetComponentsInChildren<SetSaberGlowColor>();

            foreach (var obj in left)
            {
                var colorScheme = obj.GetField<ColorManager, SetSaberGlowColor>("_colorManager").GetField<ColorScheme, ColorManager>("_colorScheme");
                colorScheme.SetField("_saberAColor", saberColorPair.Item1);
                obj.SetColors();
            }

            foreach (var obj in right)
            {
                var colorScheme = obj.GetField<ColorManager, SetSaberGlowColor>("_colorManager").GetField<ColorScheme, ColorManager>("_colorScheme");
                colorScheme.SetField("_saberBColor", saberColorPair.Item2);
                obj.SetColors();
            }
        }
    }
}
