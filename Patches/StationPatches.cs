using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ForgivingPortals.Patches
{
  [HarmonyPatch(typeof(Player), nameof(Player.AddKnownItem))]
  public static class AddKnownItemPatch
  {
    private static void Postfix(Player __instance, ItemDrop.ItemData item)
    {
      string name = item.m_shared.m_name;

      if (name == "$item_iron")
      {
        ForgivingPortalsManager.TinCopperTeleportUnlocked = true;
        ForgivingPortalsManager.Log.LogInfo("Unlocked tin / copper / bronze teleporting.");
      }

      if (name == "$item_silver")
      {
        ForgivingPortalsManager.IronTeleportUnlocked = true;
        ForgivingPortalsManager.Log.LogInfo("Unlocked iron teleporting.");
      }

      if (name == "$item_blackmetal")
      {
        ForgivingPortalsManager.SilverTeleportUnlocked = true;
        ForgivingPortalsManager.Log.LogInfo("Unlocked silver teleporting.");
      }
    }
  }
}
