using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct SpellData
{
    public SpellType Type;
    public bool Unlocked;
    public Image SpellBox;
}
public class SpellSystem : MonoBehaviour
{
    //hechizos
    public SpellType CurrentSpell = SpellType.Fist;
    [Header("Spell System")]
    [Min(0)] public float SpellCooldown = 1.38f;
    private float Cooldown = 0;
    private int SpellStart = 1; // index donde empiezan los hechizos sin contar los fists
    private int SpellCycle = 1; //empieza en uno para hacer que no se elijan los fist
    private float FistTime = 0f;
    //variables internas
    [SerializeField] private GameObject Projectile;
    [ShowIf(typeof(MovementSystem), "Player")] [Header("Player Settings")] public SpellData[] PlayerSpells;
    [ShowIf(typeof(MovementSystem), "Player")] [SerializeField] private GameObject FistHitbox;
    [ShowIf(typeof(MovementSystem), "Player")] [SerializeField] private CursorReplace CursorReplace;
    private MovementSystem MovementSystem;
    public Vector3 GetCursorPosition()
    {
        return CursorReplace.GetCursorPosition();
    }
    void Start()
    {
        MovementSystem = GetComponent<MovementSystem>();
    }
    private void Spell(SpellType Type, float Speed, Vector3 Scale, Vector3 Target)
    {
        GameObject projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
        projectile.GetComponent<SpellTypeScript>().Type = Type;
        projectile.transform.localScale = Scale;
        Vector2 projectileDirection = (Target - transform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().rotation = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        projectile.GetComponent<Rigidbody2D>().AddForce(projectileDirection * Speed, ForceMode2D.Impulse);
        Destroy(projectile, 5f);
    }
    public void CastSpell(SpellType Type, Vector3 Target)
    {
        if (Cooldown > 0 || MovementSystem.IsStunned)
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
        
        //solo el player puede pegar puñetes
        if (FistTime > 0)
            FistTime -= Time.deltaTime;
        else
            FistHitbox.SetActive(false);
    
        if (Input.GetMouseButton(0))
        {
            CastSpell(CurrentSpell, GetCursorPosition());
        }

        if (PlayerSpells.Length <= 1)
            return;

        // cambio de hechizos
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeSpell(0); //0 deberian ser los puños
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.R))
        {
            SpellCycle += 1;
            if (SpellCycle >= PlayerSpells.Length)
                SpellCycle = SpellStart;
            ChangeSpell(SpellCycle);
        } 
        SpellKeybind(1, KeyCode.Alpha1); //el numero del hechizo se consigue por orden de añadido
        SpellKeybind(2, KeyCode.Alpha2);
        SpellKeybind(3, KeyCode.Alpha3);
        SpellKeybind(4, KeyCode.Alpha4);

        //hud de hechizos
        for (int i = SpellStart; i < PlayerSpells.Length; i++)
        {
            SpellData Spell = PlayerSpells[i];
            float alpha = 0f;
            if (Spell.Unlocked)
                alpha = 1f;
            
            float color = 0.5f;
            if (i == SpellCycle)
                color = 1f;

            Spell.SpellBox.color = Color.Lerp(Spell.SpellBox.color, new Color(color, color, color, alpha), Time.deltaTime * 5);
        }
    }
    private void ChangeSpell(int SpellIndex)
    {
        if (SpellIndex >= PlayerSpells.Length || SpellIndex < 0)
            SpellIndex = 0;

        SpellData Spell = PlayerSpells[SpellIndex];
        
        if (Spell.Unlocked) {
            CurrentSpell = Spell.Type;
        } else {
            ChangeSpell(SpellIndex+1);
            return;
        }

        SpellCycle = SpellIndex;

        CursorReplace.ChangeCursor(CurrentSpell);
    }
    private void SpellKeybind(int Type, KeyCode Keybind)
    {
        if (Input.GetKeyDown(Keybind))
        {
            if (CurrentSpell == PlayerSpells[Type].Type)
                ChangeSpell(0);
            else
                ChangeSpell(Type);
        }
    }
}
