using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpell : MonoBehaviour
{
    public SpellType CurrentSpell = SpellType.Fist;
    [SerializeField] private GameObject Hitboxes;
    void Update()
    {
        Hitboxes.transform.position = transform.position;

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Hitboxes.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);
    }
}
