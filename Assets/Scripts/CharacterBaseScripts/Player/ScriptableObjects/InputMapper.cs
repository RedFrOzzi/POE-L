using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMapper : MonoBehaviour
{
    private bool IsActive;
    [SerializeField] private PlayerInputSettings inputSettings;


    private void Update()
    {
        if (IsActive == false) { return; }

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode))
                Debug.Log("KeyCode down: " + kcode);
        }
    }
}
