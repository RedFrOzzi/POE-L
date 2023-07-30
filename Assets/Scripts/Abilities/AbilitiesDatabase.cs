using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using Database;

[CreateAssetMenu(fileName = "AbilitiesDatabase", menuName = "ScriptableObjects/AbilitiesDatabase")]
public class AbilitiesDatabase : ScriptableObject
{
    public Dictionary<AbilityID, Ability> Abilities { get; private set; }
    public List<Ability> AbilitiesList { get; private set; }

    public void Initialize()
    {
        AbilitiesList = new();
        Abilities = new();

        var types = Assembly.GetAssembly(typeof(Ability)).GetTypes().Where(x => typeof(Ability).IsAssignableFrom(x));
        
        foreach (var type in types)
        {
            var clone = Activator.CreateInstance(type) as Ability;

            if (clone.ID == AbilityID.None || clone.ID == AbilityID.Void)
                continue;

            AbilitiesList.Add(clone);
            Abilities.Add(clone.ID, clone);
        }
    }
}
