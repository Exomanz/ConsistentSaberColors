using ConsistentSaberColors.Services;
using HarmonyLib;

namespace ConsistentSaberColors.HarmonyHelpers
{
    [HarmonyPatch(typeof(ColorsOverrideSettingsPanelController), MethodType.Normal)]
    [HarmonyPatch(nameof(ColorsOverrideSettingsPanelController.HandleDropDownDidSelectCellWithIdx))]
    public class SelectCellWithIdxPatch
    {
        [HarmonyPostfix] internal static void Postfix() => MenuSaberColorManager.UpdateSaberColors();
    }

    [HarmonyPatch(typeof(ColorsOverrideSettingsPanelController), MethodType.Normal)]
    [HarmonyPatch(nameof(ColorsOverrideSettingsPanelController.HandleOverrideColorsToggleValueChanged))]
    public class ToggleChangedPatch
    {
        [HarmonyPostfix] internal static void Postfix() => MenuSaberColorManager.UpdateSaberColors();
    }

    [HarmonyPatch(typeof(ColorSchemesSettings), nameof(ColorSchemesSettings.SetColorSchemeForId), MethodType.Normal)]
    public class SetColorSchemePatch
    {
        [HarmonyPostfix] internal static void Postfix() => MenuSaberColorManager.UpdateSaberColors();
    }
}
