using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    public bool Player = false;
    // velocidad
    public float Speed = 1f;
    private float CrouchMultiplier = 0.5f;
    [HideInInspector] public Vector2 MoveVector;
    private float ActualSpeed;
    // estados
    /* 
        los estados no se pueden modificar solo con la variable, si o si se tienen que aplicar
        con sus funciones respectivas y tener un tiempo de duracion. por eso estan ocultos en el inspector
        mantengo las variables publicas para que se puedan acceder desde otros scripts
    */
    [HideInInspector] public bool Freezed = false;
    [HideInInspector] public bool Stunned = false;
    private float FreezeTimer = 0;
    private float StunTimer = 0;
    // camara
    [ShowIf("Player")] public float CameraSensivility = 1f;
    private float CameraSpeed = 5f;
    //variables internas
    private Rigidbody2D RigidBody2D;
    void Start()
    {
        RigidBody2D = GetComponent<Rigidbody2D>(); 
    }
    public void Freeze(float Time)
    {
        Freezed = true;
        FreezeTimer = Time;
    }
    public void Stun(float Time)
    {
        Stunned = true;
        StunTimer = Time;
    }
    void Update()
    {
        if (Player){ 
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Vector3 target = new Vector3(
                transform.position.x + (transform.position.x + mousePosition.x) / CameraSensivility,
                transform.position.y + (transform.position.y + mousePosition.y) / CameraSensivility,
                Camera.main.transform.position.z
            );

            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, target, Time.deltaTime * CameraSpeed);
            
            Vector2 rotationDirection = (mousePosition - transform.position).normalized;
            RigidBody2D.rotation = Mathf.Atan2(rotationDirection.y, rotationDirection.x) * Mathf.Rad2Deg -90f;
        }

        if (FreezeTimer > 0) {
            FreezeTimer -= Time.deltaTime;
        } else {
            FreezeTimer = 0;
            Freezed = false;
        }

        if (StunTimer > 0) {
            StunTimer -= Time.deltaTime;
        } else {
            StunTimer = 0;
            Stunned = false;
        }

        if (Stunned)
            return;

        if (Player)
            MoveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKey(KeyCode.LeftShift) || Freezed)
            ActualSpeed = Speed * CrouchMultiplier;
        else
            ActualSpeed = Speed;

    }
    void FixedUpdate()
    {
        RigidBody2D.linearVelocity = MoveVector * ActualSpeed;
    }
}