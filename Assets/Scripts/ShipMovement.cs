using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    [Header("Hareket")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;

    [Header("Fare Dönüşü")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float verticalLookLimit = 80f;

    [Header("Kamera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Su Seviyesi")]
    [SerializeField] private float waterLevel = 0f;

    [Header("Çarpışma")]
    [SerializeField] private float collisionRadius = 3f;
    [SerializeField] private float collisionHeight = 4f;
    [SerializeField] private LayerMask collisionMask = ~0;

    private float verticalRotation = 0f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY
                       | RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 pos = transform.position;
        pos.y = waterLevel;
        rb.position = pos;
    }

    private void Update()
    {
        HandleRotation();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        LockWaterLevel();
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, mouseX, 0f));

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        bool running = Input.GetKey(KeyCode.LeftShift);
        float speed = running ? runSpeed : moveSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        move.y = 0f;

        if (move.sqrMagnitude > 0.01f)
            move = move.normalized;

        Vector3 targetPos = rb.position + move * speed * Time.fixedDeltaTime;
        targetPos.y = waterLevel;

        if (move.sqrMagnitude > 0.01f)
        {
            Vector3 capsuleBottom = rb.position + Vector3.up * collisionRadius;
            Vector3 capsuleTop = rb.position + Vector3.up * (collisionHeight - collisionRadius);
            float moveDistance = speed * Time.fixedDeltaTime;

            bool blocked = Physics.CapsuleCast(
                capsuleBottom,
                capsuleTop,
                collisionRadius,
                move,
                out RaycastHit hit,
                moveDistance + 0.05f,
                collisionMask
            );

            if (blocked)
            {
                Vector3 slideDir = Vector3.ProjectOnPlane(move, hit.normal);
                slideDir.y = 0f;

                bool slideBlocked = false;

                if (slideDir.sqrMagnitude > 0.01f)
                {
                    slideBlocked = Physics.CapsuleCast(
                        capsuleBottom,
                        capsuleTop,
                        collisionRadius,
                        slideDir.normalized,
                        moveDistance + 0.05f,
                        collisionMask
                    );
                }

                if (slideBlocked || slideDir.sqrMagnitude < 0.01f)
                    targetPos = rb.position; 
                else
                {
                    targetPos = rb.position + slideDir * speed * Time.fixedDeltaTime;
                    targetPos.y = waterLevel;
                }
            }
        }

        rb.MovePosition(targetPos);
    }

    private void LockWaterLevel()
    {
        if (!Mathf.Approximately(rb.position.y, waterLevel))
        {
            Vector3 pos = rb.position;
            pos.y = waterLevel;
            rb.position = pos;
        }
    }
}