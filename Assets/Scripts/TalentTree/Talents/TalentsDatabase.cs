using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace Database
{
    public static class TalentsDatabase
    {
        public static TalentPropsData PropsData { get; private set; }

        public static Dictionary<string, TalentProperties> TalentProperties { get; private set; }
        public static Dictionary<string, Sprite> TalentSprites { get; private set; }

        public static void Initialize()
        {
            TalentProperties = new();
            TalentSprites = new();

            var loadedSprites = Resources.LoadAll<Sprite>("Talents/TalentSprites");
            PropsData = (TalentPropsData)Resources.LoadAll("Talents/TalentProps")[0];

            List <TProp> props = new();

            props.AddRange(SpecialTalentProps.GetTalentProps());
            props.AddRange(AttackTalentProps.GetTalentProps());
            props.AddRange(AbilityTalentProps.GetTalentProps());
            props.AddRange(DefanseTalentProps.GetTalentProps());
            props.AddRange(GlobalTalentProps.GetTalentProps());
            props.AddRange(HealthTalentProps.GetTalentProps());
            props.AddRange(MagicTalentProps.GetTalentProps());
            props.AddRange(UtilityTalentProps.GetTalentProps());

            foreach (var prop in props)
            {
                TalentProperties.Add(prop.Name, prop);
            }

            foreach (var prop in TalentProperties)
            {
                foreach (var sprite in loadedSprites)
                {
                    if (sprite.name == prop.Value.SpriteName)
                    {
                        TalentSprites.Add(prop.Key, sprite);
                    }
                }

                if (TalentSprites.ContainsKey(prop.Key) == false)
                {
                    foreach (var sprite in loadedSprites)
                    {
                        if (sprite.name == "StartPoint")
                        {
                            TalentSprites.Add(prop.Key, sprite);
                        }
                    }
                }
            }

            PropsData.Clear();
            PropsData.AddRange(props);
        }
    }
}
