using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SO_Float so_Angle;
    [SerializeField]
    private SO_bool so_ControlDevice; //false = keyboard/mouse ; true = controller

    private GameObject player;
    [SerializeField]
    private PlayerInput playerInput;

    private string lastcontrolScheme;


    private void Awake()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInput>();
        lastcontrolScheme = playerInput.currentControlScheme;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        var actionMap = playerInput.actions.FindActionMap("Player");
        if(actionMap == null)
        {
            Debug.LogError("Cannot find ActionMap Player");
            return;
        }

        foreach (var action in actionMap.actions)
        {
            action.performed += OnActionTriggered;
        }
    }

    private void OnDisable()
    {
        var actionMap = playerInput.actions.FindActionMap("Player");
        if (actionMap == null)
        {
            Debug.LogError("Cannot find ActionMap Player");
            return;
        }

        foreach (var action in actionMap.actions)
        {
            action.performed -= OnActionTriggered;
        }
    }


    // Update is called once per frame
    void Update()
    {
        TargetDirection();
    }

    void TargetDirection()
    {

        if(false)
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

                    if (angle != so_Angle.Get())
                    {
                        so_Angle.Set(angle);
                    }
                }
            }
        }
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        string currentControlScheme = playerInput.currentControlScheme;

        if(currentControlScheme != lastcontrolScheme)
        {
            lastcontrolScheme = currentControlScheme;
            Debug.Log($"Device changed : {lastcontrolScheme}");
            if(lastcontrolScheme == "Gamepad")
            {
                so_ControlDevice.Set(true);
            }
            else
            {
                so_ControlDevice.Set(false);
            }
        }
    }

}
