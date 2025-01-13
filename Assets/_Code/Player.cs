using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    public Transform WinCondiotionsTransform;
    public float jumpForce = 7.0f;
    public float speed = 7.0f;

    [SerializeField] Transform cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

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
