using UnityEngine;

public class CursorReplace : MonoBehaviour
{
    private Animator Animator;
    void Start()
    {
        Cursor.visible = false;
        Animator = GetComponent<Animator>();
    }
    void Update()
    {
        gameObject.transform.position = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            gameObject.transform.position.z
        );
    }
    public void ChangeCursor(SpellType Type)
    {
        Animator.Play(Type.ToString());
    }
    public Vector3 GetCursorPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        return mousePosition;
    }
}
