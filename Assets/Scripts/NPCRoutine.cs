using UnityEngine;

public enum NPCState {Patrol, Following, Attacking}

public class NPCRoutine : MonoBehaviour
{
    public NPCState State = NPCState.Following;
    public float Distance; //distancia que toma para empezar a atacarte
    public float Sight; //distancia de vision, al irte de ahi te dejara de seguir
    [SerializeField] private Transform Player;
    private MovementSystem MovementSystem;
    private Rigidbody2D RigidBody2D;
    private SpellSystem SpellSystem;
    void Start()
    {
        MovementSystem = GetComponent<MovementSystem>();
        SpellSystem = GetComponent<SpellSystem>();
        RigidBody2D = GetComponent<Rigidbody2D>(); 
    }
    public void Rotate(Vector3 position)
    {
        Vector2 direction = (position - transform.position).normalized;
        RigidBody2D.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -90f;
    }
    public void Move(Vector2 position)
    {
        Vector2 diff = position - RigidBody2D.position;
        float dist = diff.magnitude;

        if (dist >= Sight)
        {
            MovementSystem.MoveVector = Vector2.zero;
            State = NPCState.Patrol;
            return;
        }
        if (dist <= Distance)
        {
            MovementSystem.MoveVector = Vector2.zero;
            State = NPCState.Attacking; //al llegar al rango de cercania, atacar
            return;
        }
        float step = MovementSystem.Speed * Time.fixedDeltaTime;

        MovementSystem.MoveVector = dist < step ? diff / step : diff / dist;
        Rotate(position);
    }
    public void Attack(Vector2 target)
    {
        Vector2 diff = target - RigidBody2D.position;
        float dist = diff.magnitude;

        if (dist > Distance)
        {
            State = NPCState.Following; //al salir del rango continuar siguiendo
            return;
        }
        SpellSystem.CastSpell(SpellSystem.CurrentSpell, target);
        Rotate(target);
    }
    private void Patrol(Vector2 position) // privada porque es mas como un idle que otra cosa
    {
        Vector2 diff = position - RigidBody2D.position;
        float dist = diff.magnitude;

        if (dist < Sight)
        {
            State = NPCState.Following;
            return;
        }
    }
    void Update()
    {
        if (State == NPCState.Following)
        {
            Move(Player.position);
        } 
        else if (State == NPCState.Attacking)
        {
            Attack(Player.position);
        } 
        else if (State == NPCState.Patrol)
        {
            Patrol(Player.position);
        }
    }
}
