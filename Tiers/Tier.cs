using System.Collections.Generic;
using System.Linq;

namespace ForgivingPortals.Tiers
{
  public class Tier
  {
    public bool Unlocked { get; protected set; }

    public readonly HashSet<string> Contains;
    public readonly HashSet<string> UnlockedBy;
    public readonly Tier PreviousTier;

    public Tier(IEnumerable<string> contains, IEnumerable<string> unlockedBy, Tier previousTier = null) 
      => (Contains, UnlockedBy, PreviousTier)
        = (contains.ToHashSet(), unlockedBy.ToHashSet(), previousTier);

    public void CheckUnlocks(IEnumerable<ItemDrop.ItemData> items)
    {
      foreach(var item in items)
      {
        CheckUnlock(item);
      }
    }

    public void CheckUnlock(ItemDrop.ItemData item)
    {
      CheckUnlock(item.m_shared.m_name);
    }

    public void CheckUnlock(string item)
    {
      if (!Unlocked && UnlockedBy.Contains(item))
      {
        Unlock();
      }
    }

    public void SyncItems(IEnumerable<ItemDrop.ItemData> items)
    {
      if (Unlocked)
      {
        foreach (var item in items)
        {
          if (Contains.Contains(item.m_shared.m_name))
          {
            item.m_shared.m_teleportable = true;
          }
        }
      }

      PreviousTier.SyncItems(items);
    }

    protected void Unlock()
    {
      Unlocked = true;

      ForgivingPortalsManager.Log.LogInfo($"Unlocked tier containing items: {string.Join(",", Contains)}");

      PreviousTier.Unlock();
    }
  }
}
