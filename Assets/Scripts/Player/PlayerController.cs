using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput playerInput;

    [SerializeField]
    private GameObject body;

    //GameData SO 
    [SerializeField]
    private SO_Float so_Speed;
    [SerializeField]
    private SO_bool so_controlDevice; //false = keyboard/mouse ; true = controller
    [SerializeField]
    private SO_Float so_Angle; // Angle to face mouse raycast position on a plane

    private float m_speed = 5;
    [SerializeField]
    private float jumpHeight = 2;

    private float gravity = -9.8f;

    private bool isLookAtCursor = false;

    private Vector2 moveInput;
    private Vector3 velocity;
    private Vector2 lookInput;

    private InputActionMap actionMap;

    private void Awake()
    {
        controller = this.gameObject.GetComponent<CharacterController>();
        playerInput = this.gameObject.GetComponent<PlayerInput>();
        actionMap = playerInput.actions.FindActionMap("Player");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        so_Speed.OnValueChanged += changeSpeed;

        // register to actions
        actionMap.FindAction("Target").started += OnTarget;
    }

    private void OnDisable()
    {
        so_Speed.OnValueChanged -= changeSpeed;

        // unregister to actions
        actionMap.FindAction("Target").started -= OnTarget;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * m_speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        if(!so_controlDevice.Get())
        {
            if(actionMap.FindAction("Target").IsPressed())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // ininite horizontal plane at player height
                Plane plane = new Plane(Vector3.up, transform.position);

                // Check hit point and get direction
                if (plane.Raycast(ray, out float distance))
                {
                    Vector3 hitPoint = ray.GetPoint(distance);
                    Vector3 dir = hitPoint - transform.position;

                    Vector2 direction = new Vector2(dir.x, dir.z);

                    LastDirection = direction.normalized;

                }
            }
            else
            {
                // si pas pressed regarder si lastDirection == lastmoveDir
                if(lastMoveDir != LastDirection)
                {

                }
            }
        }


        BodyDirection();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move input : {moveInput}");

        if(!isLookAtCursor && moveInput != Vector2.zero)
        {
            LastDirection = moveInput;
        }
    }

    private Vector2 LastDirection;

    public void OnLook(InputAction.CallbackContext context)
    {
        if(so_controlDevice.Get()) //false = keyboard/mouse ; true = controller
        {
            lookInput = context.ReadValue<Vector2>();
            if (lookInput != Vector2.zero)
            {
                LastDirection = lookInput;
                isLookAtCursor = true;
            }
            else
            {
                isLookAtCursor = false;
            }
        }
    }

    private int fireCnt = 0;

    public void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire");

        fireCnt++;
        if(fireCnt%3 == 0)
        {
            Debug.Log("End Fire");
            //isLookAtCursor = false;
            fireCnt = 0;
        }
        else
        {
            Debug.Log("Active  Fire");
            //isLookAtCursor = true;
        }
    }

    private void BodyDirection()
    {
        //Get angle from lastDirection
        float targetAngle = Mathf.Atan2(LastDirection.x, LastDirection.y) * Mathf.Rad2Deg;

        body.transform.localRotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    private void BodyDirection(float angle)
    {
        body.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void changeSpeed(float p_speed)
    {
        m_speed = p_speed;
    }

    #region mouse/keyboard specifics

    private Vector2 lastMoveDir;
    //test
    private void OnTarget(InputAction.CallbackContext context)
    {
        Debug.LogWarning("target mouse");

        lastMoveDir = LastDirection;
    }

    private void MouseTarget()
    {
        if (actionMap.FindAction("Target").IsPressed())
        {
            isLookAtCursor = true;
            // use angle to set direction
        }
        else
        {
            isLookAtCursor = false;
        }
    }

#endregion

}
