using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[System.Serializable]
public class UIGradient
{
    public string ColorDefName;
    public Gradient colorValue;
    public string ColorComment;
    public UIGradient(string name, Gradient color, string comm)
    {
        ColorDefName = name;
        colorValue = color;
        ColorComment = comm;
    }
}


[CreateAssetMenu(fileName = "ui_gradient_config", menuName = "UXTools/UI Gradient Asset")]
public class UIGradientAsset : ScriptableObject
{
    [SerializeField]
    [NaughtyAttributes.ShowInInspector]
    public List<UIGradient> defList = new List<UIGradient>(){
        new UIGradient("渐变1", new Gradient(){
            colorKeys = new GradientColorKey[3] {
        // Add your colour and specify the stop point
                new GradientColorKey(new Color(1, 0, 0), 0),
                new GradientColorKey(new Color(131f/255f, 1, 251f/255f), 0.453f),
                new GradientColorKey(new Color(125f/255f, 8f/255f, 180f/255f), 1)
            },
                // This sets the alpha to 1 at both ends of the gradient
                alphaKeys = new GradientAlphaKey[3] {
                new GradientAlphaKey(1, 0),
                new GradientAlphaKey(198f/255f, 0.679f),
                new GradientAlphaKey(1, 1)
            }
        },"渐变1")
    };
    public bool HasSameNameOrEmptyName()
    {
        List<string> names = new List<string>();
        foreach (var single in defList)
        {
            if (string.IsNullOrEmpty(single.ColorDefName))
            {
                return true;
            }
            if (names.Contains(single.ColorDefName))
            {
                return true;
            }
            names.Add(single.ColorDefName);
        }
        return false;
    }
#if UNITY_EDITOR
    public void GenGradientScript()
    {
        string needGenCsCode = "// This file is generated by code\n";
        needGenCsCode += "public sealed class UIGradientGenDef {\n\tpublic enum UIGradientConfigDef {\n";
        foreach (var single in defList)
        {
            needGenCsCode += $"\t\tDef_{single.ColorDefName.ToUpper()} = {Animator.StringToHash(single.ColorDefName)},\n";
        }
        needGenCsCode += "\t}\n}";
        string path = $"{UXGUIConfig.ScriptsRootPath}Runtime/Feature/UIColor/UIGradientGenDef.cs";
        if (!File.Exists(path) || File.ReadAllText(path) != needGenCsCode)
        {
            File.WriteAllText(path, needGenCsCode);
        }
        AssetDatabase.SaveAssets();
    }
#endif
}
