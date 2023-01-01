using HarmonyLib;

namespace ForgivingPortals.Patches
{
  [HarmonyPatch(typeof(Player), nameof(Player.AddKnownItem))]
  public static class AddKnownItemPatch
  {
    private static void Postfix(Player __instance, ItemDrop.ItemData item)
    {
      ForgivingPortalsManager.CheckForUnlocks(item);
    }
  }

  [HarmonyPatch(typeof(Container), "CheckForChanges")]
  public static class SyncContainerPatch
  {
    private static void Postfix(Container __instance)
    {
      Inventory inventory = Traverse.Create(__instance).Field("m_inventory").GetValue<Inventory>();
      var items = inventory.GetAllItems();

      foreach (var item in items)
      {
        ForgivingPortalsManager.CheckForUnlocks(item);
      }

      ForgivingPortalsManager.SyncTeleportationState(items);
    }
  }
}
