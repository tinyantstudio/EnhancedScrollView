using UnityEngine;
using System.Collections;

public class EnhanceItem : MonoBehaviour
{
    // get the right offset factor
    private int curveOffSetIndex = 0;
    public int CurveOffSetIndex
    {
        get { return this.curveOffSetIndex; }
        set { this.curveOffSetIndex = value; }
    }
    private int curRealIndex = 0;
    public int RealIndex
    {
        get { return this.curRealIndex; }
        set { this.curRealIndex = value; }
    }

    public bool inRightArea = false;
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
        EnhancelScrollView.GetInstance.SetHorizontalTargetItemIndex(this);
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
