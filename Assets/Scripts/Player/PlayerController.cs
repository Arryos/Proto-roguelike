using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField]
    private GameObject body;

    //GameData SO 
    [SerializeField]
    private SO_Float so_Speed;

    private float m_speed = 5;
    [SerializeField]
    private float jumpHeight = 2;

    private float gravity = -9.8f;

    private bool isLookAtCursor = false;

    private Vector2 moveInput;
    private Vector3 velocity;
    private Vector2 lookInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = this.gameObject.GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        so_Speed.OnValueChanged += changeSpeed;
    }

    private void OnDisable()
    {
        so_Speed.OnValueChanged -= changeSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * m_speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);



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
        lookInput = context.ReadValue<Vector2>();
        if(lookInput != Vector2.zero)
        {
            LastDirection = lookInput;
            isLookAtCursor = true;
        }
        else
        {
            isLookAtCursor = false;
        }
        //Debug.Log($"Move input : {moveInput}");

        //body.transform.LookAt(new Vector3(lookInput.x, transform.position.y, lookInput.y));
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

    private void changeSpeed(float p_speed)
    {
        m_speed = p_speed;
    }
}
