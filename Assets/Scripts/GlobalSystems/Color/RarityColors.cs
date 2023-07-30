using System.Collections.Generic;
using UnityEngine;

public class RarityColors
{
    public static Dictionary<Rarity, Color> Color = new()
    {
        { Rarity.Common, new Color(To01(78), To01(95), To01(106)) },
        { Rarity.Uncommon, new Color(To01(10), To01(235), 0) },
        { Rarity.Rare, new Color(To01(30), To01(59), To01(112)) },
        { Rarity.Epic, new Color(To01(163), To01(53), To01(238)) },
        { Rarity.Mythic, new Color(To01(0), To01(184), To01(235)) },
        { Rarity.Legendary, new Color(To01(235), To01(108), To01(0)) },
    };

    private const float ColorTo01 = 0.0039216f;

    private static float To01(int c)
    {
        return ColorTo01 * c;
    }
}
