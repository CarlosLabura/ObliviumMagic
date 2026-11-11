using UnityEngine;
public class ShopScript : MonoBehaviour
{
    [SerializeField] private Animator Background;
    private NPCDialogue NPCDialogue;
    void Start()
    {
        NPCDialogue = GetComponent<NPCDialogue>();
    }
    public void SetShop(bool Active)
    {
        gameObject.SetActive(Active);
        NPCDialogue.SetDialogue(Active, true);
    }
    void Update()
    {
        Background.speed = NPCDialogue.Portrait.speed;

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape)) {
            SetShop(false);
            NPCDialogue.FinishDialogue();
        }
    }
}
