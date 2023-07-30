using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputSettings", menuName = "ScriptableObjects/PlayerInputSettings")]
public class PlayerInputSettings : ScriptableObject
{
    public KeyCode UP;
    public KeyCode DOWN;
    public KeyCode LEFT;
    public KeyCode RIGHT;

    [Space(10)]
    public KeyCode SHOOT;
    public KeyCode ALTERNATIVESHOOT;
    public KeyCode RELOAD;

    [Space(10)]
    public KeyCode ABILITY_1;
    public KeyCode ABILITY_2;
    public KeyCode ABILITY_3;
    public KeyCode ABILITY_4;
    public KeyCode ABILITY_5;
    public KeyCode ABILITY_6;

    [Space(10)]
    public KeyCode INVENTORY;
    public KeyCode SWAPEQUIPMENTSET;
    public KeyCode TALENTS;

    [Space(10)]
    public KeyCode PAUSE;

    [Space(10)]
    public KeyCode CONSOLE;
    public KeyCode ENTER;
}
