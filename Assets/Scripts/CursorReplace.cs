using UnityEngine;

public class CursorReplace : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        gameObject.transform.position = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            gameObject.transform.position.z
        );
    }
}
