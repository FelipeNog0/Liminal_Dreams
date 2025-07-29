using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Movimentação")]
    public float moveSpeed = 5f;

    [Header("Mouse_Movimentação")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    private float bobFrequency = 10f;
    private float bobAmplitude = 0.05f;
    private float bobTimer = 0f;
    private Vector3 cameraInitialLocalPos;
    private Rigidbody rigidbody;

    float xRotation = 0f;
    public float corridavelocidade = 10f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        cameraInitialLocalPos = cameraTransform.localPosition;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
    }

    void Update()
    {
        Mover();
        OlharComMouse();
        CameraBalanco();
    }

    void Mover()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? corridavelocidade : moveSpeed;
        Vector3 move = transform.TransformDirection(direction) * moveSpeed * Time.deltaTime;

        transform.position += move;
    }

    void OlharComMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        
        xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(-xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void CameraBalanco()
    {
        float bobSpeed = Input.GetKey(KeyCode.LeftShift) ? bobFrequency * 1.5f : bobFrequency;
        float bobSize = Input.GetKey(KeyCode.LeftShift) ? bobAmplitude * 1.5f : bobAmplitude;
        float speed = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude;
        if (speed > 0.1f && CharacterEstáNoChão())
        {
            bobTimer += Time.deltaTime * bobFrequency;
            float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;
            cameraTransform.localPosition = cameraInitialLocalPos + new Vector3(0f, bobOffset, 0f);
        }
        else
        {
            bobTimer = 0f;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraInitialLocalPos, Time.deltaTime * 5f);
        }
    }

    private float groundCheckDistance = 1.1f;
    public LayerMask groundMask;

    bool CharacterEstáNoChão()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
    }
}
