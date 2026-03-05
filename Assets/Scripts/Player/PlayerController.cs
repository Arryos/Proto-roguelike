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
    private bool isMove = false;
    private bool LastDirInput = false; // false = move; true = look

    private Vector2 moveInput;
    private Vector3 velocity;
    private Vector2 lookInput;

    // Direction Vectors for look/target and move
    [SerializeField]
    private Vector2 LookDirection = Vector2.zero;
    [SerializeField]
    private Vector2 lastMoveDir = Vector2.zero;

    [SerializeField]
    private GameObject targetArrow;

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
                isLookAtCursor = true;
                targetArrow.SetActive(true);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // ininite horizontal plane at player height
                Plane plane = new Plane(Vector3.up, transform.position);

                // Check hit point and get direction
                if (plane.Raycast(ray, out float distance))
                {
                    Vector3 hitPoint = ray.GetPoint(distance);
                    Vector3 dir = hitPoint - transform.position;

                    Vector2 direction = new Vector2(dir.x, dir.z);

                    LookDirection = direction.normalized;

                    LastDirInput = true;
                }
            }
            else
            {
                isLookAtCursor = false;
                targetArrow.SetActive(false);
            }
        }

        if(!isLookAtCursor && isMove) // if just move use LastMoveDir
        {
            BodyDirection(lastMoveDir);
        }
        else if(!isLookAtCursor && !isMove)  // if target / Look action use LookDirection
        {
            if(LastDirInput)
            {
                BodyDirection(LookDirection);
            }
            else
            {
                BodyDirection(lastMoveDir);
            }
        }
        else
        {
            BodyDirection(LookDirection);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        isMove = true;
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move input : {moveInput}");

        if(moveInput != Vector2.zero)
        {
            lastMoveDir = moveInput;
            LastDirInput = false;
        }
        else
        {
            isMove = false;
        }
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        if(so_controlDevice.Get()) //false = keyboard/mouse ; true = controller
        {
            lookInput = context.ReadValue<Vector2>();
            if (lookInput != Vector2.zero)
            {
                LookDirection = lookInput;
                isLookAtCursor = true;
                LastDirInput = true;
                targetArrow.SetActive(true);
            }
            else
            {
                isLookAtCursor = false;
                targetArrow.SetActive(false);
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
            
            fireCnt = 0;
        }
        else
        {
            Debug.Log("Active  Fire");
        }
    }

    private void BodyDirection()
    {
        //Get angle from LookDirection
        float targetAngle = Mathf.Atan2(LookDirection.x, LookDirection.y) * Mathf.Rad2Deg;

        body.transform.localRotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    private void BodyDirection(Vector2 dir)
    {
        float targetAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        body.transform.localRotation = Quaternion.Euler(0f, targetAngle, 0f);

        so_Angle.Set(targetAngle);
    }

    private void changeSpeed(float p_speed)
    {
        m_speed = p_speed;
    }

#region mouse/keyboard specifics

    //test
    private void OnTarget(InputAction.CallbackContext context)
    {
        //Debug.LogWarning("target mouse");

        //lastMoveDir = LookDirection;
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
