using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue Options")]
    public string DialoguePortrait;
    public float TalkSpeed;
    public float TalkDelay;
    [Multiline] public string[] Dialogue;
    [Header("HUD Objects")]
    [SerializeField] private GameObject TextBoxObject;
    [SerializeField] private TextMeshProUGUI TextBoxText;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
