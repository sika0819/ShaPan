﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
[Serializable]
[ExecuteInEditMode]
public class TextGenerate : MonoBehaviour {
    Text Description;
    delegate void OnTextChanged(string Value);
    event OnTextChanged OnTextContentChanged;
    [SerializeField]
    public TextAsset textAsset;
    [SerializeField]
    public string TextContent {
        get {
            return _text;
        }set {
            if (value != null)
            {
                _text = value;
                OnResetText(value);
            }
        }
    }
    private string _text;
    void Awake()
    {
        Description = GetComponent<Text>();
        OnTextContentChanged += OnResetText;
    }
    void OnResetText(string Value)
    {
        string result="";
        string[] valueArray = Value.Split('\n');
        for (int iLoop = 0; iLoop < valueArray.Length; iLoop++)
        {
            result += "\u3000" + valueArray[iLoop] + "\n";
        }
        Description.text = result;
    }
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
