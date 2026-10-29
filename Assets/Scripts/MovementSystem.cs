using UnityEngine;
using System.Collections;

public class MovementSystem : MonoBehaviour
{
    public bool Player = false;
    [Header("Movement Settings")]
    [Min(0)] public float Speed = 1f;
    private float CrouchMultiplier = 0.5f;
    [HideInInspector] public Vector2 MoveVector;
    private float ActualSpeed;
    // estados
    /* 
        los estados no se pueden modificar solo con la variable, si o si se tienen que aplicar
        con sus funciones respectivas y tener un tiempo de duracion. por eso estan ocultos en el inspector
        mantengo las variables publicas para que se puedan acceder desde otros scripts
    */
    [HideInInspector] public bool IsFreezed = false;
    [HideInInspector] public bool IsStunned = false;
    // camara
    [ShowIf("Player")] [Header("Player Settings")]
    [Min(0)] public float CameraSensivility = 1f;
    private float CameraSpeed = 5f;
    //variables internas
    private Rigidbody2D RigidBody2D;
    private SpellSystem SpellSystem;
    void Start()
    {
        SpellSystem = GetComponent<SpellSystem>();
        RigidBody2D = GetComponent<Rigidbody2D>();
    }
    public void Freeze(float Time)
    {
        StartCoroutine(_Freeze(Time));
    }
    private IEnumerator _Freeze(float Time)
    {
        IsFreezed = true;
        yield return new WaitForSeconds(Time);
        IsFreezed = false;
    }
    public void Stun(float Time)
    {
        StartCoroutine(_Stun(Time));
    }
    private IEnumerator _Stun(float Time)
    {
        IsStunned = true;
        yield return new WaitForSeconds(Time);
        IsStunned = false;
    }
    public void Knockback(Vector3 position, float Force)
    {
        Stun(0.1f);
        Vector2 attackDirection = (position - transform.position).normalized;
        MoveVector = -attackDirection * Force;
    }
    void Update()
    {
        if (Player){ 
            Vector3 mousePosition = SpellSystem.GetCursorPosition();
            
            Vector3 target = new Vector3(
                transform.position.x + ((transform.position.x + mousePosition.x) / 2) / CameraSensivility,
                transform.position.y + ((transform.position.y + mousePosition.y) / 2) / CameraSensivility,
                Camera.main.transform.position.z
            );

            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, target, Time.deltaTime * CameraSpeed);
            
            Vector2 rotationDirection = (mousePosition - transform.position).normalized;
            RigidBody2D.rotation = Mathf.Atan2(rotationDirection.y, rotationDirection.x) * Mathf.Rad2Deg -90f;
        }

        // a partir de aca no se ejecuta nada si estas stuneado
        if (IsStunned)
            return;

        if (Player) {
            MoveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            if (Input.GetKey(KeyCode.LeftShift))
                ActualSpeed = Speed * CrouchMultiplier;
        }

        if (IsFreezed)
            ActualSpeed = Speed * (CrouchMultiplier * 0.5f);
        else
            ActualSpeed = Speed;
    }
    void FixedUpdate()
    {
        RigidBody2D.linearVelocity = MoveVector * ActualSpeed;
    }
}