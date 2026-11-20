using UnityEngine.UI;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Image))]
public class HUDBlood : MonoBehaviour
{
    [SerializeField] private Animator BloodBall;
    [HideInInspector] public float Health = 1f;
    private Animator Animator;
    private Image Image;
    void Awake()
    {
        Animator = GetComponent<Animator>();
        Image = GetComponent<Image>();
    }
    void Start()
    {
        Animator.Play("Blood");
    }
    public void SetHealth(float healthAmount)
    {
        Health = healthAmount;
        if (Health <= 0.25)
            BloodBall.Play("Low");
        else if (Health <= 0.5)
            BloodBall.Play("Half");
        else
            BloodBall.Play("Full");
    }
    void Update()
    {
        Image.fillAmount = Mathf.Lerp(Image.fillAmount, Health, Time.deltaTime*5);
    }
}
