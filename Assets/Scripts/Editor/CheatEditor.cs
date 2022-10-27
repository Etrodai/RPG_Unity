using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DebugMenus;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(Cheat))]
    public class CheatEditor : PropertyDrawer
    {
        private const float Y_OFFSET = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2f + Y_OFFSET;
            // return base.GetPropertyHeight(property, label) * 10f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int selected = 0;
            SerializedProperty command = property.FindPropertyRelative("Command");
            SerializedProperty functionName = property.FindPropertyRelative("FunctionName");
            // SerializedProperty arguments = property.FindPropertyRelative("Arguments");

            Rect commandRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            Rect functionNameRect = new Rect(position.x, commandRect.y + EditorGUIUtility.singleLineHeight + Y_OFFSET,
                position.width, EditorGUIUtility.singleLineHeight);
            // float height = arguments.isExpanded ? EditorGUIUtility.singleLineHeight * (arguments.arraySize) : EditorGUIUtility.singleLineHeight;
            // Rect argumentsRect = new Rect(position.x, functionNameRect.position.y + functionNameRect.height + Y_OFFSET, position.width, height);

            EditorGUI.BeginProperty(commandRect, label, command);
            EditorGUI.PropertyField(commandRect, command);
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(functionNameRect, label, functionName);
            string[] cheatMethods = GetCheatMethods();
            for (int i = 0; i < cheatMethods.Length; i++)
            {
                if (functionName.stringValue == cheatMethods[i])
                {
                    selected = i;
                    break;
                }
            }

            selected = EditorGUI.Popup(functionNameRect, selected, cheatMethods);
            functionName.stringValue = cheatMethods[selected];
            EditorGUI.EndProperty();

            // EditorGUI.BeginProperty(argumentsRect, label, arguments);
            // EditorGUI.PropertyField(argumentsRect, arguments, true);
            // EditorGUI.EndProperty();
        }

        private string[] GetCheatMethods()
        {
            List<MethodInfo> methods = new List<MethodInfo>();
            List<string> methodNames = new List<string>();

            methods.AddRange(typeof(CheatCodeLogic).GetMethods()
                .Where(method => method.DeclaringType == typeof(CheatCodeLogic)));

            foreach (MethodInfo item in methods)
            {
                methodNames.Add(item.Name);
            }

            return methodNames.ToArray();
        }
    }
}
