using UnityEngine;
using UnityEngine.UI;
public class ShopHUD : MonoBehaviour
{
    public Animator Background;
    public void SetShop(bool Active)
    {
        gameObject.SetActive(Active);
    }
}
