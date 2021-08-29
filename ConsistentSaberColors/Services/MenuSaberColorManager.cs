using HarmonyLib;
using IPA.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

#pragma warning disable CS0649
namespace ConsistentSaberColors.Services
{
    public class MenuSaberColorManager : MonoBehaviour
    {
        Harmony Harmony => Plugin.HarmonyID;
        [Inject] PlayerDataModel dataModel;
#pragma warning restore CS0649
        private static readonly Color defaultLeft = new Color(0.784314f, 0.078431f, 0.078431f, 1.0f);
        private static readonly Color defaultRight = new Color(0.156863f, 0.556863f, 0.823529f, 1.0f);
        private static Color leftSaber;
        private static Color rightSaber;

        public void Start()
        {
            SetupPlayerData(dataModel);
            SceneManager.activeSceneChanged += ActiveSceneChanged;
        }

        private Task SetupPlayerData(PlayerDataModel dataModel)
        {
            PlayerData data = dataModel.GetField<PlayerData, PlayerDataModel>("_playerData");

            // Make sure our PlayerData isn't null!
            // If it is and we try to read from it, the game will wipe any existing PlayerData.
            if (data is null || dataModel is null)
            {
                leftSaber = defaultLeft;
                rightSaber = defaultRight;
                throw new System.NullReferenceException("Cannot find a suitable PlayerData to read from!\nSetting default colors and exiting...");
            }

            // Check if our player is override default colors, and if so, grab those colors.
            if (data.colorSchemesSettings.overrideDefaultColors)
            {
                var dict = data.colorSchemesSettings.GetField<Dictionary<string, ColorScheme>, ColorSchemesSettings>("_colorSchemesDict");
                var key = data.colorSchemesSettings.selectedColorSchemeId;
                var value = dict[key];

                leftSaber = value.saberAColor;
                rightSaber = value.saberBColor;
            }
            else
            {
                leftSaber = defaultLeft;
                rightSaber = defaultRight;
            }

            // This is where all the magic happens, baby
            Harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
            return null;
        }

        private void ActiveSceneChanged(Scene arg0, Scene arg1)
        {
            if (arg1.name == "MainMenu") UpdateColors();
        }

        public static void UpdateColors()
        {
            var left = GameObject.Find("ControllerLeft/MenuHandle")?.GetComponentsInChildren<SetSaberGlowColor>();
            var right = GameObject.Find("ControllerRight/MenuHandle")?.GetComponentsInChildren<SetSaberGlowColor>();

            foreach (var obj in left)
            {
                var colorScheme = obj.GetField<ColorManager, SetSaberGlowColor>("_colorManager").GetField<ColorScheme, ColorManager>("_colorScheme");
                colorScheme.SetField("_saberAColor", leftSaber);
                obj.SetColors();
            }

            foreach (var obj in right)
            {
                var colorScheme = obj.GetField<ColorManager, SetSaberGlowColor>("_colorManager").GetField<ColorScheme, ColorManager>("_colorScheme");
                colorScheme.SetField("_saberBColor", rightSaber);
                obj.SetColors();
            }
        }

        public void OnDisable() => OnDestroy();
        public void OnDestroy() => SceneManager.activeSceneChanged -= ActiveSceneChanged;
    }
}
