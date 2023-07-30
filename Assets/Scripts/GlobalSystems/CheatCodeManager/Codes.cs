using System;
using UnityEngine;

public class CheatExperience : CheatCodeWithParams
{
    public CheatExperience()
    {
        CheatName = "getexp";
        CheatDescription = "Gives certain amount of experience";
        cheatCommand = (t) =>
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<CH_Experience>().GainExp(t[0]);
        };
    }
}

public class CheatMana : CheatCode
{
    public CheatMana()
    {
        CheatName = "getmana";
        CheatDescription = "Gives certain amount of mana";
        cheatCommand = () =>
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<CH_Mana>().ReplanishMana(1000);
        };
    }
}

public class CheatLevel : CheatCodeWithParams
{
    public CheatLevel()
    {
        CheatName = "getlvl";
        CheatDescription = "Gives certain amount of levels";
        cheatCommand = (x) =>
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<CH_TalentTree>().AddTalentPoints((byte)x[0]);
        };
    }
}

//public class CheatGetItem : CheatCodeGeneric<Item>
//{
//    public CheatGetItem()
//    {
//        CheatName = "getmana";
//        CheatDescription = "Gives certain amount of mana";
//        cheatCommand = (t) =>
//        {
//            GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().AddItem(t);
//        };
//    }
//}