using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct ShopData
{
    public string Name;
    [Min(0)] public float Price;
    [Min(0)] public int Amount;
    public Image Image;
}
public class ShopSystem : MonoBehaviour
{   
    public bool Closed = false;
    public ShopData[] Items;
    [SerializeField] private ShopHUD ShopScript;
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
        if (Closed)
            return;

        if (NPCRoutine.CanSee(NPCRoutine.Target.position) && !InShop)
            SetShop(true);
        else if (!NPCRoutine.CanSee(NPCRoutine.Target.position))
            SetShop(false);

        ShopScript.Background.speed = NPCDialogue.Portrait.speed;
    }

}
