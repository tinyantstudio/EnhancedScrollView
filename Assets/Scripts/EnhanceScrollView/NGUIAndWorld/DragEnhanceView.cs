using UnityEngine;
using System.Collections;

public class DragEnhanceView : MonoBehaviour
{
    private EnhanceScrollView enhanceScrollView;
    public void SetScrollView(EnhanceScrollView view)
    {
        enhanceScrollView = view;
    }

    void OnEnhanceViewDrag(Vector2 delta)
    {
        if (enhanceScrollView != null)
            enhanceScrollView.OnDragEnhanceViewMove(delta);
    }

    void OnEnhaneViewDragEnd()
    {
        if (enhanceScrollView != null)
            enhanceScrollView.OnDragEnhanceViewEnd();
    }
}