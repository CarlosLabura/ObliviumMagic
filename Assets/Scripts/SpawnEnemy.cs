using UnityEngine;
using System.Collections.Generic;
public class SpawnEnemy : MonoBehaviour
{
    public int MaxEnemiesAlive = 10;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Enemy;
    private SpellType[] EnemyTypes = {SpellType.Rock, SpellType.Fire, SpellType.Ice, SpellType.Ray};
    private List<GameObject> Enemies = new List<GameObject>();
    public Vector2 GetRandomPositionOutsideCamera()
    {
        Camera cam = MovementSystem.PlayerCamera;

        float distance = Mathf.Abs(cam.transform.position.z);

        Vector2 min = cam.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector2 max = cam.ViewportToWorldPoint(new Vector3(1, 1, distance));

        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0:
                return new Vector2(min.x - 1, Random.Range(min.y, max.y));
            case 1:
                return new Vector2(max.x + 1, Random.Range(min.y, max.y));
            case 2:
                return new Vector2(Random.Range(min.x, max.x), min.y - 1);
            default:
                return new Vector2(Random.Range(min.x, max.x), max.y + 1);
        }
    }
    void Update()
    {
        Enemies.RemoveAll(enemy => enemy == null);

        if (Enemies.Count < MaxEnemiesAlive){
            GameObject NewEnemy = Instantiate(Enemy, GetRandomPositionOutsideCamera(), Quaternion.identity);
            SpellType EnemyType = EnemyTypes[Random.Range(0, EnemyTypes.Length)];
            NewEnemy.GetComponent<HealthSystem>().Weakness = EnemyType;
            NewEnemy.GetComponent<HealthSystem>().MaxHealth = 1;
            NewEnemy.GetComponent<HealthSystem>().Health = 1;
            NewEnemy.GetComponent<SpellSystem>().CurrentSpell = EnemyType;
            Color EnemyColor;
            switch (EnemyType)
            {
                case SpellType.Fire:
                    EnemyColor = Color.orange;
                    break;
                case SpellType.Ice:
                    EnemyColor =  Color.lightBlue;
                    break;
                case SpellType.Ray:
                    EnemyColor =  Color.yellow;
                    break;
                default:
                    EnemyColor =  Color.gray;
                    break;
            }
            NewEnemy.GetComponent<SpriteRenderer>().color = EnemyColor;
            NewEnemy.GetComponent<NPCRoutine>().State = NPCState.Zombie;
            NewEnemy.GetComponent<NPCRoutine>().SightLost = 99;
            NewEnemy.GetComponent<NPCRoutine>().Target = Player.transform;
            Enemies.Add(NewEnemy);
        }
    }
}
