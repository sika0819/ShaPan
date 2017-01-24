using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Brush : MonoBehaviour {
    public GameObject BrushTail;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (UIController.Instance.NowMenu == UIController.Instance.BuildYourselfMenu && UIController.Instance.NowMenu.transform.localPosition == Vector3.zero)
        {
            BrushTail.SetActive(true);
        }
        else {
            TrailRenderer trail= BrushTail.GetComponent<TrailRenderer>();
            trail.Clear();
            BrushTail.SetActive(false);
        }
        if (BrushTail.activeSelf)
        {
            if (Input.GetMouseButton(0))
            {
                Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
                //BrushTail.transform.localPosition;
                RectTransform rect = BrushTail.transform as RectTransform;
                rect.anchoredPosition = pos;
            }
            else if(Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                   // Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                    Vector2 pos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.GetTouch(0).position, canvas.worldCamera, out pos);
                    //BrushTail.transform.localPosition;
                    RectTransform rect = BrushTail.transform as RectTransform;
                    rect.anchoredPosition = pos;
                }

            }
        }
	}
    public static Vector2 WordToScenePoint(Vector3 wordPosition)
    {
        CanvasScaler canvasScaler = GameObject.Find("Canvas").gameObject.GetComponent<CanvasScaler>();

        float resolutionX = canvasScaler.referenceResolution.x;

        float resolutionY = canvasScaler.referenceResolution.y;

        float offect = (Screen.width / canvasScaler.referenceResolution.x) * (1 - canvasScaler.matchWidthOrHeight) + (Screen.height / canvasScaler.referenceResolution.y) * canvasScaler.matchWidthOrHeight;

        Vector2 a = RectTransformUtility.WorldToScreenPoint(Camera.main, wordPosition);

        return new Vector2(a.x / offect, a.y / offect); ;
    }
}
