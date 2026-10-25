using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{
    //hechizos
    public SpellType CurrentSpell = SpellType.Fist;
    public float SpellCooldown = 1.38f;
    private float Cooldown = 0;
    //variables internas
    [SerializeField] private GameObject Hitboxes; //uso esto para el pivote este en el centro del jugador
    [SerializeField] private GameObject FistHitbox;
    [SerializeField] private GameObject Projectile;
    private MovementSystem MovementSystem;
    void Start()
    {
        MovementSystem = GetComponent<MovementSystem>();
    }
    public void CastSpell(SpellType Type, float Speed, Vector3 Scale)
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
        if (Cooldown > 0) 
            Cooldown -= Time.deltaTime;

        // hitboxes fijas apuntan al mouse
        Hitboxes.transform.position = transform.position;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        
        Vector2 direction = mousePosition - transform.position;
        Hitboxes.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);

        // lo que pasa al clikear, se aca se activan las hitbox y se tiran los proyectiles
        if (Cooldown <= 0 && !MovementSystem.Stunned){
            FistHitbox.SetActive(false);
            if (Input.GetMouseButton(0))
            {   
                switch (CurrentSpell)
                {
                    case SpellType.Fist:
                        FistHitbox.SetActive(true);
                        Cooldown = SpellCooldown/3; 
                        break;
                    case SpellType.Rock:
                        CastSpell(SpellType.Rock, 10f, new Vector3(0.9f,0.7f,0.7f));
                        Cooldown = SpellCooldown; //por decision popular todos los hechizos van a tener el mismo cooldown
                        break;
                    case SpellType.Fire:
                        CastSpell(SpellType.Fire, 15f, new Vector3(1f,0.5f,0.5f));
                        Cooldown = SpellCooldown; //por comodidad lo voy a dejar asi por si dsp lo quieren cambiar
                        break;
                    case SpellType.Ice:
                        CastSpell(SpellType.Ice, 12f, new Vector3(0.7f,0.7f,0.7f));
                        Cooldown = SpellCooldown;
                        break;
                    case SpellType.Electricty:
                        CastSpell(SpellType.Electricty, 20f, new Vector3(0.9f,0.4f,0.4f));
                        Cooldown = SpellCooldown;
                        break;
                    default:
                        break;
                }
            }
        }
        // cambio de hechizos
        //añadir desbloqueo de hechizos en vez de que solo esten todos desbloqueados
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
    private void SpellChange(SpellType Type, KeyCode Keybind)
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
