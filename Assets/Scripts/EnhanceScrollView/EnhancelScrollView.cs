using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnhancelScrollView : MonoBehaviour
{
    // Control the item's scale curve
    public AnimationCurve scaleCurve;
    // Control the position curve
    public AnimationCurve positionCurve;
    // Control the "depth"'s curve(In 3d version just the Z value, in 2D UI you can use the depth(NGUI))
    // NOTE:
    // 1. In NGUI set the widget's depth may cause performance problem
    // 2. If you use 3D UI just set the Item's Z position
    public AnimationCurve depthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
    public float posCurveFactor = 500.0f;
    // vertical fixed position value 
    public float yFixedPositionValue = 46.0f;

    // targets in scroll view
    public List<EnhanceItem> scrollViewItems;

    // center and preCentered item
    private EnhanceItem curCenterItem;
    private EnhanceItem preCenterItem;

    // if we can change the target item
    private bool canChangeItem = true;
    private float dFactor = 0.2f;

    // each item's horizontal value offset
    private float[] dHorizontalValues;

    // originHorizontalValue Lerp to horizontalTargetValue
    private float cachedHorizontalValue = 0.0f;
    private float originHorizontalValue = 0.1f;
    public float horizontalTargetValue = 0.5f;

    // Lerp duration
    public float lerpDuration = 0.2f;
    private float mCurrentDuration = 0.0f;
    private int mCenterIndex = 0;

    private static EnhancelScrollView instance;
    public static EnhancelScrollView GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        dFactor = (Mathf.RoundToInt((1f / scrollViewItems.Count) * 10000f)) * 0.0001f;
        Debug.Log("## calculate factor : " + dFactor);

        if (dHorizontalValues == null)
            dHorizontalValues = new float[scrollViewItems.Count];
        mCenterIndex = scrollViewItems.Count / 2;
        if (scrollViewItems.Count % 2 == 0)
            mCenterIndex = scrollViewItems.Count / 2 - 1;

        for (int i = 0; i < scrollViewItems.Count; i++)
        {
            scrollViewItems[i].scrollViewItemIndex = i;
            dHorizontalValues[i] = dFactor * (mCenterIndex - i);
            dHorizontalValues[mCenterIndex] = 0.0f;
            scrollViewItems[i].SetSelectColor(false);
            Debug.Log("## value " + dFactor * (mCenterIndex - i));
        }

        curCenterItem = scrollViewItems[mCenterIndex];
        canChangeItem = true;
        cachedHorizontalValue = Mathf.Lerp(originHorizontalValue, horizontalTargetValue, 1.0f);
        originHorizontalValue = horizontalTargetValue;
        UpdateEnhanceScrollView(cachedHorizontalValue);
        List<EnhanceItem> tmpList = new List<EnhanceItem>(this.scrollViewItems);
        SortViewItem(tmpList);
    }

    public void UpdateEnhanceScrollView(float fValue)
    {
        for (int i = 0; i < scrollViewItems.Count; i++)
        {
            EnhanceItem itemScript = scrollViewItems[i];
            float xValue = GetXPosValue(fValue, dHorizontalValues[itemScript.scrollViewItemIndex]);
            float scaleValue = GetScaleValue(fValue, dHorizontalValues[itemScript.scrollViewItemIndex]);
            float zValue = depthCurve.Evaluate(fValue + dHorizontalValues[itemScript.scrollViewItemIndex]);
            itemScript.UpdateScrollViewItems(xValue, -zValue * 50, yFixedPositionValue, scaleValue);
        }
    }

    void Update()
    {
        mCurrentDuration += Time.deltaTime;
        if (mCurrentDuration > lerpDuration)
        {
            mCurrentDuration = lerpDuration;
            if (curCenterItem != null)
                curCenterItem.SetSelectColor(true);
            if (preCenterItem != null)
                preCenterItem.SetSelectColor(false);
            canChangeItem = true;
        }

        float percent = mCurrentDuration / lerpDuration;
        cachedHorizontalValue = Mathf.Lerp(originHorizontalValue, horizontalTargetValue, percent);
        UpdateEnhanceScrollView(cachedHorizontalValue);
    }

    // Get the evaluate value to set item's scale
    private float GetScaleValue(float sliderValue, float added)
    {
        float scaleValue = scaleCurve.Evaluate(sliderValue + added);
        return scaleValue;
    }


    // Get the X value set the Item's position
    private float GetXPosValue(float sliderValue, float added)
    {
        float evaluateValue = positionCurve.Evaluate(sliderValue + added) * posCurveFactor;
        return evaluateValue;
    }

    // sort item with X so we can know how much distance we need to move the timeLine(curve time line)
    static public int SortPosition(EnhanceItem a, EnhanceItem b) { return a.transform.localPosition.x.CompareTo(b.transform.localPosition.x); }
    private int GetMoveCurveFactorCount(EnhanceItem item)
    {
        List<EnhanceItem> tmpList = new List<EnhanceItem>(this.scrollViewItems);
        SortViewItem(tmpList);
        int factorCount = Mathf.Abs(item.RealIndex) - Mathf.Abs(mCenterIndex);
        return Mathf.Abs(factorCount);

        // Testing code
        // Debug.Log("## Move factor count is " + factorCount + "  " + mCenterIndex + "  realIndex " + item.RealIndex);
        //float targetXPos = item.transform.localPosition.x;
        //for (int i = 0; i < scrollViewItems.Count; i++)
        //{
        //    float factor = (0.5f - dFactor * (mCenterIndex - i));
        //    float tempPosX = positionCurve.Evaluate(factor) * posCurveFactor;
        //    // Debug.Log(string.Format("factor:{0}, tempPosx:{1}, dis:{2}.", factor, tempPosX, Mathf.Abs(targetXPos - tempPosX)));
        //}
    }

    private void SortViewItem(List<EnhanceItem> items)
    {
        items.Sort(SortPosition);
        for (int i = items.Count - 1; i >= 0; i--)
            items[i].RealIndex = i;
    }

    public void SetHorizontalTargetItemIndex(int itemIndex)
    {
        if (!canChangeItem)
            return;

        EnhanceItem item = scrollViewItems[itemIndex];
        if (curCenterItem == item)
            return;

        canChangeItem = false;
        preCenterItem = curCenterItem;
        curCenterItem = item;

        // calculate the direction of moving
        float centerXValue = positionCurve.Evaluate(0.5f) * posCurveFactor;
        bool isRight = false;
        if (item.transform.localPosition.x > centerXValue)
            isRight = true;

        // calculate the offset * dFactor
        int moveIndexCount = GetMoveCurveFactorCount(item);
        float dvalue = 0.0f;
        if (isRight)
            dvalue = -dFactor * moveIndexCount;
        else
            dvalue = dFactor * moveIndexCount;

        horizontalTargetValue += dvalue;
        mCurrentDuration = 0.0f;
        originHorizontalValue = cachedHorizontalValue;
    }

    // Click the right button to select the next item.
    public void OnBtnRightClick()
    {
        if (!canChangeItem)
            return;
        int targetIndex = curCenterItem.scrollViewItemIndex + 1;
        if (targetIndex > scrollViewItems.Count - 1)
            targetIndex = 0;
        SetHorizontalTargetItemIndex(targetIndex);
    }

    // Click the left button the select next next item.
    public void OnBtnLeftClick()
    {
        if (!canChangeItem)
            return;
        int targetIndex = curCenterItem.scrollViewItemIndex - 1;
        if (targetIndex < 0)
            targetIndex = scrollViewItems.Count - 1;
        SetHorizontalTargetItemIndex(targetIndex);
    }


    //
    // Test for the scroll view click left or right button behavior.
    // Changing horizontalTargetValue will time for looking the scroll view in running.
    // 

    //private bool isInTesting = false;
    //public int clickCount = 0;
    //IEnumerator _StartTest()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    Debug.Log("## Begin scroll view test ##");
    //    while (true)
    //    {
    //        canChangeItem = true;
    //        yield return new WaitForSeconds(0.2f);
    //        clickCount++;
    //        this.OnBtnLeftClick();
    //        Debug.Log("### hello world ###" + clickCount);
    //    }
    //}
}
