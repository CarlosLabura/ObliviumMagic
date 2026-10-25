using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int MaxHealth = 50;
    public int Health = 50;
    public SpellType Weakness;
    private int FireDamage = 1;
    private float FireTimer = 0;
    private int FireTicks = 0;
    private PlayerMovement PlayerMovement;
    void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
    }
    public void Damage(int Amount)
    {
        Health -= Amount;
        if (Health <= 0)
        {
            // game over
        }
    }
    public void Fire(int Ticks)
    {
        FireTicks += Ticks;
        FireTimer = 1f;
    }
    void Update()
    {
        if (FireTimer > 0) {
            FireTimer -= Time.deltaTime;
        } else {
            if (FireTicks > 0)
            {
                FireTicks -= 1;
                FireTimer = 1f;
                Damage(FireDamage);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        // si el enemigo recibe un trigger de la hitbox del player or el player recibe hitbox de un enemigo
        Debug.Log(other.gameObject.CompareTag("PlayerHitbox"));
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
                        PlayerMovement.Freeze(3f);
                        break;
                    case SpellType.Electricty:
                        Damage(10);
                        PlayerMovement.Stun(1f);
                        //generar otro rayo o algo i dunno
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // si la hitbox no corresponde con la debilidad pero es un golpe q saque uno de vida
                if (other.gameObject.GetComponent<SpellTypeScript>().Type == SpellType.Fist)
                    Health -= 1;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.CompareTag("Player") && collision.gameObject.CompareTag("Enemy"))
            Damage(10);
    }
}
