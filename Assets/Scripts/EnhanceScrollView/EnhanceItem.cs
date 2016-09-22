using UnityEngine;
using System.Collections;

public class EnhanceItem : MonoBehaviour
{

    // 在ScrollViewitem中的索引
    // 定位当前的位置和缩放
    public int scrollViewItemIndex = 0;
    public bool inRightArea = false;

    private Vector3 targetPos = Vector3.one;
    private Vector3 targetScale = Vector3.one;

    private Transform mTrs;
    private UITexture mTexture;
    private int curRealIndex = 0;
    public int RealIndex
    {
        get { return this.curRealIndex; }
        set { this.curRealIndex = value; }
    }

    void Awake()
    {
        mTrs = this.transform;
        mTexture = this.GetComponent<UITexture>();
    }

    void Start()
    {
        UIEventListener.Get(this.gameObject).onClick = OnClickScrollViewItem;
    }

    // 当点击Item，将该item移动到中间位置
    private void OnClickScrollViewItem(GameObject obj)
    {
        EnhancelScrollView.GetInstance().SetHorizontalTargetItemIndex(scrollViewItemIndex);
    }

    /// <summary>
    /// 更新该Item的缩放和位移
    /// </summary>
    public void UpdateScrollViewItems(float xValue, float zPosValue, float yValue, float scaleValue)
    {
        targetPos.x = xValue;
        targetPos.y = yValue;
        // targetPos.z = zPosValue;
        mTexture.depth = (int)Mathf.Abs(zPosValue);
        targetScale.x = targetScale.y = scaleValue;

        mTrs.localPosition = targetPos;
        mTrs.localScale = targetScale;
    }

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
