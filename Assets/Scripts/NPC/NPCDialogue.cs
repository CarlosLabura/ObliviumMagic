using TMPro;
using UnityEngine;
[RequireComponent(typeof(NPCRoutine))]
public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue Options")]
    public string DialoguePortrait;
    [Min(0)] public float TalkSpeed = 0.05f;
    [Min(0)] public float TalkDelay = 1f;
    [Multiline] public string[] Dialogue;
    [Header("Text Box Objects")]
    [SerializeField] private GameObject Parent;
    [SerializeField] private TextMeshProUGUI Text;
    public Animator Portrait;
    private static NPCDialogue ActiveSpeaker; // npc hablando en la caja de texto ahora mismo (compartido entre todos)
    private int CurrentText = 0;
    private int CurrentCharacter = 0;
    private string ActualText = "";
    private float TalkTimer = 0;
    private float DelayTimer = 0;
    private bool Talking = false;
    private NPCRoutine NPCRoutine;
    void Awake()
    {
        NPCRoutine = GetComponent<NPCRoutine>();
    }
    void ResetText()
    {
        CurrentCharacter = 0;
        ActualText = "";
        TalkTimer = 0;
        DelayTimer = 0;
        Text.SetText(ActualText);
        Portrait.speed = 1f;
    }
    void StartDialogue()
    {
        ActiveSpeaker = this;
        Talking = true;
        CurrentText = 0;
        Parent.SetActive(true);
        Portrait.Play(DialoguePortrait.ToLower());
        ResetText();
    }
    void StopDialogue()
    {
        Talking = false;
        CurrentText = 0;
        ResetText();

        if (ActiveSpeaker == this)
        {
            ActiveSpeaker = null;
            Parent.SetActive(false);
        }
    }
    void NextDialogue()
    {
        CurrentText++;
        if (CurrentText >= Dialogue.Length)
        {
            StopDialogue();
            return;
        }
        ResetText();
    }

    void Update()
    {
        if (!NPCRoutine.CanSee(NPCRoutine.Target.position))
        {
            if (Talking) 
                StopDialogue();
            return;
        }
        if (!Talking)
        {
            if (ActiveSpeaker == null)
                StartDialogue();
            else
                return;
        }
        
        string line = Dialogue[CurrentText];
        if (ActualText.Length >= line.Length)
        {
            Portrait.speed = 0f;
            DelayTimer += Time.deltaTime;
            if (DelayTimer >= TalkDelay)
                NextDialogue();
            return;
        }

        TalkTimer += Time.deltaTime;
        if (TalkTimer >= TalkSpeed)
        {
            TalkTimer = 0;
            ActualText += line[CurrentCharacter];
            Text.SetText(ActualText);
            CurrentCharacter++;
        }
    }
}