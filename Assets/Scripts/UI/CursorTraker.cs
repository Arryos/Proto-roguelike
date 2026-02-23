using UnityEngine;

public class CursorTraker : MonoBehaviour
{
    private Camera m_camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        arrowDirection();
    }

    void arrowDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // ininite horizontal plane at player height
        Plane plane = new Plane(Vector3.up, transform.position);

        // Check hit point and get direction
        if(plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 dir = hitPoint - transform.position;
            dir.y = 0;

            // Convert to Euler angle and apply to arrow
            if (dir.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

                angle -= 90;
                if(angle < 0)
                {
                    angle += 360;
                }

                transform.localRotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

    }
}
