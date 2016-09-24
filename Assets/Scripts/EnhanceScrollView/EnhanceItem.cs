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
    public int curRealIndex = 0;
    public int RealIndex
    {
        get { return this.curRealIndex; }
        set { this.curRealIndex = value; }
    }

    // Curve center offset 
    public float dCurveCenterOffset = 0.0f;
    public float CenterOffSet
    {
        get { return this.dCurveCenterOffset; }
        set { dCurveCenterOffset = value; }
    }
    private Transform mTrs;
    private UITexture mTexture;

    void Awake()
    {
        mTrs = this.transform;
        mTexture = this.GetComponent<UITexture>();
    }

    void Start()
    {
        UIEventListener.Get(this.gameObject).onClick = OnClickScrollViewItem;
    }

    // Select the item
    private void OnClickScrollViewItem(GameObject obj)
    {
        EnhanceScrollView.GetInstance.SetHorizontalTargetItemIndex(this);
    }

    // Update Item's status
    // 1. position
    // 2. scale
    // 3. "depth" is 2D or z Position in 3D to set the front and back item
    public void UpdateScrollViewItems(float xValue, float depthValue, float yValue, float scaleValue)
    {
        Vector3 targetPos = Vector3.one;
        Vector3 targetScale = Vector3.one;
        targetPos.x = xValue;
        targetPos.y = yValue;

        // Set the "depth" of item
        // targetPos.z = depthValue;
        if (mTexture.depth != (int)Mathf.Abs(depthValue))
            mTexture.depth = (int)Mathf.Abs(depthValue);
        targetScale.x = targetScale.y = scaleValue;

        mTrs.localPosition = targetPos;
        mTrs.localScale = targetScale;
    }

    // Set the item center state
    public void SetSelectState(bool isCenter)
    {
        if (mTexture == null)
            mTexture = this.GetComponent<UITexture>();
        if (isCenter)
            mTexture.color = Color.white;
        else
            mTexture.color = Color.gray;
    }
}
