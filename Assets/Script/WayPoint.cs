using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
[SerializeField]
public class WayPoint:MonoBehaviour{
    public string animName;
    Anim headAnim;
    public Vector3[] targetPoint;
    void Awake()
    {
        animName = name;
        Transform[] allTransform = transform.GetComponentsInChildren<Transform>();
        targetPoint = new Vector3[allTransform.Length - 1];
        for (int iLoop = 1; iLoop < allTransform.Length; iLoop++)
        {
            targetPoint[iLoop - 1] = allTransform[iLoop].localPosition;
        }
    }
}
