using UnityEngine;
using System.Collections;

public class EnhanceScrollViewDragController : MonoBehaviour
{
    private Vector2 lastPosition = Vector2.zero;
    private Vector2 cachedPosition = Vector2.zero;
    private GameObject dragTarget;

    private Camera targetCamera;
    private int rayCastMask = 0;
    private bool dragStart = false;

    public void SetTargetCameraAndMask(Camera camera, int mask)
    {
        this.targetCamera = camera;
        this.rayCastMask = mask;
    }

    void Update()
    {
        if (this.targetCamera == null)
            return;
#if UNITY_EDITOR
        ProcessMouseInput();
#elif UNITY_IOS || UNITY_ANDROID
        ProcessTouchInput();
#endif
    }

    /// <summary>
    /// Process Mouse Input
    /// </summary>
    private void ProcessMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (targetCamera == null)
                return;
            dragTarget = RayCast(this.targetCamera, Input.mousePosition);
            lastPosition.x = Input.mousePosition.x;
            lastPosition.y = Input.mousePosition.y;
        }
        if (Input.GetMouseButton(0))
        {
            if (dragTarget == null)
                return;
            cachedPosition.x = Input.mousePosition.x;
            cachedPosition.y = Input.mousePosition.y;
            Vector2 delta = cachedPosition - lastPosition;
            if (!dragStart && delta.sqrMagnitude != 0f)
                dragStart = true;

            if (dragStart)
            {
                // Notify target
                dragTarget.SendMessage("OnEnhanceViewDrag", delta, SendMessageOptions.DontRequireReceiver);
            }
            lastPosition = cachedPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (dragTarget != null && dragStart)
            {
                dragTarget.SendMessage("OnEnhaneViewDragEnd", SendMessageOptions.DontRequireReceiver);
            }
            dragTarget = null;
            dragStart = false;
        }
    }

    /// <summary>
    /// Process Touch input
    /// </summary>
    private void ProcessTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (targetCamera == null)
                    return;
                dragTarget = RayCast(this.targetCamera, Input.mousePosition);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (dragTarget == null)
                    return;
                if (!dragStart && touch.deltaPosition.sqrMagnitude != 0f)
                {
                    dragStart = true;
                }
                if (dragStart)
                {
                    // Notify target
                    dragTarget.SendMessage("OnEnhanceViewDrag", touch.deltaPosition, SendMessageOptions.DontRequireReceiver);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (dragTarget != null && dragStart)
                {
                    dragTarget.SendMessage("OnEnhaneViewDragEnd", SendMessageOptions.DontRequireReceiver);
                }
                dragTarget = null;
                dragStart = false;
            }
        }
    }

    public GameObject RayCast(Camera cam, Vector3 inPos)
    {
        Vector3 pos = cam.ScreenToViewportPoint(inPos);
        if (float.IsNaN(pos.x) || float.IsNaN(pos.y))
            return null;
        if (pos.x < 0f || pos.x > 1f || pos.y < 0f || pos.y > 1f) return null;

        Ray ray = cam.ScreenPointToRay(inPos);
        float dis = 100f;
        RaycastHit[] hits = Physics.RaycastAll(ray, dis, rayCastMask);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                GameObject go = hits[i].collider.gameObject;
                DragEnhanceView dragView = go.GetComponent<DragEnhanceView>();
                if (dragView == null)
                    continue;
                else
                {
                    // just return current hover object our drag target
                    return go;
                }
            }
        }
        return null;
    }
}
