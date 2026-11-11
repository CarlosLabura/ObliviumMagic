using UnityEngine;
public class ShopActivator : MonoBehaviour
{
    [SerializeField] private ShopScript ShopScript;
    [HideInInspector] public bool InShop = false;
    private NPCRoutine NPCRoutine;
    private NPCDialogue NPCDialogue;
    void Start()
    {
        NPCRoutine = GetComponent<NPCRoutine>();
        NPCDialogue = GetComponent<NPCDialogue>();
    }
    void SetShop(bool Active)
    {
        ShopScript.SetShop(Active);
        InShop = Active;
    }
    void Update()
    {
        if (NPCRoutine.CanSee(NPCRoutine.Target.position) && !InShop)
            SetShop(true);
        else if (!NPCRoutine.CanSee(NPCRoutine.Target.position))
            SetShop(false);

        ShopScript.Background.speed = NPCDialogue.Portrait.speed;
    }

}
