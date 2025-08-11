using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraHolder;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;
    public GameObject Hands;

    private Rigidbody rb;
    private bool isGrounded;
    private float xRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(HandsFollow());
    }

    private void Update()
    {
        Look();
        ;   
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Move();
        CheckGround();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.forward * v + transform.right * h;
        rb.MovePosition(rb.position + moveDirection.normalized * speed * Time.fixedDeltaTime);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); 
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.5f, groundLayer);
    }

    IEnumerator HandsFollow()
    {
        while (true)
        {
            yield return null;

            float yRotate = Input.GetAxis("Vertical") * -5f;
            
            Quaternion targetRotation = Quaternion.Euler(xRotation, 0f, yRotate);

            Hands.transform.localRotation = Quaternion.Lerp(
                        Hands.transform.localRotation,
                        targetRotation,
                        Time.deltaTime * 5f
                        );
        }
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); 
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}