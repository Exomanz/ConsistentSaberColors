using IPA.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace ConsistentSaberColors
{
    public static class MenuSaberColorManager
    {
        internal static Color defaultRight = new Color(0.156863f, 0.556863f, 0.823529f, 1.0f);
        internal static Color defaultLeft = new Color(0.784314f, 0.078431f, 0.078431f, 1.0f);

        public static void UpdateColors()
        {
            PlayerDataModel PlayerDataModel;
            PlayerData playerData;

            PlayerDataModel = GameObject.Find("PlayerDataModel(Clone)")?.GetComponent<PlayerDataModel>();
            playerData = PlayerDataModel!.GetField<PlayerData, PlayerDataModel>("_playerData");

            if (playerData is null || PlayerDataModel is null) return;

            // Create some empty Colors
            Color colA;
            Color colB;

            // Check if our player is overriding colors, and if so, yoink those values
            if (!playerData.colorSchemesSettings.overrideDefaultColors)
            {
                colA = defaultLeft;
                colB = defaultRight;
            }
            else
            {
                var x = playerData.colorSchemesSettings.GetField<Dictionary<string, ColorScheme>, ColorSchemesSettings>("_colorSchemesDict");
                var key = playerData.colorSchemesSettings.selectedColorSchemeId;
                var val = x[key];

                colA = val.saberAColor;
                colB = val.saberBColor;
            }

            var saberGlows_L = GameObject.Find("ControllerLeft/MenuHandle")!.GetComponentsInChildren<SetSaberGlowColor>();
            var saberGlows_R = GameObject.Find("ControllerRight/MenuHandle")!.GetComponentsInChildren<SetSaberGlowColor>();

            foreach (var x in saberGlows_L)
            {
                var colorScheme = x.GetField<ColorManager, SetSaberGlowColor>("_colorManager").GetField<ColorScheme, ColorManager>("_colorScheme");
                colorScheme.SetField("_saberAColor", colA);
                x.SetColors();
            }

            foreach (var y in saberGlows_R)
            {
                var colorScheme = y.GetField<ColorManager, SetSaberGlowColor>("_colorManager").GetField<ColorScheme, ColorManager>("_colorScheme");
                colorScheme.SetField("_saberBColor", colB);
                y.SetColors();
            }
        }
    }
}
