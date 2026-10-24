using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpell : MonoBehaviour
{
    public SpellType CurrentSpell = SpellType.Fist;
    [SerializeField] private GameObject Hitboxes; //uso esto para el pivote este en el centro del jugador
    [SerializeField] private GameObject FistHitbox;
    private float Cooldown = 0;
    void Update()
    {
        if (Cooldown > 0) {
            Cooldown -= Time.deltaTime;
        }

        // hitboxes fijas apuntan al mouse
        Hitboxes.transform.position = transform.position;

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Hitboxes.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);

        if (Cooldown <= 0){
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
                        break;
                    case SpellType.Fire:
                        break;
                    case SpellType.Ice:
                        break;
                    case SpellType.Electricty:
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
