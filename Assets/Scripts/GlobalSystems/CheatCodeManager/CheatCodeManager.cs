using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System;

public class CheatCodeManager : MonoBehaviour
{
    public static CheatCodeManager Instance;

    private bool isActive;

    private readonly List<CheatCodeBase> cheatCodes = new();

    string input;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        var types = Assembly.GetAssembly(typeof(CheatCodeBase))
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(CheatCodeBase)) && !t.IsAbstract)
            .ToArray();
        
        foreach (var type in types)
        {
            var cheatCodeBase = Activator.CreateInstance(type) as CheatCodeBase;

            if (cheatCodeBase.CheatName != null)
            {
                cheatCodes.Add(cheatCodeBase);
            }
            
        }
    }

    public void ToggleConsole()
    {
        isActive = !isActive;

        input = "";
    }

    public void HandleInput()
    {
        if (input == null) { return; }

        string[] props = input.Split('_');

        foreach (var code in cheatCodes)
        {
            if (input.Contains(code.CheatName))
            {
                if (code as CheatCode != null)
                {
                    var c = (CheatCode)code;
                    c.ApplyCommand();
                    Debug.Log(c.CheatName + " is activated");
                }
                else if (code as CheatCodeWithParams != null)
                {
                    var c = (CheatCodeWithParams)code;

                    if (props.Length > 1)
                    {
                        c.ApplyCommand(float.Parse(props[1]));
                        Debug.Log(c.CheatName + " is activated");
                    }
                }
            }
            else
            {
                Debug.Log($"there is no code with name <{input}>");
            }
        }

        if (isActive)
        {
            ToggleConsole();
        }
    }

    private void OnGUI()
    {
        if (!isActive) { return; }

        float y = Screen.height - 30f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }
}
