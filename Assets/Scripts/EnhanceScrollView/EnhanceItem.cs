using UnityEngine;
using System.Collections;

public class EnhanceItem : MonoBehaviour
{
    // Start index
    private int curveOffSetIndex = 0;
    public int CurveOffSetIndex
    {
        get { return this.curveOffSetIndex; }
        set { this.curveOffSetIndex = value; }
    }

    // Runtime real index(Be calculated in runtime)
    private int curRealIndex = 0;
    public int RealIndex
    {
        get { return this.curRealIndex; }
        set { this.curRealIndex = value; }
    }

    // Curve center offset 
    private float dCurveCenterOffset = 0.0f;
    public float CenterOffSet
    {
        get { return this.dCurveCenterOffset; }
        set { dCurveCenterOffset = value; }
    }
    private Transform mTrs;

    void Awake()
    {
        mTrs = this.transform;
        OnAwake();
    }

    void Start()
    {
        // Use NGUI's ui click event system
        // You can UGUI or 3D collider click event system
        UIEventListener.Get(this.gameObject).onClick = OnClickScrollViewItem;
        OnStart();
    }

    // Select the item
    private void OnClickScrollViewItem(GameObject obj)
    {
        EnhanceScrollView.GetInstance.SetHorizontalTargetItemIndex(this);
        OnClickItem();
    }

    // Update Item's status
    // 1. position
    // 2. scale
    // 3. "depth" is 2D or z Position in 3D to set the front and back item
    public void UpdateScrollViewItems(float xValue, float depthValue, float yValue, float scaleValue)
    {
        Vector3 targetPos = Vector3.one;
        Vector3 targetScale = Vector3.one;
        // position
        targetPos.x = xValue;
        targetPos.y = yValue;
        mTrs.localPosition = targetPos;

        // Set the "depth" of item
        // targetPos.z = depthValue;
        SetItemDepth(depthValue);
        // scale
        targetScale.x = targetScale.y = scaleValue;
        mTrs.localScale = targetScale;
    }

    protected virtual void OnClickItem()
    {
    }

    protected virtual void OnStart()
    {
    }

    protected virtual void OnAwake()
    {
    }

    protected virtual void SetItemDepth(float depthValue)
    {
    }

    // Set the item center state
    public virtual void SetSelectState(bool isCenter)
    {
    }
}
