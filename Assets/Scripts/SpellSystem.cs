using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SpellSystem : MonoBehaviour
{
    //hechizos
    public SpellType CurrentSpell = SpellType.Fist;
    [Header("Spell System")]
    [Min(0)] public float SpellCooldown = 1.38f;
    private float Cooldown = 0;
    private int SpellCycle = 1; //empieza en uno para hacer que no se elijan los fist
    private float FistTime = 0f;
    //variables internas
    [SerializeField] private GameObject Projectile;
    [ShowIf(typeof(MovementSystem), "Player")] [Header("Player Settings")] public SpellType[] UnlockedSpells = {SpellType.Fist};
    [ShowIf(typeof(MovementSystem), "Player")] [SerializeField] private Transform Hitboxes; //uso esto para el pivote este en el centro del jugador
    [ShowIf(typeof(MovementSystem), "Player")] [SerializeField] private GameObject FistHitbox;
    [ShowIf(typeof(MovementSystem), "Player")] [SerializeField] private CursorReplace CursorReplace;
    [ShowIf(typeof(MovementSystem), "Player")] [SerializeField] private TextMeshProUGUI SpellUI; //temporal
    private MovementSystem MovementSystem;
    void Start()
    {
        MovementSystem = GetComponent<MovementSystem>();
    }
    private void Spell(SpellType Type, float Speed, Vector3 Scale, Vector3 Target) //mousePosition
    {
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
                    Cooldown = SpellCooldown/2;
                    FistTime = 0.15f;
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

        // toda la logica de aca solo se aplica si el script lo tiene un jugador
        if (! MovementSystem.Player)
            return;
        
        if (FistTime > 0)
            FistTime -= Time.deltaTime;
        else
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

        if (UnlockedSpells.Length <= 1)
            return;

        // cambio de hechizos
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeSpell(SpellType.Fist);
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.R))
        {
            SpellCycle += 1;
            if (SpellCycle >= UnlockedSpells.Length)
                SpellCycle = 1;
            ChangeSpell(UnlockedSpells[SpellCycle]);
        } 
        SpellKeybind(SpellType.Rock, KeyCode.Alpha1);
        SpellKeybind(SpellType.Fire, KeyCode.Alpha2);
        SpellKeybind(SpellType.Ice, KeyCode.Alpha3);
        SpellKeybind(SpellType.Ray, KeyCode.Alpha4);
    }
    private void ChangeSpell(SpellType Spell)
    {
        if (UnlockedSpells.Contains(Spell))
            CurrentSpell = Spell;
        else
            CurrentSpell = UnlockedSpells[0];
        
        CursorReplace.ChangeCursor(CurrentSpell);
        SpellUI.SetText(CurrentSpell.ToString());
    }
    private void SpellKeybind(SpellType Type, KeyCode Keybind)
    {
        if (Input.GetKeyDown(Keybind))
        {
            if (CurrentSpell == Type)
                ChangeSpell(SpellType.Fist);
            else
                ChangeSpell(Type);
        }
    }
}
