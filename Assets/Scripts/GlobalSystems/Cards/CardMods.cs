using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Database
{
    public class Card
    {
        public CH_Stats Stats { get; protected set; }

        public Sprite Sprite { get; protected set; }
        public string Name { get; protected set; } = "Base";
        public string ID { get; protected set; } = Guid.NewGuid().ToString();
        public byte Tier { get; protected set; }
        public float[] TierValues { get; protected set; }
        public ModTag[] ModTags { get; protected set; } = new ModTag[5];
        public virtual string Description() { return string.Empty; }
        public void SetStats(CH_Stats stats) => Stats = stats;
        public void SetTier(byte tier) => Tier = tier;
        public virtual void ApplyCard(byte tier) { return; }
        public virtual void RemoveCard(byte tier) { return; }
    }

    public class CardIncreaseAS : Card
    {
        public CardIncreaseAS()
        {
            Name = "CardIncreaseAttackSpeed";
            Stats = null;
            ModTags[0] = ModTag.Speed;
            TierValues = new float[] {15, 10, 5};
            Tier = 0;
            Sprite = GameDatabasesManager.Instance.CardsDatabase.CardsSprites["Aim"];
        }

        public override string Description() => $"Increase attack speed by {TierValues[Tier]}%";

        public override void ApplyCard(byte tier) => Stats.GSC.AttackSC.IncreaseAttackSpeed(TierValues[tier]);
    }
    public class CardIncreaseMS : Card
    {
        public CardIncreaseMS()
        {
            Name = "CardIncreaseMoveSpeed";
            Stats = null;
            ModTags[0] = ModTag.Speed;
            TierValues = new float[] { 15, 10, 5 };
            Tier = 0;
            Sprite = GameDatabasesManager.Instance.CardsDatabase.CardsSprites["Aim"];
        }

        public override string Description() => $"Increase move speed by {TierValues[Tier]}%";

        public override void ApplyCard(byte tier) => Stats.GSC.UtilitySC.IncreaseMovementSpeed(TierValues[tier]);
    }
    public class CardIncreaseRS : Card
    {
        public CardIncreaseRS()
        {
            Name = "CardIncreaseReloadSpeed";
            Stats = null;
            ModTags[0] = ModTag.Speed;
            ModTags[1] = ModTag.Reload;
            TierValues = new float[] { 15, 10, 5 };
            Tier = 0;
            Sprite = GameDatabasesManager.Instance.CardsDatabase.CardsSprites["Aim"];
        }

        public override string Description() => $"Increase reload speed by {TierValues[Tier]}%";

        public override void ApplyCard(byte tier) => Stats.GSC.AttackSC.IncreaseReloadSpeed(TierValues[tier]);
    }
}