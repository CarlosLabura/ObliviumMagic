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

    // Update is called once per frame
    void Update()
    {
        Image.fillAmount = Health;
    }
}
