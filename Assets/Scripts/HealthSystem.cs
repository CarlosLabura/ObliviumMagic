using UnityEngine;
[RequireComponent(typeof(MovementSystem))]
[RequireComponent(typeof(SpellSystem))]
public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    [Min(0)] public int MaxHealth = 50;
    [Min(-1)] public int Health = 0;
    public SpellType Weakness;
    [Header("Fire Settings")]
    [Min(0)] public int FireDamage = 1;
    [Min(0)] public float FireGap = 1f;
    private float FireTimer = 0;
    private int FireTicks = 0;
    [HideInInspector] public bool IsOnFire = false;
    // invencibilidad
    // consultar que es un estado en MovementSystem.cs
    [HideInInspector] public bool IsInvencible = false;
    private float InvencibleTimer = 0f;
    // variables internas
    private MovementSystem MovementSystem;
    private SpellSystem SpellSystem;
    [ShowIf(typeof(MovementSystem), "Player")] [Header("Player Settings")] [SerializeField] private HUDBlood HUDBlood;
    void Awake()
    {
        MovementSystem = GetComponent<MovementSystem>();
        SpellSystem = GetComponent<SpellSystem>();
    }
    void Start()
    {
        Health = MaxHealth;
    }
    public void Invencible(float Time)
    {
        IsInvencible = true;
        InvencibleTimer = Time;
    }
    public void Damage(int Amount, bool Knockback = true)
    {
        if (IsInvencible)
            return;

        Health -= Amount;
        if (MovementSystem.Player) {
            HUDBlood.SetHealth((float)Health / MaxHealth);
            if (Knockback) {
                Invencible(1);
                MovementSystem.Knockback(SpellSystem.GetCursorPosition(), 2);
            }
        }
        if (Health <= 0)
        {
            Destroy(gameObject);
            return;
        }
    }
    /*
        el fuego no es un estado, simplemente se aplica una cantidad de daño que se va aplicando
        lentamente con ticks de 1 segundo, hay una cantidad de ticks restantes que al pasar un segundo
        se sacan del contador y se restan de la vida
    */
   public void Fire(int Ticks)
    {
        if (IsInvencible)
            return;

        IsOnFire = true;
        FireTicks += Ticks;
        FireTimer = FireGap;
    }
    void Update()
    {
        if (InvencibleTimer > 0){
            InvencibleTimer -= Time.deltaTime;
        } else {
            if (IsInvencible)
                IsInvencible = false;
        }

        if (FireTimer > 0) {
            FireTimer -= Time.deltaTime;
        } else {
            if (FireTicks > 0)
            {
                FireTicks -= 1;
                FireTimer = FireGap;
                Damage(FireDamage, false);
            } else {
                IsOnFire = false;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        // si el enemigo recibe un trigger de la hitbox del player or el player recibe hitbox de un enemigo
        if (gameObject.CompareTag("Enemy") && other.gameObject.CompareTag("PlayerHitbox") 
        || gameObject.CompareTag("Player") && other.gameObject.CompareTag("EnemyHitbox"))
        {
            // si el tipo de hechizo recivdo por el trigger es igual a la debilidad del enemigo (o ninguno)
            if (other.gameObject.GetComponent<SpellTypeScript>().Type == Weakness || Weakness == SpellType.None)
            {
                // lo que pasa al un objeto recibir daño de una hitbox o un proyectil
                switch (other.gameObject.GetComponent<SpellTypeScript>().Type)
                {
                    case SpellType.Fist:
                        Damage(10);
                        break;
                    case SpellType.Rock:
                        Damage(10);
                        break;
                    case SpellType.Fire:
                        Fire(3);
                        Damage(8);
                        break;
                    case SpellType.Ice:
                        Damage(10);
                        MovementSystem.Freeze(3f);
                        break;
                    case SpellType.Ray:
                        Damage(10, false);
                        MovementSystem.Stun(1f);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // si la hitbox no corresponde con la debilidad pero es un golpe q saque uno de vida
                if (other.gameObject.GetComponent<SpellTypeScript>().Type == SpellType.Fist)
                    Damage(1);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // al jugador colisionar directamente con un enemigo
        if (gameObject.CompareTag("Player") && collision.gameObject.CompareTag("Enemy"))
            Damage(10);
    }
}
