using UnityEngine;

public enum NPCState { Patrol, Following, Attacking, Searching }

public class NPCRoutine : MonoBehaviour
{
    public NPCState State = NPCState.Following;
    public float Distance;              // distancia para empezar a atacar
    public float Sight;                 // distancia de visión, al irte de ahí te deja de seguir
    private Vector2 LastSeenPosition;    // última posición en la que te vio
    // variables internas
    [SerializeField] private Transform Player;
    private MovementSystem MovementSystem;
    private Rigidbody2D RigidBody2D;
    private SpellSystem SpellSystem;
    private const float ArriveDistance = 0.15f;
    void Start()
    {
        MovementSystem = GetComponent<MovementSystem>();
        SpellSystem = GetComponent<SpellSystem>();
        RigidBody2D = GetComponent<Rigidbody2D>();
    }
    public void Rotate(Vector3 position)
    {
        Vector2 direction = (position - transform.position).normalized;
        RigidBody2D.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    }
    private bool objectBetween(Vector2 target, string Tag = "Wall")
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(RigidBody2D.position, target);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag(Tag))
                return true;
        }
        return false;
    }
    private bool CanSee(Vector2 target)
    {
        return (target - RigidBody2D.position).magnitude < Sight && !objectBetween(target);
    }
    private void StartSearching()
    {
        MovementSystem.MoveVector = Vector2.zero;
        State = NPCState.Searching;
    }
    public void RawMove(Vector2 position) // moverse sin cambiar el estado
    {
        Vector2 diff = position - RigidBody2D.position;
        float dist = diff.magnitude;

        if (objectBetween(position)) // si hay una pared en medio dejar de moverse
        {
            MovementSystem.MoveVector = Vector2.zero;
            return;
        }
        float step = MovementSystem.Speed * Time.fixedDeltaTime;

        MovementSystem.MoveVector = dist < step ? diff / step : diff / dist;
        Rotate(position);
    }
    public void Move(Vector2 position)
    {
        Vector2 diff = position - RigidBody2D.position;
        float dist = diff.magnitude;

        if (!CanSee(position)) // fuera de rango o con pared en medio
        {
            StartSearching();
            return;
        }
        if (dist <= Distance)
        {
            MovementSystem.MoveVector = Vector2.zero;
            State = NPCState.Attacking; // al llegar al rango de cercanía, atacar
            return;
        }
        float step = MovementSystem.Speed * Time.fixedDeltaTime;

        MovementSystem.MoveVector = dist < step ? diff / step : diff / dist;
        Rotate(position);
    }
    public void RawAttack(Vector2 target) // ignora paredes y dispara sin cambiar el estado
    {
        SpellSystem.CastSpell(SpellSystem.CurrentSpell, target);
        Rotate(target);
    }
    public void Attack(Vector2 target)
    {
        if (objectBetween(target))
        {
            StartSearching(); // pared en medio: ir a la última posición vista
            return;
        }
        if ((target - RigidBody2D.position).magnitude > Distance)
        {
            State = NPCState.Following; // al salir del rango continuar siguiendo
            return;
        }
        SpellSystem.CastSpell(SpellSystem.CurrentSpell, target);
        Rotate(target);
    }

    public void Search(Vector2 lastSeen)
    {
        if (CanSee(Player.position))
        {
            State = NPCState.Patrol;
            return;
        }

        Vector2 diff = lastSeen - RigidBody2D.position;
        float dist = diff.magnitude;

        if (dist <= ArriveDistance)
        {
            MovementSystem.MoveVector = Vector2.zero;
            State = NPCState.Patrol;
            return;
        }

        float step = MovementSystem.Speed * Time.fixedDeltaTime;
        MovementSystem.MoveVector = dist < step ? diff / step : diff / dist;
        Rotate(lastSeen);
    }
    private void Patrol(Vector2 position) // privada porque es más un idle que otra cosa
    {
        if (CanSee(position))
        {
            if (objectBetween(position, "Window"))
                State = NPCState.Attacking;
            else
                State = NPCState.Following;

            return;
        }
    }
    void Update()
    {
        Vector2 playerPos = Player.position;

        if (CanSee(playerPos))
            LastSeenPosition = playerPos;

             if (State == NPCState.Patrol)    Patrol(playerPos);
        else if (State == NPCState.Following) Move(playerPos);
        else if (State == NPCState.Attacking) Attack(playerPos);
        else if (State == NPCState.Searching) Search(LastSeenPosition);
    }
}