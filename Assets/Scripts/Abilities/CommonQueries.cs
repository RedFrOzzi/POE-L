using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class CommonQueries
{
    public static StatsChanges[] FindAbilitiesStatsByTags(AbilitySlot[] abilitySlots, params ModTag[] queryTags)
    {
        if (queryTags.Length == 0 || abilitySlots == null)
        {
            Debug.Log("Ability search is invalid");
            return null;
        }

        if (queryTags.Length == 1)
        {
            return FindAbilitiesStatsByTag(abilitySlots, queryTags[0]);
        }

        abilitySlots = abilitySlots.Where(x =>
        {
            foreach (var tag in x.Ability.Tags)
            {
                foreach (var t in queryTags)
                {
                    if (tag == t)
                        return true;
                }
            }
            return false;
        })
            .ToArray();

        StatsChanges[] statsChanges = new StatsChanges[abilitySlots.Length];
        for (byte i = 0; i < statsChanges.Length; i++)
        {
            statsChanges[i] = abilitySlots[i].LSC;
        }
        return statsChanges;
    }

    private static StatsChanges[] FindAbilitiesStatsByTag(AbilitySlot[] abilitySlots, ModTag queryTag)
    {
        abilitySlots = abilitySlots.Where(x =>
        {
            foreach (var tag in x.Ability.Tags)
            {
                if (tag == queryTag)
                    return true;
            }
            return false;
        }).ToArray();

        StatsChanges[] statsChanges = new StatsChanges[abilitySlots.Length];
        for (byte i = 0; i < statsChanges.Length; i++)
        {
            statsChanges[i] = abilitySlots[i].LSC;
        }
        return statsChanges;
    }
}
