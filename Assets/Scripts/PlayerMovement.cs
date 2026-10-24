using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 1f;
    public float CrouchMultiplier = 0.5f;
    public bool Freezed = false;
    public bool Stunned = false;
    private Rigidbody2D rb;
    private Vector2 MoveVector;
    private float ActualSpeed;
    private float FreezeTimer = 0;
    private float StunTimer = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
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
        
        MoveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKey(KeyCode.LeftShift) || Freezed)
            ActualSpeed = Speed * CrouchMultiplier;
        else
            ActualSpeed = Speed;

    }
    void FixedUpdate()
    {
        rb.linearVelocity = MoveVector * ActualSpeed;
    }
}