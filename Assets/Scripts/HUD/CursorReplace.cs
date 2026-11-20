using UnityEngine;
[RequireComponent(typeof(Animator))]
public class CursorReplace : MonoBehaviour
{
    private Animator Animator;
    void Awake()
    {
        Animator = GetComponent<Animator>();
    }
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
    public void ChangeCursor(SpellType Type)
    {
        Animator.Play(Type.ToString());
    }
    static public Vector3 GetCursorPosition()
    {
        Vector3 mousePosition = MovementSystem.PlayerCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        return mousePosition;
    }
}
