using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

[ExecuteInEditMode]
public class Anim : MonoBehaviour {
    public string AnimName;
    public AnimType animType;
    public float duration=1;
    public bool isRepeat = false;
    Image image;
    Vector3 originPos;
    public Color originColor;
    Color targetColor;
    [HideInInspector]
    public Vector3[] path;
    public string WayPointName;
    public Anim NextAnim;
    Tweener pathTween;
  //  private AnimType playAnimType = AnimType.Glow;//默认为发光动画
    // Use this for initialization
    void Start () {
        NextAnim = new Anim();
        AnimName = gameObject.name;
        image = GetComponent<Image>();
        originPos = transform.localPosition;
        if (image)
        {
            originColor = image.color;
        }
        Init(animType);
    }
    void Init(AnimType animType)
    {
       // Color none = new Color(0, 0, 0, 0);
        switch (animType)
        {
            case AnimType.Glow:
                if (image)
                {
                    image.color = originColor;
                }
                break;
            case AnimType.Translate:
              //  transform.localPosition = originPos;
                break;
            case AnimType.ChangeColor:
                if (image)
                {
                    image.color = originColor;
                }
                break;
            case AnimType.Fade:
                if (image)
                {
                    image.color = originColor;
                }
                break;
            case AnimType.None:
                OnFinshed();
                break;
        }
    }
	// Update is called once per frame
	void Update () {
        if (animType == AnimType.Path)
        {
            if (pathTween != null)
            {
                if (pathTween.CompletedLoops() == 1)
                {
                    OnFinshed();
                }
            }
        }
    }
    public void Play(AnimType animType=AnimType.Glow,float duration=1)
    {//默认为发光动画1秒
        this.animType = animType;
        this.duration = duration;
        Init(animType);//初始化
        switch (animType)
        {
            case AnimType.Glow:
                Glow(duration);
                break;
            case AnimType.Fade:
                Fade(duration);
                break;
            case AnimType.ScaleIn:
                ScaleIn(duration);
                break;
            case AnimType.ScaleOut:
                ScaleOut(duration);
                break;
            case AnimType.None:
                break;
    
        }
    }
  
    public void Play(Color target, float duration)
    {
        animType = AnimType.ChangeColor;
        this.targetColor = target;
        ChangeColor(target, duration);
    }
    public void ChangeColor(Color target,float duration)
    {
       // Debug.Log(target);
        if (image)
        {
            image.DOColor(target, duration).OnComplete(OnFinshed);
        }
    }
    public void Play(Vector3 target, float duration = 1)
    {
        this.animType = AnimType.Translate;
        Translate(target, duration);
    }
    public void Play(Vector3[] targetPath, float duration = 1)
    {
        this.animType = AnimType.Path;
        DoPath(targetPath, duration);
    }
    public void Glow(float duration)
    {
        if (image)
        {
            image.DOColor(Color.white, duration).OnComplete(OnFinshed);
        }
    }
    public void Translate(Vector3 targetPos, float duration=1)
    {
        transform.DOLocalMove(targetPos, duration).OnComplete(OnFinshed);
        
    }

    public void DoPath(Vector3[] path,float duration)
    {
        this.path = path;
        pathTween= transform.DOLocalPath(path, duration, PathType.Linear).OnPlay(OnStarted).OnComplete(OnFinshed).SetLoops(-1,LoopType.Yoyo);
       
    }
    
    public void Fade(float duration) {
        if (image)
        {
            image.DOFade(0,duration);//用这个不用DoColor,因为不管什么颜色都得有消失。直接变alpha
        }
    }
    public void ScaleIn(float duration) {
        if (image)
        {
            image.rectTransform.DOScale(1, duration).OnComplete(OnFinshed);
        }
    }
    public void ScaleOut(float duration)
    {
        if (image)
        {
            image.rectTransform.DOScale(0, duration).OnComplete(OnFinshed);
        }
    }
    public void AddNext(Anim anim)
    {
        NextAnim = anim;
       // Debug.Log(NextAnim.AnimName);
    }
    public void AddNext(string Name, AnimType animType, float duration)
    {
        foreach (KeyValuePair<string, Anim> item in Manager.Instance.AnimationDic)
        {
            if (item.Key == Name)
            {
                //NextAnim = item.Value;
              //  Debug.Log(Name);
                item.Value.AnimName = Name;
                item.Value.animType = animType;
                item.Value.duration = duration;
                NextAnim = item.Value;
             }
        }
    }
    public void AddNext(string Name,string wayPointName, float duration)
    {
        foreach (KeyValuePair<string, Anim> item in Manager.Instance.AnimationDic)
        {
            if (item.Key == Name)
            {
                //NextAnim = new Anim();
                // NextAnim = item.Value;
                item.Value.AnimName = Name;
                item.Value.animType = AnimType.Path;
                item.Value.WayPointName = wayPointName;
               // Debug.Log(item.Key + "" + wayPointName);
                item.Value.duration = duration;
                NextAnim = item.Value;
                item.Value.gameObject.SetActive(true);
            }
        }
    }
    void OnStarted()
    {
        if (animType == AnimType.Path)
        {
            // transform.localPosition = originPos;
          //  gameObject.SetActive(true);
            TrailRenderer trail = GetComponent<TrailRenderer>();
            trail.Clear();
            trail.enabled = true;
            pathTween.Play();
            pathTween.SetLoops(-1);
        }
    }
    public void OnFinshed()
    {
       // Debug.Log("完成动画：" + AnimName + "的" + animType.ToString());
       // Debug.Log("下一个" + NextAnim.AnimName);
        if (NextAnim != null) {

            if (NextAnim.animType==AnimType.Path)
            {
             //   Debug.Log("下一个动画" + NextAnim.AnimName + "类型" + NextAnim.animType + "路径" + NextAnim.WayPointName);
                StartCoroutine(ClearTrail());
                Manager.Instance.ExcuteAnimation(NextAnim.AnimName, NextAnim.WayPointName, duration);
            }
            else
            {
                Manager.Instance.ExcuteAnimation(NextAnim);
            }
        }
      
    }
    public void Stop(AnimType animType)
    {
        if (animType == AnimType.Path)
        {
            
            pathTween.SetLoops(0);
            gameObject.GetComponent<RectTransform>().localPosition = originPos;
            pathTween.Kill();
            StartCoroutine(ClearTrail());
        }
    }
    IEnumerator ClearTrail()
    {
        TrailRenderer trail = GetComponent<TrailRenderer>();
        if (trail)
        {
            trail.Clear();
            yield return trail.time;
            //   gameObject.SetActive(false);
            trail.enabled = false;
        }
        yield return null;
    }

    
}
