using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[CreateAssetMenu(fileName = "New CheatSheet", menuName = "CheatSheet")]
public class CheatSheet : ScriptableObject
{
    [SerializeField] private Cheat[] usableCheats;

    private Dictionary<string, Cheat> cheats;

    private void OnValidate()
    {
        cheats = new Dictionary<string, Cheat>();
        foreach (Cheat item in usableCheats)
        {
            cheats.Add(item.Command, item);
        }
    }

    public void ProcessInput(string command)
    {
        Debug.Log($"Processing command: {command}");

        Cheat cheatToUse = null;
        cheats.TryGetValue(command, out cheatToUse);

        if (cheatToUse == null)
        {
            Debug.LogError("Cheat does not exist!");
            return;
        }

        MethodInfo method = typeof(CheatCodeLogic).GetMethod(cheatToUse.FunctionName);
        method.Invoke(new CheatCodeLogic(), null);
    }
    
    // public void ProcessInput(string input)
    // {
    //     Debug.Log($"Processing command: {input}");
    //
    //     string[] commandParts = input.Split(' ');
    //     string command = commandParts[0];
    //     
    //     Cheat cheatToUse = null;
    //     cheats.TryGetValue(command, out cheatToUse);
    //
    //     if (cheatToUse == null)
    //     {
    //         Debug.LogError("Cheat does not exist!");
    //         return;
    //     }
    //
    //     object[] parameters = new object[cheatToUse.Arguments.Length];
    //     for (int i = 0; i < cheatToUse.Arguments.Length; i++)
    //     {
    //         parameters[i] = GetArgumentValue(cheatToUse.Arguments[i], commandParts[i + 1]);
    //     }
    //
    //     MethodInfo method = typeof(CheatCodeLogic).GetMethod(cheatToUse.FunctionName);
    //     method.Invoke(new CheatCodeLogic(), parameters);
    // }
    //
    // private object GetArgumentValue(ArgumentType argumentType, string value)
    // {
    //     switch (argumentType)
    //     {
    //         case ArgumentType.None:
    //             return null;
    //         case ArgumentType.Int:
    //             return System.Convert.ToInt32(value);
    //         case ArgumentType.Float:
    //             return System.Convert.ToSingle(value);
    //         case ArgumentType.String:
    //             return value;
    //         default:
    //             return null;
    //     }
    // }
}
