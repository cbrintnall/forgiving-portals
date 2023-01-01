using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace ForgivingPortals
{
  [BepInPlugin("com.otter.bee.portals", "Forgiving Portals", "0.0.1")]
  public class ForgivingPortalsManager : BaseUnityPlugin
  {
    public static BepInEx.Logging.ManualLogSource Log;

    public static bool TinCopperTeleportUnlocked = false;
    public static bool IronTeleportUnlocked = false;
    public static bool SilverTeleportUnlocked = false;

    public static HashSet<string> TinCopperItems = new HashSet<string>()
    {
      "$item_copper",
      "$item_tin",
      "$item_bronze",
      "$item_tinore",
      "$item_copperore"
    };

    public static HashSet<string> IronItems = new HashSet<string>()
    {
      "$item_iron",
      "$item_ironscrap"
    };

    public static HashSet<string> SilverItems = new HashSet<string>()
    {
      "$item_silver",
      "$item_silverore"
    };

    private Harmony _harmony;

    public ForgivingPortalsManager()
    {
      Log = Logger;

      Logger.LogInfo("Patching...");

      _harmony = new Harmony("com.otter.bee.portals");
      _harmony.PatchAll();

      Logger.LogInfo("Done");
    }

    public static void SyncTeleportationState(List<ItemDrop.ItemData> items)
    {
      foreach(var item in items)
      {
        if (TinCopperTeleportUnlocked && TinCopperItems.Contains(item.m_shared.m_name))
        {
          item.m_shared.m_teleportable = true;
        }

        if (IronTeleportUnlocked && IronItems.Contains(item.m_shared.m_name))
        {
          item.m_shared.m_teleportable = true;
        }

        if (SilverTeleportUnlocked && SilverItems.Contains(item.m_shared.m_name))
        {
          item.m_shared.m_teleportable = true;
        }
      }
    }

    /// <summary>
    /// Checks an item to see if it qualifies for unlocking a teleportation
    /// tier.
    /// </summary>
    /// <param name="item">The item to be examined</param>
    /// <returns>If anything was unlocked</returns>
    public static bool CheckForUnlocks(ItemDrop.ItemData item)
    {
      string name = item.m_shared.m_name;

      if (!TinCopperTeleportUnlocked && name == "$item_iron")
      {
        TinCopperTeleportUnlocked = true;
        Log.LogInfo("Unlocked tin / copper / bronze teleporting.");
        return true;
      }

      if (!IronTeleportUnlocked && name == "$item_silver")
      {
        IronTeleportUnlocked = true;
        Log.LogInfo("Unlocked iron teleporting.");
        return true;
      }

      if (!SilverTeleportUnlocked && name == "$item_blackmetal")
      {
        SilverTeleportUnlocked = true;
        Log.LogInfo("Unlocked silver teleporting.");
        return true;
      }

      return false;
    }
  }
}
