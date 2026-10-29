using TMPro;
using UnityEngine;
using System.Collections;


public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    [Min(0)] public int MaxHealth = 50;
    [Min(-1)] public int Health = 0;
    public SpellType Weakness;
    [Header("Fire Settings")]
    public int FireDamage = 1;
    [Min(0)] public float FireTimer = 1f;
    private int FireTicks = 0;
    [HideInInspector] public bool IsOnFire = false;
    // invencibilidad
    // consultar que es un estado en MovementSystem.cs
    [HideInInspector] public bool IsInvencible = false;
    // variables internas
    private MovementSystem MovementSystem;
    private SpellSystem SpellSystem;
    [ShowIf(typeof(MovementSystem), "Player")] [Header("Player Settings")] [SerializeField] private HUDBlood HUDBlood;
    void Start()
    {
        MovementSystem = GetComponent<MovementSystem>();
        SpellSystem = GetComponent<SpellSystem>();
        Health = MaxHealth;
    }
    public void Invencible(float Time)
    {
        StartCoroutine(_Invencible(Time));
    }
    private IEnumerator _Invencible(float Time)
    {
        IsInvencible = true;
        yield return new WaitForSeconds(Time);
        IsInvencible = false;
    }
    public void Damage(int Amount)
    {
        if (IsInvencible)
            return;

        Health -= Amount;
        if (MovementSystem.Player) {
            HUDBlood.SetHealth((float)Health / MaxHealth);
            Invencible(1);
            MovementSystem.Knockback(SpellSystem.GetCursorPosition(), 2);
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

        StartCoroutine(_Fire(Ticks));
    }
    private IEnumerator _Fire(int Ticks)
    {
        IsOnFire = true;
        FireTicks = Ticks;
        while (FireTicks > 0)
        {
            yield return new WaitForSeconds(FireTimer);
            FireTicks -= 1;
            Damage(FireDamage);
        }
        IsOnFire = false;
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
                        Damage(8);
                        Fire(3);
                        break;
                    case SpellType.Ice:
                        Damage(10);
                        MovementSystem.Freeze(3f);
                        break;
                    case SpellType.Ray:
                        Damage(10);
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
