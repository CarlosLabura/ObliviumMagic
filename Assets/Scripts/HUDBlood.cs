using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class HUDBlood : MonoBehaviour
{
    [HideInInspector] public float Health = 1f;
    private Animator Animator;
    private UnityEngine.UI.Image Image;
    void Start()
    {
        Animator = GetComponent<Animator>();
        Animator.Play("Blood");

        Image = GetComponent<UnityEngine.UI.Image>();
    }
    public void SetHealth(float healthAmount)
    {
        Health = healthAmount;
    }
    void Update()
    {
        Image.fillAmount = Mathf.Lerp(Image.fillAmount, Health, Time.deltaTime*5);
    }
}
