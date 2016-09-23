using UnityEngine;
using System.Collections;

public class EnhanceScrollViewDragController : MonoBehaviour
{
    private Vector2 lastPosition = Vector2.zero;
    private Vector2 cachedPosition = Vector2.zero;
    private GameObject dragTarget;

    private Camera targetCamera;
    private int rayCastMask = 0;

    public void SetTargetCameraAndMask(Camera camera, int mask)
    {
        this.targetCamera = camera;
        this.rayCastMask = mask;

        Debug.Log("## set target camera and mask ##");
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
            // Notify target
            dragTarget.SendMessage("OnEnhanceViewDrag", delta, SendMessageOptions.DontRequireReceiver);
            lastPosition = cachedPosition;
        }
    }

    private void ProcessTouchInput()
    {

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
