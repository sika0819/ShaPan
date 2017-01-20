using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using DG.Tweening;
public class UIController : MonoBehaviour {
    Vector3 originPos;
    Vector3 endPos;
    public GameObject Title;
    public Image Fill;
    public Image WuShuiMenu;
    public Image ChuLiFill;
    public Image YuanLi;
    public Image YuanLiFill;
    public Image LiuCheng;
    public Image LiuChengFill;
    public Image BuildYouSelf;
    public Image BuildSelfFill;
    public delegate void OnSlideUp();
    public event OnSlideUp OnSlidedUp;
	// Use this for initialization
	void Start () {
        Title.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        Fill.rectTransform.sizeDelta = new Vector2(20,10);
        WuShuiMenu.rectTransform.sizeDelta = new Vector2(5,0);
        YuanLi.rectTransform.sizeDelta = new Vector2(5, 0);
        LiuCheng.rectTransform.sizeDelta = new Vector2(5, 0);
        BuildYouSelf.rectTransform.sizeDelta = new Vector2(5, 0);
        ChuLiFill.rectTransform.sizeDelta = new Vector2(0, 0);
        YuanLiFill.rectTransform.sizeDelta = new Vector2(0, 0);
        LiuChengFill.rectTransform.sizeDelta = new Vector2(0, 0);
        BuildSelfFill.rectTransform.sizeDelta = new Vector2(0, 0);
        OnSlidedUp += OnSlideUpFunc;
	}
	
	// Update is called once per frame
	void Update () {
        TouchBegin();
        TouchEnd();
        if (endPos.y - originPos.y > 0.5)
        {
            //Debug.Log("向上");
            originPos = endPos;
            OnSlidedUp();
        }
	}
    bool TouchBegin() {
        if (Input.GetMouseButtonDown(0))
        {
            originPos = Input.mousePosition;
            return true;
        }
        else if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                originPos = Input.GetTouch(0).position;
                return true;
            }
           
        }
        return false;
    }
    bool TouchEnd() {
        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            return true;
        }
        else if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endPos = Input.GetTouch(0).position;
                return true;
            }
        }
        return false;
    }
    public void OnSlideUpFunc() {
        Debug.Log("向上");
        Title.transform.DOLocalMove(new Vector3(0,150,0),1);
        Fill.rectTransform.DOSizeDelta(new Vector2(1075, 10), 1).OnComplete(ShowLine);
    }
    void ShowLine() {
        WuShuiMenu.rectTransform.DOSizeDelta(new Vector2(5, 245), 1).OnComplete(ShowChuLiFill);
        YuanLi.rectTransform.DOSizeDelta(new Vector2(5, 245), 1).OnComplete(ShowYuanLiFill);
        LiuCheng.rectTransform.DOSizeDelta(new Vector2(5, 245), 1).OnComplete(ShowLiuChengFill);
        BuildYouSelf.rectTransform.DOSizeDelta(new Vector2(5, 245), 1).OnComplete(ShowBuildYouSelfFill);
    }
    void ShowChuLiFill()
    {
        ChuLiFill.rectTransform.DOSizeDelta(new Vector2(120, 120), 1);
    }
    void ShowYuanLiFill()
    {
        YuanLiFill.rectTransform.DOSizeDelta(new Vector2(120, 120), 1);
    }
    void ShowLiuChengFill()
    {
        LiuChengFill.rectTransform.DOSizeDelta(new Vector2(120, 120), 1);
    }
    void ShowBuildYouSelfFill()
    {
        BuildSelfFill.rectTransform.DOSizeDelta(new Vector2(120, 120), 1);
    }
    
}
