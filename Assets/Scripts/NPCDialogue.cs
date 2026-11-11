using TMPro;
using UnityEngine;
public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue Options")]
    public string DialoguePortrait = "dad";
    [Min(0)] public float TalkSpeed = 0.05f;
    [Min(0)] public float TalkDelay = 3;
    [Multiline] public string[] Dialogue;
    [Header("Text Box Objects")]
    [SerializeField] private GameObject Parent;
    [SerializeField] private TextMeshProUGUI Text;
    public Animator Portrait; //publica para que pueda ser accedida por la tienda
    //variables internas
    [HideInInspector] public bool Talking = false;
    private int CurrentText = 0; // numero del texto que se esta escribiendo
    private int CurrentCharacter = 0; //numero de letra que se tiene que escribir en pantalla
    private string ActualText; // texto que se esta escribiendo
    private float TalkTimer = 0; // timer entre cada letra
    private float DelayTimer = 0; // timer para que pase al siguiente texto
    private bool Talked = false; // termino de hablar
    private bool Forced = false; //forzar a que aparezca el texto
    private NPCRoutine NPCRoutine;
    void Start()
    {
        NPCRoutine = GetComponent<NPCRoutine>();
    }
    void RestartDialogue()
    {
        CurrentCharacter = 0;
        ActualText = "";
        Text.SetText(ActualText);
        TalkTimer = 0;
        DelayTimer = 0;
        Portrait.speed = 1f;
    }
    public void SetDialogue(bool Active, bool Force = false)
    {
        Parent.SetActive(Active);
        RestartDialogue();
        CurrentText = 0;
        Portrait.Play(DialoguePortrait.ToLower());
        Talking = Active;
        Forced = Force;
    }
    void NextDialogue()
    {
        RestartDialogue();
        CurrentText += 1;

        if (CurrentText == Dialogue.Length)
            FinishDialogue();
    }
    public void StartDialogue(bool Force = false)
    {
        SetDialogue(true, Force);
    }
    public void FinishDialogue()
    {
        SetDialogue(false);
        Talked = false;
    }
    void Update()
    {
        if (!Forced) {
            if (NPCRoutine.CanSee(NPCRoutine.Target.position) && !Talking && !Talked)
                StartDialogue();
            else if (!NPCRoutine.CanSee(NPCRoutine.Target.position))
                FinishDialogue();
        } else
        {
            if (!Talking && !Talked)
                StartDialogue();
        }

        if (ActualText == Dialogue[CurrentText])
        {   
            Portrait.speed = 0f;
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
