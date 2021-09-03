using ConsistentSaberColors.Services;
using HarmonyLib;

namespace ConsistentSaberColors.HarmonyPatches
{
    [HarmonyPatch(typeof(ColorsOverrideSettingsPanelController), MethodType.Normal)]
    [HarmonyPatch("HandleOverrideColorsToggleValueChanged")]
    public class ToggleValueChangedPatch
    {
        [HarmonyPostfix]
        internal static void Postfix() => MenuSaberColorManager.RefreshColorsData();
    }
}
