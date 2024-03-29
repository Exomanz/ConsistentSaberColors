﻿using ConsistentSaberColors.Services;   
using HarmonyLib;

namespace ConsistentSaberColors.HarmonyPatches
{
    [HarmonyPatch(typeof(ColorsOverrideSettingsPanelController), MethodType.Normal)]
    [HarmonyPatch("HandleEditColorSchemeControllerDidFinish")]
    internal class HandleEditColorSchemeDidFinishPatch
    {
        [HarmonyPostfix] 
        internal static void Postfix() => MenuSaberColorManager.Instance?.RefreshColorsData();
    }
}
