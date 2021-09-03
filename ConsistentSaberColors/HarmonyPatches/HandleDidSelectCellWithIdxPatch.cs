using ConsistentSaberColors.Services;
using HarmonyLib;

namespace ConsistentSaberColors.HarmonyPatches
{
    [HarmonyPatch(typeof(ColorsOverrideSettingsPanelController), MethodType.Normal)]
    [HarmonyPatch("HandleDropDownDidSelectCellWithIdx")]
    public class HandleDidSelectCellWithIdxPatch
    {
        [HarmonyPostfix]
        internal static void Postfix() => MenuSaberColorManager.RefreshColorsData();
    }
}
