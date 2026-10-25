using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SpellSystem : MonoBehaviour
{
    //hechizos
    public SpellType CurrentSpell = SpellType.Fist;
    public float SpellCooldown = 1.38f;
    private float Cooldown = 0;
    //variables internas
    [SerializeField] private Transform Hitboxes; //uso esto para el pivote este en el centro del jugador
    [SerializeField] private GameObject FistHitbox;
    [SerializeField] private GameObject Projectile;
    private MovementSystem MovementSystem;
    [SerializeField] private TextMeshProUGUI SpellUI; //temporal
    void Start()
    {
        MovementSystem = GetComponent<MovementSystem>();
    }
    private void Spell(SpellType Type, float Speed, Vector3 Scale, Vector3 Target) //mousePosition
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        GameObject projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
        projectile.GetComponent<SpellTypeScript>().Type = Type;
        projectile.transform.localScale = Scale;
        Vector2 projectileDirection = (Target - transform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().rotation = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        projectile.GetComponent<Rigidbody2D>().AddForce(projectileDirection * Speed, ForceMode2D.Impulse);
    }
    public void CastSpell(SpellType Type, Vector3 Target)
    {
        if (Cooldown > 0 || MovementSystem.Stunned)
            return;

        switch (Type)
        {
            case SpellType.Fist:
                if (MovementSystem.Player) {
                    FistHitbox.SetActive(true);
                    Cooldown = SpellCooldown/3; 
                }
                break;
            case SpellType.Rock:
                Spell(SpellType.Rock, 10f, new Vector3(0.9f,0.7f,0.7f), Target);
                Cooldown = SpellCooldown; //por decision popular todos los hechizos van a tener el mismo cooldown
                break;
            case SpellType.Fire:
                Spell(SpellType.Fire, 15f, new Vector3(1f,0.5f,0.5f), Target);
                Cooldown = SpellCooldown; //por comodidad lo voy a dejar asi por si dsp lo quieren cambiar
                break;
            case SpellType.Ice:
                Spell(SpellType.Ice, 12f, new Vector3(0.7f,0.7f,0.7f), Target);
                Cooldown = SpellCooldown;
                break;
            case SpellType.Ray:
                Spell(SpellType.Ray, 20f, new Vector3(0.9f,0.4f,0.4f), Target);
                Cooldown = SpellCooldown;
                break;
            default:
                break;
        }
    }
    void Update()
    {
        if (Cooldown > 0)
            Cooldown -= Time.deltaTime;

        // toda la logica de aca solo se aplica si el script lo tiene el jugador
        if (MovementSystem.Player){
            if (Cooldown <= 0)
                FistHitbox.SetActive(false);

            // hitboxes fijas apuntan al mouse
            //solo el player puede pegar puñetes
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Hitboxes.position = transform.position;
            
            Vector2 direction = mousePosition - transform.position;
            Hitboxes.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);
        
            if (Input.GetMouseButton(0))
            {
                CastSpell(CurrentSpell, mousePosition);
            }

            // cambio de hechizos
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CurrentSpell = SpellType.Fist;
                SpellUI.SetText(CurrentSpell.ToString());
            }
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.R))
            {
                CurrentSpell += 1;
                if (CurrentSpell == SpellType.None)
                    CurrentSpell = SpellType.Rock;

                SpellUI.SetText(CurrentSpell.ToString());
            } 
            SpellChange(SpellType.Rock, KeyCode.Alpha1);
            SpellChange(SpellType.Fire, KeyCode.Alpha2);
            SpellChange(SpellType.Ice, KeyCode.Alpha3);
            SpellChange(SpellType.Ray, KeyCode.Alpha4);
        }
    }
    private void SpellChange(SpellType Type, KeyCode Keybind)
    {
        if (Input.GetKeyDown(Keybind))
        {
            if (CurrentSpell == Type)
                CurrentSpell = SpellType.Fist;
            else
                CurrentSpell = Type;
            
            SpellUI.SetText(CurrentSpell.ToString());
        }
    }
}
