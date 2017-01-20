using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System;

public class Manager : MonoBehaviour {
    private static GameObject container;
    public static Manager Instance {
        get {
            if(!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(Manager)) as Manager;
                if (_instance)
                {
                    container = new GameObject();
                    container.name = "Manager";
                    _instance = container.AddComponent(typeof(Manager)) as Manager;
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
        WayPoint[] allWay= GameObject.FindObjectsOfType<WayPoint>();
        for (int iLoop=0;iLoop<allAnim.Length;iLoop++)
        {
            AnimationDic.Add(allAnim[iLoop].name,allAnim[iLoop]);
        }
        for (int iLoop = 0; iLoop < allWay.Length; iLoop++)
        {
            WayPointDic.Add(allWay[iLoop].name, allWay[iLoop]);
        }
       
    }

   

    void Start()
    {
       // ExcuteAll();
       // WaterIn(7,7);
    }
	// Update is called once per frame
	void Update () {
	    
	}
    public void ExcuteAll() {
        StartCoroutine(ExcuteAllAnim());
    }
    IEnumerator ExcuteAllAnim()
    {
        yield return new WaitForSeconds(1);
        DisActiveAll(1);
        yield return new WaitForSeconds(1);
        RemainAll(1);
        yield return new WaitForSeconds(1);
        WaterIn(10, 10);
        yield return new WaitForSeconds(11);
        RemainAll(1);
        yield return new WaitForSeconds(1);
        ConsoleWater(10, 10);
        yield return new WaitForSeconds(11);
        RemainAll(1);
        yield return new WaitForSeconds(1);
        CleanWaterOut(10, 10);
        yield return new WaitForSeconds(2);
        DirtyOut(15, 10);
        yield return new WaitForSeconds(20);
        RemainAll(1);
    }
    public void RemainAll(float duration)
    {
        foreach (KeyValuePair<string, Anim> item in AnimationDic)
        {
            ExcuteAnimation(item.Key,item.Value.originColor,duration);
        }
    }
    public void DisActiveAll(float duration)
    {
        foreach (KeyValuePair<string,Anim> item in AnimationDic)
        {
            float r = 245f / 255;
            float g = 174f / 255;
            float b = 27f / 255;
            Color nomalcolor = new Color(r, g, b, 1);
           // Debug.Log(nomalcolor);
            ExcuteAnimation(item.Key,nomalcolor, duration);
        }
    }
    public void WaterIn(float duration,float step)
    {
        float stepDuration = duration / step;
        ExcuteAnimation(AnimName.Trail1, WayPointName.WayPoint1,stepDuration);
        ExcuteAnimation(AnimName.Trail2, WayPointName.WayPoint2, stepDuration);
        AddNext(AnimName.Trail1,AnimName.CuGeShan1, AnimType.Glow, stepDuration);
        AddNext(AnimName.Trail2, AnimName.CuGeShan2, AnimType.Glow, stepDuration);
        AddNext(AnimName.CuGeShan1, AnimName.Trail3, WayPointName.WayPoint3, stepDuration);
        AddNext(AnimName.CuGeShan2, AnimName.Trail4, WayPointName.WayPoint4, stepDuration);
        AddNext(AnimName.Trail3, AnimName.XiGeShan1, AnimType.Glow, stepDuration);
        AddNext(AnimName.Trail4, AnimName.XiGeShan2, AnimType.Glow, stepDuration);
        AddNext(AnimName.XiGeShan1, AnimName.ChenShanChi, AnimType.Glow, stepDuration);
        AddNext(AnimName.ChenShanChi, AnimName.Trail5, WayPointName.WayPoint5, stepDuration);
        AddNext(AnimName.Trail5, AnimName.FirstChenDian, AnimType.Glow, stepDuration);
    }
    public void ConsoleWater(float duration,float step)
    {
        float stepDuration = duration / step;
        ExcuteAnimation(AnimName.FirstChenDian,AnimType.Glow,stepDuration);
        ExcuteAnimation(AnimName.GuFengJi, AnimType.Glow, stepDuration);
        ExcuteAnimation(AnimName.Trail6, WayPointName.WayPoint6, stepDuration);
        AddNext(AnimName.GuFengJi, AnimName.Trail7, WayPointName.WayPoint7, stepDuration);
        AddNext(AnimName.Trail6, AnimName.YanYang,AnimType.Glow,stepDuration);
        AddNext(AnimName.YanYang, AnimName.Trail8,WayPointName.WayPoint8,stepDuration);
        AddNext(AnimName.Trail8, AnimName.Trail9, WayPointName.WayPoint9, stepDuration);
        AddNext(AnimName.Trail9, AnimName.Trail10, WayPointName.WayPoint10, stepDuration);
        AddNext(AnimName.Trail10, AnimName.SecondChenDian, AnimType.Glow, stepDuration);
    }
    public void CleanWaterOut(float duration, float step)
    {
        float stepDuration = duration / step;
        ExcuteAnimation(AnimName.SecondChenDian, AnimType.Glow, stepDuration);
        AddNext(AnimName.SecondChenDian,AnimName.Trail11, WayPointName.WayPoint11, stepDuration);
        AddNext(AnimName.Trail11,AnimName.XiaoDuPool, AnimType.Glow, stepDuration);
        AddNext(AnimName.XiaoDuPool, AnimName.Trail12, WayPointName.WayPoint12, stepDuration);
        AddNext(AnimName.Trail12, AnimName.Trail13, WayPointName.WayPoint13, stepDuration);
        AddNext(AnimName.Trail13, AnimName.HunNingPool, AnimType.Glow, stepDuration);
        AddNext(AnimName.HunNingPool, AnimName.Trail14, WayPointName.WayPoint14, stepDuration);
        AddNext(AnimName.Trail14,AnimName.TanPool,AnimType.Glow,stepDuration);
        AddNext(AnimName.TanPool,AnimName.Trail15,WayPointName.WayPoint15,stepDuration);
    }
    public void DirtyOut(float duration, float step)
    {
        float stepDuration = duration / step;
        ExcuteAnimation(AnimName.FirstChenDian, AnimType.Glow, stepDuration);
        ExcuteAnimation(AnimName.SecondChenDian, AnimType.Glow, stepDuration);
        AddNext(AnimName.FirstChenDian,AnimName.Trail16, WayPointName.WayPoint16, stepDuration);
        AddNext(AnimName.SecondChenDian, AnimName.Trail17, WayPointName.WayPoint17, stepDuration);
        AddNext(AnimName.Trail16, AnimName.DirtyPool, AnimType.Glow, stepDuration);
        AddNext(AnimName.DirtyPool, AnimName.Trail18, WayPointName.WayPoint18, stepDuration);
        AddNext(AnimName.Trail18, AnimName.LuoYa, AnimType.Glow, stepDuration);
        AddNext(AnimName.LuoYa,AnimName.Trail19,WayPointName.WayPoint19,stepDuration);
        AddNext(AnimName.Trail19, AnimName.DuoJiXiaoHuaPool, AnimType.Glow, stepDuration);
        AddNext(AnimName.DuoJiXiaoHuaPool, AnimName.Trail20, WayPointName.WayPoint20, stepDuration);
        AddNext(AnimName.Trail20, AnimName.LiXinTuoShui, AnimType.Glow, stepDuration);
        AddNext(AnimName.LiXinTuoShui, AnimName.Trail21, WayPointName.WayPoint21, stepDuration);
        
    }
    public void AddNext(string headName, string nextName, AnimType animType=AnimType.Glow, float duration=1)
    {
        if (AnimationDic.ContainsKey(headName))
        {
          //  Debug.Log(headName+"后面的是"+ nextName);
            AnimationDic[headName].AddNext(nextName, animType,duration);
        }
    }
    public void AddNext(string headName,string nextName, string wayPointName, float duration=1)
    {
        if (AnimationDic.ContainsKey(headName))
        {
            AnimationDic[headName].AddNext(nextName, wayPointName, duration);
            if (WayPointDic.ContainsKey(wayPointName))
            {
                AnimationDic[nextName].transform.localPosition = WayPointDic[wayPointName].targetPoint[0];

            }
            else
            {
                Debug.LogError("路径不存在");
            }
         //   Debug.Log(headName + "后面的是" + nextName);

        }
        else {
            Debug.LogError("下一个动画不存在");
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
    
    void ExcuteAnimation(string animName, Color targetColor, float duration)
    {
        if (AnimationDic.ContainsKey(animName))
        {
            AnimationDic[animName].Play(targetColor, duration);
        }
        else
        {
            Debug.LogError("动画不存在");
        }
    }
    public void ExcuteAnimation(string animName, string wayPointName, float duration = 1)
    {
        
            if (WayPointDic.ContainsKey(wayPointName))
            {
                AnimationDic[animName].Play(WayPointDic[wayPointName].targetPoint, duration);
            }
            else
            {
                Debug.LogError("路径不存在");
            }
    
    }
    public void ExcuteAnimation(string name,AnimType animType=AnimType.Glow,float duration=1)
    {
        if (AnimationDic.ContainsKey(name))
        {
            AnimationDic[name].Play(animType, duration);
            //Debug.Log("播放"+ name + "的" + animType.ToString());
        }
    }
   
   
    
}
