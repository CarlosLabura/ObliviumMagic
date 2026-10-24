using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpell : MonoBehaviour
{
    public SpellType CurrentSpell = SpellType.Fist;
    
    [SerializeField] private GameObject Hitboxes; //uso esto para el pivote este en el centro del jugador
    [SerializeField] private GameObject FistHitbox;
    [SerializeField] private GameObject Projectile;
    private float Cooldown = 0;

    void Shoot(SpellType Type, float Speed, Vector3 Scale)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        GameObject projectile = Instantiate(Projectile, gameObject.GetComponent<Transform>().position, Quaternion.identity);
        projectile.GetComponent<SpellTypeScript>().Type = Type;
        projectile.transform.localScale = Scale;
        Vector2 projectileDirection = (mousePosition - transform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().rotation = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        projectile.GetComponent<Rigidbody2D>().AddForce(projectileDirection * Speed, ForceMode2D.Impulse);
    }
    void Update()
    {
        if (Cooldown > 0) {
            Cooldown -= Time.deltaTime;
        }

        // hitboxes fijas apuntan al mouse
        Hitboxes.transform.position = transform.position;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        
        Vector2 direction = mousePosition - transform.position;
        Hitboxes.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);

        // lo que pasa al clikear, se aca se activan las hitbox y se tiran los proyectiles
        if (Cooldown <= 0 && !GetComponent<PlayerMovement>().Stunned){
            FistHitbox.SetActive(false);
            if (Input.GetMouseButton(0))
            {   
                switch (CurrentSpell)
                {
                    case SpellType.Fist:
                        FistHitbox.SetActive(true);
                        Cooldown = 0.5f; 
                        break;
                    case SpellType.Rock:
                        Shoot(SpellType.Rock, 10f, new Vector3(0.9f,0.7f,0.7f));
                        Cooldown = 1f;
                        break;
                    case SpellType.Fire:
                        Shoot(SpellType.Fire, 15f, new Vector3(1f,0.5f,0.5f));
                        Cooldown = 1.5f;
                        break;
                    case SpellType.Ice:
                        Shoot(SpellType.Ice, 12f, new Vector3(0.7f,0.7f,0.7f));
                        Cooldown = 2f;
                        break;
                    case SpellType.Electricty:
                        Shoot(SpellType.Electricty, 20f, new Vector3(0.9f,0.4f,0.4f));
                        Cooldown = 2.5f;
                        break;
                    default:
                        break;
                }
            }
        }


        // cambio de hechizos
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.R))
        {
            CurrentSpell += 1;
            if (CurrentSpell == SpellType.None)
                CurrentSpell = SpellType.Fist;
        } 
        SpellChange(SpellType.Rock, KeyCode.Alpha1);
        SpellChange(SpellType.Fire, KeyCode.Alpha2);
        SpellChange(SpellType.Ice, KeyCode.Alpha3);
        SpellChange(SpellType.Electricty, KeyCode.Alpha4);
    }
    void SpellChange(SpellType Type, KeyCode Keybind)
    {
        if (Input.GetKeyDown(Keybind))
        {
            if (CurrentSpell == Type)
                CurrentSpell = SpellType.Fist;
            else
                CurrentSpell = Type;
        }
    }
}
