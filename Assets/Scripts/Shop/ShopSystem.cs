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
[RequireComponent(typeof(NPCRoutine))]
[RequireComponent(typeof(NPCDialogue))]
public class ShopSystem : MonoBehaviour
{   
    public bool Closed = false;
    public ShopData[] Items;
    [SerializeField] private ShopHUD ShopScript;
    private NPCRoutine NPCRoutine;
    private NPCDialogue NPCDialogue;
    void Awake()
    {
        NPCRoutine = GetComponent<NPCRoutine>();
        NPCDialogue = GetComponent<NPCDialogue>();
    }
    void SetShop(bool Active)
    {
        ShopScript.SetShop(Active);
    }
    void Update()
    {
        if (Closed)
            return;

        SetShop(NPCRoutine.CanSee(NPCRoutine.Target.position));
        ShopScript.Background.speed = NPCDialogue.Portrait.speed;
    }

}
