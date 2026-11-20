using UnityEngine;
public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private GameObject Enemy;
    private int EnemyCount = 0;
    public Vector2 GetRandomPositionOutsideCamera()
    {
        Camera cam = MovementSystem.PlayerCamera;

        float distance = Mathf.Abs(cam.transform.position.z);

        Vector2 min = cam.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector2 max = cam.ViewportToWorldPoint(new Vector3(1, 1, distance));

        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: // Izquierda
                return new Vector2(min.x - 1, Random.Range(min.y, max.y));

            case 1: // Derecha
                return new Vector2(max.x + 1, Random.Range(min.y, max.y));

            case 2: // Abajo
                return new Vector2(Random.Range(min.x, max.x), min.y - 1);

            default: // Arriba
                return new Vector2(Random.Range(min.x, max.x), max.y + 1);
        }
    }
    void Update()
    {
        
    }
}
