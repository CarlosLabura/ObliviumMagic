using UnityEngine;
public class ShopScript : MonoBehaviour
{
    public Animator Background;
    public void SetShop(bool Active)
    {
        gameObject.SetActive(Active);
    }
}
