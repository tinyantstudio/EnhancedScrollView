# EnhancedScrollView
Cool "3d" scoll view for Unity3D 4.x and 5.x version
**NGUI and UGUI support**

Using Unity3d's AnimationCurve to finish this EnhancedScrollView.AnimationCurve is very powerful and useful tools in developing game,the player's jumping, camera's path and so on.

## Features
1. Click left right button to recenter item
2. Click target to recenter and select item
4. Drag feature: drag enhanceScrollview to recenter item
4. Update item status with Curve horizontal time changing
5. Edit control curves for scale, position, and "depth", you can add your own curve to control item other properties
6. NGUI 2d, world 3d, and UGUI support


## Curves In Project
1. Position Curve. Control item's position.
2. Scale Curve. Control item's scale
3. "Depth" Curve. For determine item's back and front relationship, It's can be UIWidget's depth in NGUI(2D) or the Item's Z position value in 3D world

## In developing task
1. ~~drag feature~~ (Done)
2. animation curve editor(you can type the value for the KeyFrame)
3. ~~UGUI example~~(Done)
4. ......

## How to use 
Easy to make your own Enhance Item

1. open NGUIEnhanceScrollView.unity for NGUI example
2. open UGUIEnhanceScrollView.unity for UGUI example

Easy way to make better Curve: Copy EnhancedScrollView Component in example and paste it to your own TargetScrollView

<pre><code>
/// 
/// NGUI Enhance item example
/// 
public class MyNGUIEnhanceItem : EnhanceItem
{
    private UITexture mTexture;
    ......
    // Set the item "depth" 2d or 3d
    protected override void SetItemDepth(float depthCurveValue, int depthFactor, float itemCount)
    {
        if (mTexture.depth != (int)Mathf.Abs(depthCurveValue * depthFactor))
            mTexture.depth = (int)Mathf.Abs(depthCurveValue * depthFactor);
    }

    // Item is centered
    public override void SetSelectState(bool isCenter)
    {
        if (mTexture == null)
            mTexture = this.GetComponent<UITexture>();
        if (mTexture != null)
            mTexture.color = isCenter ? Color.white : Color.gray;
    }
    ......
}


///
/// UGUI Enhance item example
///
public class MyUGUIEnhanceItem : EnhanceItem
{
    private Button uButton;
    private RawImage rawImage;
    ......
    private void OnClickUGUIButton()
    {
        OnClickEnhanceItem();
    }

    // Set the item "depth" 2d or 3d
    protected override void SetItemDepth(float depthCurveValue, int depthFactor, float itemCount)
    {
        curDepth = (int)(depthCurveValue * itemCount);
        this.transform.SetSiblingIndex(curDepth);
    }

    public override void SetSelectState(bool isCenter)
    {
        if (rawImage == null)
            rawImage = GetComponent<RawImage>();
        rawImage.color = isCenter ? Color.white : Color.gray;
    }
    ......
}

</code></pre>

## ScreenShot
![ScreenShot](https://github.com/tinyantstudio/EnhancedScrollView/blob/master/screenshot.png)


**If you have some cool ideas or bugs just share with us or open issues**
