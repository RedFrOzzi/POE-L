using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class ArmorMods
    {
        public static List<ModForArmor> GetArmorMods()
        {
            List<ModForArmor> mods = new();

            ModForArmor m = new();

            //---Armor---
            m.Name = "IncreaseLocalArmor";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Armor;
            m.ModTags[1] = ModTag.Defance;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL armor by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseArmor(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseArmor(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatLocalArmor";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Armor;
            m.ModTags[1] = ModTag.Defance;
            m.TierValues = new float[] { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]} to LOCAL armor";
            m.ApplyMod = (m) => (m as ModForArmor).Armor.LSC.DefanceSC.AddFlatArmor((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForArmor).Armor.LSC.DefanceSC.AddFlatArmor(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            //---Health---
            m = new();
            m.Name = "IncreaseLocalHP";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Health;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL health by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseHP(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseHP(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatLocalHP";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Health;
            m.TierValues = new float[] { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]} to LOCAL health points";
            m.ApplyMod = (m) => (m as ModForArmor).Armor.LSC.DefanceSC.AddFlatHP((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForArmor).Armor.LSC.DefanceSC.AddFlatHP(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            //---Magic-Resist---
            m = new();
            m.Name = "IncreaseLocalMagicResist";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.MagicResist;
            m.ModTags[1] = ModTag.Defance;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL magic resist by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseMagicResist(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseMagicResist(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatLocalMagicResist";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.MagicResist;
            m.ModTags[1] = ModTag.Defance;
            m.TierValues = new float[] { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]} to LOCAL magic resist";
            m.ApplyMod = (m) => (m as ModForArmor).Armor.LSC.DefanceSC.AddFlatMagicResist((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForArmor).Armor.LSC.DefanceSC.AddFlatMagicResist(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            //---Hybrid---
            m = new();
            m.Name = "HybridLocalHealthMResist";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Health;
            m.ModTags[1] = ModTag.MagicResist;
            m.ModTags[2] = ModTag.Defance;
            m.TierValues = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierValues2 = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL health points by {m.TierValues[m.Tier]}% and magic resist by {m.TierValues2[m.Tier]}%";
            m.ApplyMod = (m) =>
            {
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseHP(m.TierValues[m.Tier]);
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseMagicResist(m.TierValues2[m.Tier]);
            };
            m.RemoveMod = (m) =>
            {
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseHP(-m.TierValues[m.Tier]);
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseMagicResist(-m.TierValues2[m.Tier]);
            };
            mods.Add(m);

            m = new();
            m.Name = "HybridLocalHealthArmor";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Health;
            m.ModTags[1] = ModTag.MagicResist;
            m.ModTags[2] = ModTag.Defance;
            m.TierValues = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierValues2 = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL health points by {m.TierValues[m.Tier]}% and armor by {m.TierValues2[m.Tier]}%";
            m.ApplyMod = (m) =>
            {
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseHP(m.TierValues[m.Tier]);
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseArmor(m.TierValues2[m.Tier]);
            };
            m.RemoveMod = (m) =>
            {
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseHP(-m.TierValues[m.Tier]);
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseArmor(-m.TierValues2[m.Tier]);
            };
            mods.Add(m);

            m = new();
            m.Name = "HybridLocalArmorMResist";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Health;
            m.ModTags[1] = ModTag.MagicResist;
            m.ModTags[2] = ModTag.Defance;
            m.TierValues = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierValues2 = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL magic resist by {m.TierValues[m.Tier]}% and armor by {m.TierValues2[m.Tier]}%";
            m.ApplyMod = (m) =>
            {
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseMagicResist(m.TierValues[m.Tier]);
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseArmor(m.TierValues2[m.Tier]);
            };
            m.RemoveMod = (m) =>
            {
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseMagicResist(-m.TierValues[m.Tier]);
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseArmor(-m.TierValues2[m.Tier]);
            };
            mods.Add(m);

            m = new();
            m.Name = "HybridLocalDoubleHealth";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Health;
            m.TierValues = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierValues2 = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]} to LOCAL health points and increase LOCAL health by {m.TierValues2[m.Tier]}%";
            m.ApplyMod = (m) =>
            {
                (m as ModForArmor).Armor.LSC.DefanceSC.AddFlatHP((int)m.TierValues[m.Tier]);
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseHP(m.TierValues2[m.Tier]);
            };
            m.RemoveMod = (m) =>
            {
                (m as ModForArmor).Armor.LSC.DefanceSC.AddFlatHP(-(int)m.TierValues[m.Tier]);
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseHP(-m.TierValues2[m.Tier]);
            };
            mods.Add(m);

            m = new();
            m.Name = "HybridLocalDoubleArmor";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Armor;
            m.ModTags[1] = ModTag.Defance;
            m.TierValues = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierValues2 = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]} to LOCAL armor and increase LOCAL armor by {m.TierValues2[m.Tier]}%";
            m.ApplyMod = (m) =>
            {
                (m as ModForArmor).Armor.LSC.DefanceSC.AddFlatArmor((int)m.TierValues[m.Tier]);
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseArmor(m.TierValues2[m.Tier]);
            };
            m.RemoveMod = (m) =>
            {
                (m as ModForArmor).Armor.LSC.DefanceSC.AddFlatArmor(-(int)m.TierValues[m.Tier]);
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseArmor(-m.TierValues2[m.Tier]);
            };
            mods.Add(m);

            m = new();
            m.Name = "HybridLocalDoubleMResist";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.MagicResist;
            m.ModTags[1] = ModTag.Defance;
            m.TierValues = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierValues2 = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]} to LOCAL magic resist and increase LOCAL magic resist by {m.TierValues2[m.Tier]}%";
            m.ApplyMod = (m) =>
            {
                (m as ModForArmor).Armor.LSC.DefanceSC.AddFlatMagicResist((int)m.TierValues[m.Tier]);
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseMagicResist(m.TierValues2[m.Tier]);
            };
            m.RemoveMod = (m) =>
            {
                (m as ModForArmor).Armor.LSC.DefanceSC.AddFlatMagicResist(-(int)m.TierValues[m.Tier]);
                (m as ModForArmor).Armor.LSC.DefanceSC.IncreaseMagicResist(-m.TierValues2[m.Tier]);
            };
            mods.Add(m);

            return mods;
        }
    }
}