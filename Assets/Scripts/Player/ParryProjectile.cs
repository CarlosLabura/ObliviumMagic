using UnityEngine;
public class ParryProjectile : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        // si el enemigo recibe un trigger de la hitbox del player or el player recibe hitbox de un enemigo
        if (other.gameObject.CompareTag("EnemyHitbox") && gameObject.activeSelf)
        {
            other.gameObject.tag = "PlayerHitbox";

            Vector2 projectileDirection = (CursorReplace.GetCursorPosition() - transform.position).normalized;
            
            other.gameObject.GetComponent<Rigidbody2D>().rotation = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(projectileDirection * 20f, ForceMode2D.Impulse);
        }
    }
}
