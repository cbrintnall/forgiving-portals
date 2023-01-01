using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ForgivingPortals.Patches
{
  [HarmonyPatch(typeof(InventoryGui), "Show")]
  public static class InventoryShow
  {
    private static void Postfix()
    {
      ForgivingPortalsManager.SyncTeleportationState(Player.m_localPlayer.GetInventory().GetAllItems());
    }
  }

  [HarmonyPatch(typeof(Inventory), "IsTeleportable")]
  public static class InventoryIsTeleportable
  {
    private static void Prefix()
    {
      ForgivingPortalsManager.SyncTeleportationState(Player.m_localPlayer.GetInventory().GetAllItems());
    }
  }

  [HarmonyPatch(typeof(Container), "Interact")]
  public static class ContainerInteract
  {
    private static void Prefix(ref Container __instance)
    {
      if (__instance.GetInventory() != null)
      {
        ForgivingPortalsManager.SyncTeleportationState(__instance.GetInventory().GetAllItems());
      }
    }
  }
}
