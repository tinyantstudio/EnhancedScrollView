using UnityEngine;
using System.Collections;

/// <summary>
/// NGUI Enhance item example
/// </summary>
public class MyNGUIEnhanceItem : EnhanceItem
{
    private UITexture mTexture;

    protected override void OnAwake()
    {
        this.mTexture = GetComponent<UITexture>();
    }

    // Set the item "depth" 2d or 3d
    protected override void SetItemDepth(float depthValue)
    {
        if (mTexture.depth != (int)Mathf.Abs(depthValue))
            mTexture.depth = (int)Mathf.Abs(depthValue);
    }

    // Item is centered
    public override void SetSelectState(bool isCenter)
    {
        if (mTexture == null)
            mTexture = this.GetComponent<UITexture>();
        if (isCenter)
            mTexture.color = Color.white;
        else
            mTexture.color = Color.gray;
    }

    protected override void OnClickItem()
    {
        // item was clicked
    }
}
