using UnityEngine;

public enum NPCState {Patrol, Following, Attacking}

public class NPCRoutine : MonoBehaviour
{
    public NPCState State = NPCState.Following;
    [SerializeField] private Transform Player;
    private MovementSystem MovementSystem;
    private Rigidbody2D RigidBody2D;
    void Start()
    {
        MovementSystem = GetComponent<MovementSystem>();
        RigidBody2D = GetComponent<Rigidbody2D>(); 
    }

    public void Rotate(Vector3 position)
    {
        Vector2 direction = (position - transform.position).normalized;
        RigidBody2D.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -90f;
    }
    public void Move(Vector2 position, float speed, float stopDistance)
    {
        Vector2 diff = position - RigidBody2D.position;
        float dist = diff.magnitude;

        // Llegó: frená y salí
        if (dist <= stopDistance)
        {
            MovementSystem.MoveVector = Vector2.zero;
            return;
        }

        // Distancia que recorrería en un paso de física
        float step = speed * Time.fixedDeltaTime;

        // Si falta menos que un paso, vector más corto para no pasarse
        MovementSystem.MoveVector = dist < step ? diff / step : diff / dist;

        // Si MovementSystem tiene su propia velocidad, pasale esta
        MovementSystem.Speed = speed;
    }
    
    void Update()
    {
        if (State == NPCState.Following)
        {
            Rotate(Player.position);
            Move(Player.position, 3, 4);
        } 
        else if (State == NPCState.Attacking)
        {
            Rotate(Player.position);
        } 
        else if (State == NPCState.Patrol)
        {
            
        }
    }
}
