using UnityEngine;
public class ProjectileCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("PlayerHitbox") && other.gameObject.CompareTag("Enemy") //projectil del player choca con el enemigo
        || gameObject.CompareTag("EnemyHitbox") && other.gameObject.CompareTag("Player")  //projectil del enemigo choca con el player
        || other.gameObject.CompareTag("Wall")) //projectil choca con una pared
            Destroy(gameObject);
    }
}