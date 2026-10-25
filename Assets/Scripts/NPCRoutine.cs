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
    public void RawMove(Vector2 position) //moverse sin cambiar el estado
    {
        Vector2 diff = position - RigidBody2D.position;
        float dist = diff.magnitude;

        if (objectBetween(position)) //si hay una pared en medio dejar de moverse
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

        if (dist >= Sight || objectBetween(position)) //si se va de su cambio de vision o hay una pared en medio dejar de moverse
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
    public void RawAttack(Vector2 target) //ignora si hay una pared en medio y solo dispara sin cambiar el estado
    {
        SpellSystem.CastSpell(SpellSystem.CurrentSpell, target);
        Rotate(target);
    }
    public void Attack(Vector2 target)
    {
        if (objectBetween(target))
        {
            State = NPCState.Patrol; //si hay una pared en medio dejar de atacar
            return;
        }
        if ((target - RigidBody2D.position).magnitude > Distance)
        {
            State = NPCState.Following; //al salir del rango continuar siguiendo
            return;
        }
        SpellSystem.CastSpell(SpellSystem.CurrentSpell, target);
        Rotate(target);
    }
    private void Patrol(Vector2 position) // privada porque es mas como un idle que otra cosa
    {
        if ((position - RigidBody2D.position).magnitude < Sight && !objectBetween(position))
        {
            if (objectBetween(position, "Window")){
                State = NPCState.Attacking;
                return;
            } else {
                State = NPCState.Following;
                return;
            }
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
