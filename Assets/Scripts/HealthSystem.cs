using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int MaxHealth = 50;
    public int Health = 50;
    public SpellType Weakness;

    //public int MaxShield = 0;
    //public int Shield = 0;


    void OnTriggerEnter2D(Collider2D other) {
        if (gameObject.CompareTag("Enemy")) //que pasa si un enemigo recibe daño
        {
            if (other.gameObject.GetComponent<SpellTypeScript>().Type == Weakness) // si el tipo de hechizo recivdo por el trigger es igual a la debilidad del enemigo
            {
                // lo que pasa al un objeto recibir daño de una hitbox o un proyectil
                switch (other.gameObject.GetComponent<SpellTypeScript>().Type)
                {
                    case SpellType.Fist:
                        Health -= 10;
                        break;
                    case SpellType.Rock:
                        Health -= 10;
                        break;
                    case SpellType.Fire:
                        Health -= 8;
                        break;
                    case SpellType.Ice:
                        Health -= 10;
                        break;
                    case SpellType.Electricty:
                        Health -= 10;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // si la hitbox no corresponde con la debilidad pero es un golpe q saque uno de vida
                if (other.gameObject.GetComponent<SpellTypeScript>().Type == SpellType.Fist)
                {
                    Health -= 1;
                }
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
