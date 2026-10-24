using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 1f;
    public float CrouchMultiplier = 0.5f;
    private Rigidbody2D rb;
    private Vector2 MoveVector;
    private float ActualSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }
    void Update()
    {
        MoveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
            ActualSpeed = Speed * CrouchMultiplier;
        else
            ActualSpeed = Speed;
    }
    void FixedUpdate()
    {
        rb.linearVelocity = MoveVector * ActualSpeed;
    }
}