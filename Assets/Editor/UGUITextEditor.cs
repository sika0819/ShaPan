using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

[CustomEditor(typeof(TextGenerate))]
public class UGUITextEditor:Editor {

    public string text = "Nothing Opened...";

    public TextAsset txtAsset;
    Vector2 scroll;

    public override void OnInspectorGUI()
    {
        TextGenerate textGen = target as TextGenerate;
        TextAsset newTxtAsset = EditorGUILayout.ObjectField("添加", txtAsset, typeof(TextAsset), true) as TextAsset;
        textGen.textAsset = newTxtAsset;
        if (newTxtAsset != txtAsset)
            ReadTextAsset(newTxtAsset);

        scroll = EditorGUILayout.BeginScrollView(scroll);
        text = EditorGUILayout.TextArea(text, GUILayout.Height(300),GUILayout.Width(300));
        textGen.TextContent = text;
        EditorGUILayout.EndScrollView();
    }


    private void ReadTextAsset(TextAsset newTxtAsset)
    {
        text = newTxtAsset.text;
        txtAsset =newTxtAsset;
       
    }
}
