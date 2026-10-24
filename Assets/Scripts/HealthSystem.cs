using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int MaxHealth = 50;
    public int Health = 50;
    public SpellType Weakness;

    //public int MaxShield = 0;
    //public int Shield = 0;


    void OnTriggerEnter2D(Collider2D other) {
        if (gameObject.CompareTag("Enemy"))
        {
            Debug.Log(other.gameObject.GetComponent<SpellTypeScript>().Type == Weakness);
            if (other.gameObject.GetComponent<SpellTypeScript>().Type == Weakness) // si el tipo de hechizo recivdo por el trigger es igual a la debilidad del enemigo
            {
                switch (other.gameObject.GetComponent<SpellTypeScript>().Type)
                {
                    case SpellType.Fist:
                        Health -= 1;
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
