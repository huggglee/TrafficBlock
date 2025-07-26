using UnityEngine;

public class DragControllerMobile : MonoBehaviour
{
    private Camera cam;
    private Transform selectedBlock;
    private Vector3 offset;
    private Plane dragPlane;
    private float fixedZ;  

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Ray ray = cam.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.parent != null && hit.transform.parent.CompareTag("Block"))
                        {
                            selectedBlock = hit.transform.parent;
                        }
                        else if (hit.transform.CompareTag("Block"))
                        {
                            selectedBlock = hit.transform;
                        }
                        else
                        {
                            selectedBlock = null;
                            break;
                        }

                        fixedZ = selectedBlock.position.z;
                        dragPlane = new Plane(Vector3.forward, new Vector3(0, 0, fixedZ));

                        float distance;
                        dragPlane.Raycast(ray, out distance);
                        offset = selectedBlock.position - ray.GetPoint(distance);
                    }
                    break;

                case TouchPhase.Moved:
                    if (selectedBlock != null)
                    {
                        Ray moveRay = cam.ScreenPointToRay(touch.position);
                        float moveDistance;
                        if (dragPlane.Raycast(moveRay, out moveDistance))
                        {
                            Vector3 newPos = moveRay.GetPoint(moveDistance) + offset;
                            newPos.z = fixedZ;   // giữ nguyên Z
                            selectedBlock.position = newPos;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    selectedBlock = null;
                    break;
            }
        }
    }
}
