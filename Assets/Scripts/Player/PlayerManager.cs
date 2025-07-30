using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Movimentação")]
    public float moveSpeed = 5f;

    [Header("Mouse_Movimentação")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    [Header("Lanterna")]
    private Light luzLanterna;
    private bool lanternaAtivada = false;

    [Header("Áudio")]
    public AudioSource audioSourcePassos;
    public AudioClip[] passosClips;
    public float tempoEntrePassos = 0.5f;

    [Header("Som da Lanterna")]
    public AudioClip somLanterna;
    private AudioSource audioSourceLanterna;


    private float proximoSomPasso = 0f;

    private float bobFrequency = 10f;
    private float bobAmplitude = 0.05f;
    private float bobTimer = 0f;
    private Vector3 cameraInitialLocalPos;
    private Rigidbody meuRigidbody;

    float xRotation = 0f;
    public float corridavelocidade = 10f;

    private float groundCheckDistance = 1.1f;
    public LayerMask groundMask;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        cameraInitialLocalPos = cameraTransform.localPosition;
        meuRigidbody = GetComponent<Rigidbody>();
        meuRigidbody.freezeRotation = true;
        audioSourceLanterna = gameObject.AddComponent<AudioSource>();
        audioSourceLanterna.playOnAwake = false;
        audioSourceLanterna.spatialBlend = 0f;

    }

    void Update()
    {
        Mover();
        OlharComMouse();
        CameraBalanco();
        ChecarLanterna();
    }

    void Mover()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? corridavelocidade : moveSpeed;
        Vector3 move = transform.TransformDirection(direction) * currentSpeed * Time.deltaTime;

        transform.position += move;

        if (direction.magnitude > 0.1f && CharacterEstáNoChão())
        {
            if (Time.time >= proximoSomPasso)
            {
                TocarPasso();
                float modificador = Input.GetKey(KeyCode.LeftShift) ? 0.3f : 0.5f;
                proximoSomPasso = Time.time + modificador;
            }
        }
    }

    void TocarPasso()
    {
        if (passosClips.Length > 0 && audioSourcePassos != null)
        {
            AudioClip clip = passosClips[Random.Range(0, passosClips.Length)];
            audioSourcePassos.PlayOneShot(clip);
        }
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
            bobTimer += Time.deltaTime * bobSpeed;
            float bobOffset = Mathf.Sin(bobTimer) * bobSize;
            cameraTransform.localPosition = cameraInitialLocalPos + new Vector3(0f, bobOffset, 0f);
        }
        else
        {
            bobTimer = 0f;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraInitialLocalPos, Time.deltaTime * 5f);
        }
    }

    bool CharacterEstáNoChão()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
    }

    void ChecarLanterna()
    {
        if (luzLanterna != null && Input.GetKeyDown(KeyCode.F))
        {
            lanternaAtivada = !lanternaAtivada;
            luzLanterna.enabled = lanternaAtivada;

            if (somLanterna != null)
                audioSourceLanterna.PlayOneShot(somLanterna);
        }
    }



    public void RegistrarNovaLanterna(GameObject lanternaInstanciada)
    {
        luzLanterna = lanternaInstanciada.GetComponentInChildren<Light>();
        if (luzLanterna != null)
            luzLanterna.enabled = false;
    }

}
