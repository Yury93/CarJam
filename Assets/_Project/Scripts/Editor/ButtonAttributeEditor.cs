using UnityEngine;
using UnityEditor;
using System;
using System.Reflection; 
using _Project.Scripts.Helper;

namespace _Project.Scripts 
{
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class ButtonAttributeEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (MethodInfo method in methods)
            {
                ButtonAttribute buttonAttribute = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));

                if (buttonAttribute != null)
                {
                    if (GUILayout.Button(buttonAttribute.ButtonName))
                    {
                        method.Invoke(target, null);
                    }
                }
            }
        }
    }

 
 
}