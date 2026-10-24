using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpell : MonoBehaviour
{
    public SpellType CurrentSpell = 0;
    [SerializeField] private GameObject Hitboxes;
    void Update()
    {
        Hitboxes.transform.position = transform.position;

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Hitboxes.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);

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
        Debug.Log(CurrentSpell);
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
