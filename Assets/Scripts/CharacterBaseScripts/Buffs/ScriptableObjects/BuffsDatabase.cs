using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Database;


[CreateAssetMenu(fileName = "Buff", menuName = "ScriptableObjects/BuffDatabase/Database")]
public class BuffsDatabase : ScriptableObject
{
    //--------------------------------------------------------------------
    public static Dictionary<string, BuffHot> HotBuffs { get; private set; } = new();
    //---
    [Serializable] public class BuffHotProperties
    {
        public BaseBuffProperties properties = new();
        public float HealAmount = 100;
    }
    [ExposedField] public BuffHotProperties[] buffHotProperties;
    //--------------------------------------------------------------------
    public static Dictionary<string, BuffSpeedUp> SpeedUpBuffs { get; private set; } = new();
    //---
    [Serializable] public class BuffSpeedUpProperties
    {
        public BaseBuffProperties properties = new();
        public float SpeedAmount = 100f;
    }
    [ExposedField] public BuffSpeedUpProperties[] speedUpProperties;
    //--------------------------------------------------------------------
    public static Dictionary<string, DebuffSlow> SlowDebuffs { get; private set; } = new();
    //---
    [Serializable] public class DebuffSlowProperties
    {
        public BaseBuffProperties properties = new();
        public float SlowAmount = 20;
    }
    [ExposedField] public DebuffSlowProperties[] debuffSlowProperties;
    //--------------------------------------------------------------------
    public static Dictionary<string, DebuffStun> StunDebuffs { get; private set; } = new();
    //---
    [Serializable] public class DebuffStunProperties
    {
        public BaseBuffProperties properties = new();
    }
    [ExposedField] public DebuffStunProperties[] debuffStunProperties;
    //--------------------------------------------------------------------


    public void Show()
    {
        foreach (var (key, value) in HotBuffs)
        {
            Debug.Log(key);
        }
        foreach (var (key, value) in SpeedUpBuffs)
        {
            Debug.Log(key);
        }
        foreach (var (key, value) in SlowDebuffs)
        {
            Debug.Log(key);
        }
        foreach (var (key, value) in StunDebuffs)
        {
            Debug.Log(key);
        }
        
    }


    public void AddAllBuffsToDatabase()
    {
    }
        //    if (HotBuffs.Count > 0)
        //        HotBuffs.Clear();

        //    foreach (BuffHotProperties prop in buffHotProperties)
        //    {
        //        BuffHot buff = new();
        //        buff.Name = prop.properties.Name;
        //        buff.Description = prop.properties.Description;
        //        buff.Duration = prop.properties.Duration;
        //        buff.HealAmount = prop.HealAmount;

        //        HotBuffs.Add(prop.properties.Name, buff);
        //    }

        //    if (SpeedUpBuffs.Count > 0)
        //        SpeedUpBuffs.Clear();

        //    foreach (BuffSpeedUpProperties prop in speedUpProperties)
        //    {
        //        BuffSpeedUp buff = new();
        //        buff.Name = prop.properties.Name;
        //        buff.Description = prop.properties.Description;
        //        buff.Duration = prop.properties.Duration;
        //        buff.SpeedAmount = prop.SpeedAmount;

        //        SpeedUpBuffs.Add(prop.properties.Name, buff);
        //    }

        //    if (SlowDebuffs.Count > 0)
        //        SlowDebuffs.Clear();

        //    foreach (DebuffSlowProperties prop in debuffSlowProperties)
        //    {
        //        DebuffSlow buff = new();
        //        buff.Name = prop.properties.Name;
        //        buff.Description = prop.properties.Description;
        //        buff.Duration = prop.properties.Duration;
        //        buff.SlowAmount = prop.SlowAmount;

        //        SlowDebuffs.Add(prop.properties.Name, buff);
        //    }

        //    if (StunDebuffs.Count > 0)
        //        StunDebuffs.Clear();

        //    foreach (DebuffStunProperties prop in debuffStunProperties)
        //    {
        //        DebuffStun buff = new();
        //        buff.Name = prop.properties.Name;
        //        buff.Description = prop.properties.Description;
        //        buff.Duration = prop.properties.Duration;

        //        StunDebuffs.Add(prop.properties.Name, buff);
        //    }
        //}
    }

[Serializable]
public class BaseBuffProperties
{
    public string Name = "Name";
    public string Description = "Description";
    public float Duration = float.MaxValue;    
}


//public void InitBuffsDictionarys()
//{
//    Type[] types = Assembly.GetAssembly(typeof(Buff)).GetTypes();

//    foreach (Type type in types)
//    {
//        Buff buff = Activator.CreateInstance(type) as Buff;

//        if (buff.IsPositive)
//        {
//            Buffs.Add(buff.ID, buff);
//        }
//        else
//        {
//            Debuffs.Add(buff.ID, buff);
//        }
//    }
//}
