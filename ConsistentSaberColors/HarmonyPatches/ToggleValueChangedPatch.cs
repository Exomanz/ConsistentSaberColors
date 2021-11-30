using ConsistentSaberColors.Services;
using HarmonyLib;

namespace ConsistentSaberColors.HarmonyPatches
{
    [HarmonyPatch(typeof(ColorsOverrideSettingsPanelController), MethodType.Normal)]
    [HarmonyPatch("HandleOverrideColorsToggleValueChanged")]
    internal class ToggleValueChangedPatch
    {
        [HarmonyPostfix]
        internal static void Postfix() => MenuSaberColorManager.Instance?.RefreshColorsData();
    }
}
