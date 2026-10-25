using UnityEngine;
public class ProjectileCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("PlayerHitbox") && other.gameObject.CompareTag("Enemy") 
        || gameObject.CompareTag("EnemyHitbox") && other.gameObject.CompareTag("Player") 
        || other.gameObject.CompareTag("Wall"))
            Destroy(gameObject);
    }
}