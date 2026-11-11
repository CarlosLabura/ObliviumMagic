using UnityEngine;

public class ShopActivator : MonoBehaviour
{
    [SerializeField] private ShopScript ShopScript;
    public float Cooldown = 5f;
    private bool EnteredShop = false;
    private float CooldownTimer = 0f; 
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !EnteredShop && CooldownTimer <= 0) {
            ShopScript.SetShop(true);
            EnteredShop = true;
        }
    }
    void Update()
    {
        if (CooldownTimer > 0)
            CooldownTimer -= Time.deltaTime;
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && EnteredShop) {
            EnteredShop = false;
            CooldownTimer = Cooldown;
        }
    }

}
