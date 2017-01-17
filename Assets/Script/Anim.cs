using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
[ExecuteInEditMode]
public class Anim : MonoBehaviour {
    public string AnimName;
    public AnimType animType;
    public float duration=1;
    Image image;
    Vector3 originPos;
    Color originColor;
    Color targetColor;
    [HideInInspector]
    public Vector3[] path;
    public string WayPointName;
    Anim NextAnim;
  //  private AnimType playAnimType = AnimType.Glow;//默认为发光动画
    // Use this for initialization
    void Awake () {
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
        Color none = new Color(0, 0, 0, 0);
        switch (animType)
        {
            case AnimType.Glow:
                if (image)
                {
                    image.color = none;
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
    public void Play(Color targetColor, float duration)
    {
        animType = AnimType.ChangeColor;
        ChangeColor(targetColor, duration);
    }
    public void ChangeColor(Color targetColor,float duration)
    {
        if (image)
        {
            image.DOColor(targetColor, duration).OnComplete(OnFinshed);
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
        transform.DOLocalPath(path, duration, PathType.Linear).OnComplete(OnFinshed);
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
    }
    public void AddNext(string Name, AnimType animType, float duration)
    {
        foreach (KeyValuePair<string, Anim> item in Manager.Instance.AnimationDic)
        {
            if (item.Key == Name)
            {
                NextAnim = item.Value;
                NextAnim.name = Name;
                NextAnim.animType = animType;
                NextAnim.duration = duration;
                AddNext(item.Value);
            }
        }
    }
    public void AddNext(string Name,string wayPointName, float duration)
    {
        foreach (KeyValuePair<string, Anim> item in Manager.Instance.AnimationDic)
        {
            if (item.Key == Name)
            {
                NextAnim = item.Value;
                NextAnim.name = Name;
                NextAnim.animType = AnimType.Path;
                NextAnim.WayPointName = wayPointName;
                NextAnim.duration = duration;
                AddNext(item.Value);
            }
        }
    }
    void OnStarted()
    {
        if (animType == AnimType.Path)
        {
            TrailRenderer trail = GetComponent<TrailRenderer>();
            trail.Clear();
            trail.enabled = true;
            
        }
    }
    public void OnFinshed()
    {
        Debug.Log("完成动画：" + AnimName + "的" + animType.ToString());
        if (NextAnim != null) {
             
            if (NextAnim.animType == AnimType.Path)
            {
                StartCoroutine(ClearTrail());
                Manager.Instance.ExcuteAnimation(AnimName, WayPointName);
            }
            else
            {
                Manager.Instance.ExcuteAnimation(NextAnim);
            }
        }
    }
    IEnumerator ClearTrail()
    {
        TrailRenderer trail = GetComponent<TrailRenderer>();
        if (trail)
        {
            yield return trail.time;
            trail.Clear();
        }
        yield return null;
    }
}
