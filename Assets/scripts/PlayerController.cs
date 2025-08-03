using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public float rotationSpeed = 10f;
    public Transform camera;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;

    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Move();
        CheckGround();


        // Получаем текущий угол поворота игрока
        float currentYAngle = transform.eulerAngles.y;

        // Получаем горизонтальный угол камеры (только ось Y)
        float targetYAngle = camera.eulerAngles.y;

        // Плавно поворачиваем игрока к нужному углу
        float newYAngle = Mathf.LerpAngle(currentYAngle, targetYAngle, rotationSpeed * Time.fixedDeltaTime);

        transform.rotation = Quaternion.Euler(0f, newYAngle, 0f);
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDirection = camera.forward * v + camera.right * h;
        moveDirection.y = 0f;

        rb.MovePosition(rb.position + moveDirection.normalized * speed * Time.deltaTime);
    }
    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.5f, groundLayer);
    }
}
