using HarmonyLib;

namespace ConsistentSaberColors
{
    [HarmonyPatch(typeof(ColorsOverrideSettingsPanelController), "HandleDropDownDidSelectCellWithIdx", MethodType.Normal)]
    public class SelectCellWithIdxPatch
    {
        [HarmonyPostfix]
        internal static void Postfix()
        {
            MenuSaberColorManager.UpdateColors();
        }
    }

    [HarmonyPatch(typeof(ColorsOverrideSettingsPanelController), "HandleOverrideColorsToggleValueChanged", MethodType.Normal)]
    public class ValueChangedPatch
    {
        [HarmonyPostfix]
        internal static void Postfix(ref bool isOn)
        {
            if (isOn) MenuSaberColorManager.UpdateColors();
        }
    }

    [HarmonyPatch(typeof(ColorSchemesSettings), "SetColorSchemeForId", MethodType.Normal)]
    public class SetColorSchemeForIdPatch
    {
        [HarmonyPostfix]
        internal static void Postfix()
        {
            MenuSaberColorManager.UpdateColors();
        }
    }
}
