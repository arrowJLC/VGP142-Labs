using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Windows;

public class Player : MonoBehaviour/*, ThirdPersonInputs.IOverworldActions*/
{
    CharacterController cc;
    //ThirdPersonInputs inputs;

    Vector2 direction;
    Vector3 velocity;
    bool isJumpPressed = false;

    // Start is called before the first frame update
    Rigidbody rb;
    public Transform WinCondiotionsTransform;
    public float jumpForce = 7.0f;
    private float jumpTime = 1f;
    public float speed = 7.0f;


    float timeToApex; //max jump time / 2
    float initialJumpVelocity;
    float gravity;

    [SerializeField] Transform cam;

    //void Awake()
    //{
    //    inputs = new ThirdPersonInputs();
    //}

    //private void OnEnable()
    //{
    //    inputs.Enable();
    //    inputs.Overworld.SetCallbacks(this);
    //}

    //private void OnDisable()
    //{
    //    inputs.Disable();
    //    inputs.Overworld.RemoveCallbacks(this);
    //}

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        cc = GetComponent<CharacterController>();

        ////fomulas taken from the following video: https://www.youtube.com/watch?v=hG9SzQxaCm8
        timeToApex = jumpTime / 2;
        gravity = (-2 * jumpForce) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = -(gravity * timeToApex);
    }

    //private void FixedUpdate()
    //{
    //    velocity = new Vector3(direction.x * speed, velocity.y, direction.y * speed);

    //    if (!cc.isGrounded) velocity.y += gravity * Time.fixedDeltaTime;
    //    else velocity.y = CheckJump();

    //    cc.Move(velocity * Time.fixedDeltaTime);
    //}

    //private float CheckJump()
    //{
    //    if (isJumpPressed) return initialJumpVelocity;
    //    return -cc.minMoveDistance;
    //}

    //public void OnJump(InputAction.CallbackContext context) => isJumpPressed = context.ReadValueAsButton();
    //public void OnMove(InputAction.CallbackContext ctx)
    //{
    //    if (ctx.performed) direction = ctx.ReadValue<Vector2>();
    //    if (ctx.canceled) direction = Vector2.zero;
    //}

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        Vector3 camFoward = cam.forward;
        Vector3 camRight = cam.right;

        camFoward.y = 10;
        camRight.y = 0;

        Vector3 fowardRelative = zInput * camFoward;
        Vector3 rightRelative = xInput * camRight;

        Vector3 playDirec = fowardRelative + rightRelative;   


        rb.velocity = new Vector3(playDirec.x * speed, rb.velocity.y, playDirec.z * speed);


        if (Input.GetButtonDown("Jump") && rb.velocity.y <= 0)
        {
            if (rb.velocity.y < 1)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
