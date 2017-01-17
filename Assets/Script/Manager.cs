using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System;

public class Manager : MonoBehaviour {
    public static Manager Instance {
        get {
            if(!_instance)
            {
                _instance= GameObject.FindObjectOfType<Manager>();
                if (_instance)
                {
                    Debug.LogWarning("场景中需要有挂载Manager的物体");
                }
            }
            return _instance;
        }
    }
    private static Manager _instance;
    public Dictionary<string,Anim> AnimationDic;
    Dictionary<string, WayPoint> WayPointDic;
    // Use this for initialization
    void Awake () {
        AnimationDic = new Dictionary<string, Anim>();
        WayPointDic = new Dictionary<string, WayPoint>();
        Anim[]allAnim= GameObject.FindObjectsOfType<Anim>();
        WayPoint[] allWay = GameObject.FindObjectsOfType<WayPoint>();
        for (int iLoop=0;iLoop<allAnim.Length;iLoop++)
        {
            AnimationDic.Add(allAnim[iLoop].name,allAnim[iLoop]);
        }
        for (int iLoop = 0; iLoop < allWay.Length; iLoop++)
        {
            WayPointDic.Add(allWay[iLoop].animName, allWay[iLoop]);
        }
        // Debug.Log(AnimationDic.Count);
    }

   

    void Start()
    {
        WaterIn();
    }
	// Update is called once per frame
	void Update () {
	
	}
    void WaterIn()
    {
        ExcuteAnimation(AnimName.Trail1, WayPointName.WayPoint1,1);
        AddNext(AnimName.Trail1,AnimName.CuGeShan1, AnimType.Glow);
        ExcuteAnimation(AnimName.Trail2, WayPointName.WayPoint2, 1);
        AddNext(AnimName.Trail2, AnimName.CuGeShan2, AnimType.Glow);

        AddNext(AnimName.CuGeShan1,AnimName.Trail1,WayPointName.WayPoint3);
        AddNext(AnimName.CuGeShan2, AnimName.Trail2, WayPointName.WayPoint4);

    }
   
    public void AddNext(string headName, string nextName, AnimType animType=AnimType.Glow, float duration=1)
    {
        if (AnimationDic.ContainsKey(headName))
        {
           // Debug.Log(headName+"后面的是"+ nextName);
            AnimationDic[headName].AddNext(nextName, animType,duration);
        }
    }
    public void AddNext(string headName,string nextName, string wayPointName, float duration=1)
    {
        if (AnimationDic.ContainsKey(headName))
        {
          //  Debug.Log(headName + "后面的是" + nextName);
            AnimationDic[headName].AddNext(nextName,wayPointName, duration);
        }
    }
    public void AddNext(string animName, Anim tailAnim)
    {
        AddNext(animName, tailAnim.AnimName, tailAnim.animType, tailAnim.duration);
    }
    public void ExcuteAnimation(Anim nextAnim)
    {
        ExcuteAnimation(nextAnim.name, nextAnim.animType, nextAnim.duration);
    }
    public void ExcuteAnimation(string name, string wayPointName, float duration = 1)
    {
        if (AnimationDic.ContainsKey(name))
        {
            if(WayPointDic.ContainsKey(wayPointName))
                AnimationDic[name].Play(WayPointDic[wayPointName].targetPoint,duration);
        }
    }
    public void ExcuteAnimation(string name,AnimType animType=AnimType.Glow,float duration=1)
    {
        if (AnimationDic.ContainsKey(name))
        {
            AnimationDic[name].Play(animType, duration);
            Debug.Log("播放"+ name + "的" + animType.ToString());
        }
    }
   
   
    
}
