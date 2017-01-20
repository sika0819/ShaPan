using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class AnimArea : MonoBehaviour {
    public string AnimName="";
    void Awake()
    {
        AnimName = name;
    }
}
