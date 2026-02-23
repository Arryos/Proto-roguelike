using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SO_Float so_Angle;

    [SerializeField]
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TargetDirection();
    }

    void TargetDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // ininite horizontal plane at player height
        Plane plane = new Plane(Vector3.up, player.transform.position);

        // Check hit point and get direction
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 dir = hitPoint - player.transform.position;
            dir.y = 0;

            // Convert to Euler angle and apply to SO
            if (dir.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

                angle -= 90;
                if (angle < 0)
                {
                    angle += 360;
                }

                if(angle != so_Angle.Get())
                {
                    so_Angle.Set(angle);
                }
            }
        }
    }
}
