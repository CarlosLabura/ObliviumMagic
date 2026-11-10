using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue Options")]
    public string DialoguePortrait;
    public float TalkSpeed;
    public float TalkDelay;
    [Multiline] public string[] Dialogue;
    [Header("Text Box Objects")]
    [SerializeField] private GameObject Parent;
    [SerializeField] private TextMeshProUGUI Text;
    //variables internas
    [HideInInspector] public bool Talking = false;
    private int CurrentText = 0; // numero del texto que se esta escribiendo
    private int CurrentCharacter = 0; //numero de letra que se tiene que escribir en pantalla
    private string ActualText; // texto que se esta escribiendo
    private float TalkTimer = 0; // timer entre cada letra
    private float DelayTimer = 0; // timer para que pase al siguiente texto
    private NPCRoutine NPCRoutine;
    void Start()
    {
        NPCRoutine = GetComponent<NPCRoutine>();
    }
    void RestartDialogue()
    {
        CurrentCharacter = 0;
        ActualText = "";
        TalkTimer = 0;
        DelayTimer = 0;
    }
    void SetDialogue(bool Active)
    {
        RestartDialogue();
        CurrentText = 0;

        Parent.SetActive(Active);
        Talking = Active;
    }
    void NextDialogue()
    {
        RestartDialogue();
        CurrentText += 1;

        if (CurrentText == Dialogue.Length)
            SetDialogue(false);
            
    }
    void Update()
    {
        if (NPCRoutine.CanSee(NPCRoutine.Target.position) && !Talking)
            SetDialogue(true);

        if (ActualText == Dialogue[CurrentText])
        {   
            DelayTimer += Time.deltaTime;
            if (DelayTimer < TalkDelay)
                return;
            else
                NextDialogue();
        }

        TalkTimer += Time.deltaTime;
        if (TalkTimer >= TalkSpeed)
        {
            TalkTimer = 0;
            ActualText += Dialogue[CurrentText][CurrentCharacter];
            Text.SetText(ActualText);
            CurrentCharacter += 1;
        }
    }
}
