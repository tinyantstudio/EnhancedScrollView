using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// [ExecuteInEditMode]
public class EnhancelScrollView : MonoBehaviour
{
    // 缩放曲线
    public AnimationCurve scaleCurve;
    // 位移曲线
    public AnimationCurve positionCurve;
    // 位移系数
    public float posCurveFactor = 500.0f;
    // y轴坐标固定值(所有的item的y坐标一致)
    public float yPositionValue = 46.0f;

    // 添加到EnhanceScrollView的目标对象
    public List<EnhanceItem> scrollViewItems;
    // 目标对象Widget脚本，用于depth排序
    private List<UITexture> textureTargets;

    // 当前处于中间的item
    private EnhanceItem centerItem;
    private EnhanceItem preCenterItem;

    // 当前出移动中，不能进行点击切换
    private bool canChangeItem = true;

    // 计算差值系数
    public float dFactor = 0.2f;
    
    // 点击目标移动的横向目标值
    private float[] moveHorizontalValues;
    // 对象之间的差值数组(根据差值系数算出)
    private float[] dHorizontalValues;

    // 横向变量值
    public float horizontalValue = 0.0f;
    // 目标值
    public float horizontalTargetValue = 0.1f;

    // 移动动画参数
    private float originHorizontalValue = 0.1f;
    public float duration = 0.2f;
    private float currentDuration = 0.0f;

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
        if((scrollViewItems.Count % 2) == 0)    
        {
            Debug.LogError("item count is invaild,please set odd count! just support odd count.");
        }

        if(moveHorizontalValues == null)
            moveHorizontalValues = new float[scrollViewItems.Count];

        if(dHorizontalValues == null)
            dHorizontalValues = new float[scrollViewItems.Count];

        if (textureTargets == null)
            textureTargets = new List<UITexture>();

        int centerIndex = scrollViewItems.Count / 2;
        for (int i = 0; i < scrollViewItems.Count;i++ )
        {
            scrollViewItems[i].scrollViewItemIndex = i;
            UITexture tmpTexture = scrollViewItems[i].gameObject.GetComponent<UITexture>();
            textureTargets.Add(tmpTexture);

            dHorizontalValues[i] = dFactor * (centerIndex - i);

            dHorizontalValues[centerIndex] = 0.0f;
            moveHorizontalValues[i] = 0.5f - dHorizontalValues[i];
            scrollViewItems[i].SetSelectColor(false);
        }

        centerItem = scrollViewItems[centerIndex];
        canChangeItem = true;
    }

    public void UpdateEnhanceScrollView(float fValue)
    {
        for (int i = 0; i < scrollViewItems.Count; i++)
        {
            EnhanceItem itemScript = scrollViewItems[i];
            float xValue = GetXPosValue(fValue, dHorizontalValues[itemScript.scrollViewItemIndex]);
            float scaleValue = GetScaleValue(fValue, dHorizontalValues[itemScript.scrollViewItemIndex]);
            itemScript.UpdateScrollViewItems(xValue, yPositionValue, scaleValue);
        }
    }

    void Update()
    {
        currentDuration += Time.deltaTime;
        if (currentDuration > duration)
        {
            // 更新完毕设置选中item的对象即可
            currentDuration = duration;
            if(centerItem != null)
                centerItem.SetSelectColor(true);
            if(preCenterItem != null)
                preCenterItem.SetSelectColor(false);
            canChangeItem = true;
        }

        SortDepth();
        float percent = currentDuration / duration;
        horizontalValue = Mathf.Lerp(originHorizontalValue, horizontalTargetValue, percent);
        UpdateEnhanceScrollView(horizontalValue);
    }

    /// <summary>
    /// 缩放曲线模拟当前缩放值
    /// </summary>
    private float GetScaleValue(float sliderValue, float added)
    {
        float scaleValue = scaleCurve.Evaluate(sliderValue + added);
        return scaleValue;
    }

    /// <summary>
    /// 位置曲线模拟当前x轴位置
    /// </summary>
    private float GetXPosValue(float sliderValue, float added)
    {
        float evaluateValue = positionCurve.Evaluate(sliderValue + added) * posCurveFactor;
        return evaluateValue;
    }

    public void SortDepth()
    {
        textureTargets.Sort(new CompareDepthMethod());
        for (int i = 0; i < textureTargets.Count; i++)
            textureTargets[i].depth = i;
    }

    /// <summary>
    /// 用于层级对比接口
    /// </summary>
    public class CompareDepthMethod : IComparer<UITexture>
    {
        public int Compare(UITexture left, UITexture right)
        {
            if (left.transform.localScale.x > right.transform.localScale.x)
                return 1;
            else if (left.transform.localScale.x < right.transform.localScale.x)
                return -1;
            else
                return 0;
        }
    }

    /// <summary>
    /// 获得当前要移动到中心的Item需要移动的factor间隔数
    /// </summary>
    private int GetMoveCurveFactorCount(float targetXPos)
    {
        int centerIndex = scrollViewItems.Count / 2;
        for (int i = 0; i < scrollViewItems.Count;i++ )
        {
            float factor = (0.5f - dFactor * (centerIndex - i));

            float tempPosX = positionCurve.Evaluate(factor) * posCurveFactor;
            if (Mathf.Abs(targetXPos - tempPosX) < 0.01f)
                return Mathf.Abs(i - centerIndex);
        }
        return -1;
    }

    /// <summary>
    /// 设置横向轴参数，根据缩放曲线和位移曲线更新缩放和位置
    /// </summary>
    public void SetHorizontalTargetItemIndex(int itemIndex)
    {
        if (!canChangeItem)
            return;

        EnhanceItem item = scrollViewItems[itemIndex];
        if (centerItem == item)
            return;

        canChangeItem = false;
        preCenterItem = centerItem;
        centerItem = item;

        // 判断点击的是左侧还是右侧计算ScrollView中心需要移动的value
        float centerXValue = positionCurve.Evaluate(0.5f) * posCurveFactor;
        bool isRight = false;
        if (item.transform.localPosition.x > centerXValue)
            isRight = true;

        // 差值,计算横向值
        int moveIndexCount = GetMoveCurveFactorCount(item.transform.localPosition.x);
        if (moveIndexCount == -1)
        {
            Debug.LogWarning("*****Move Index count is invalid.");
            moveIndexCount = 1; 
        }

        float dvalue = 0.0f;
        if (isRight)
            dvalue = -dFactor * moveIndexCount;
        else
            dvalue = dFactor * moveIndexCount;

        // 更改target数值，平滑移动
        horizontalTargetValue += dvalue;
        currentDuration = 0.0f;
        originHorizontalValue = horizontalValue;
    }

    /// <summary>
    /// 向右选择角色按钮
    /// </summary>
    public void OnBtnRightClick()
    {
        if (!canChangeItem)
            return;
        int targetIndex = centerItem.scrollViewItemIndex + 1;
        if (targetIndex > scrollViewItems.Count - 1)
            targetIndex = 0;
        SetHorizontalTargetItemIndex(targetIndex);
    }

    /// <summary>
    /// 向左选择按钮
    /// </summary>
    public void OnBtnLeftClick()
    {
        if (!canChangeItem)
            return;
        int targetIndex = centerItem.scrollViewItemIndex - 1;
        if (targetIndex < 0)
            targetIndex = scrollViewItems.Count - 1;
        SetHorizontalTargetItemIndex(targetIndex);
    }
}
