using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using ForgivingPortals.Tiers;

namespace ForgivingPortals
{
  [BepInPlugin("com.otter.bee.portals", "Forgiving Portals", "1.1.0")]
  public class ForgivingPortalsManager : BaseUnityPlugin
  {
    public static BepInEx.Logging.ManualLogSource Log;

    public static List<Tier> Tiers;

    private Harmony _harmony;

    public ForgivingPortalsManager()
    {
      Log = Logger;

      Logger.LogInfo("Patching...");

      _harmony = new Harmony("com.otter.bee.portals");
      _harmony.PatchAll();

      Logger.LogInfo("Creating tiers...");

      Tier tinCopper = new Tier(
        new List<string>() { "$item_copper", "$item_tin", "$item_bronze", "$item_tinore", "$item_copperore" },
        new List<string>() { "$item_iron" }
      );

      Tier iron = new Tier(
        new List<string>() { "$item_iron", "$item_ironscrap" },
        new List<string>() { "$item_silver" },
        tinCopper
      );

      Tier silver = new Tier(
        new List<string>() { "$item_silver", "$item_silverore" },
        new List<string>() { "$item_blackmetal" },
        iron
      );

      Tiers = new List<Tier>()
      {
        tinCopper,
        iron,
        silver
      };

      Logger.LogInfo("Done");
    }

    public static void SyncTeleportationState(List<ItemDrop.ItemData> items)
    {
      foreach(Tier tier in Tiers)
      {
        tier.SyncItems(items);
      }
    }

    /// <summary>
    /// Checks an item to see if it qualifies for unlocking a teleportation
    /// tier.
    /// </summary>
    /// <param name="item">The item to be examined</param>
    public static void CheckForUnlocks(ItemDrop.ItemData item)
    {
      foreach(Tier tier in Tiers)
      {
        tier.CheckUnlock(item);
      }
    }
  }
}
