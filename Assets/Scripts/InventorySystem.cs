using UnityEngine;
[System.Serializable]
public struct ItemData
{
    public string Name;
    [Min(0)] public int Amount;
    //public Image SpellBox;
}
public class InventorySystem : MonoBehaviour
{
    public ItemData[] Items;
    public int GetItemAmount(string Name)
    {
        int Amount = 0;

        for (int i = 0; i < Items.Length; i++)
            if (Items[i].Name == Name)
                Amount = Items[i].Amount;

        return Amount;
    }
}
