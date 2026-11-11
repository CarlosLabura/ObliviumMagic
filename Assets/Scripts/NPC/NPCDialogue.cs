using TMPro;
using UnityEngine;
public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue Options")]
    public string DialoguePortrait;
    [Min(0)] public float TalkSpeed;
    [Min(0)] public float TalkDelay;
    [Multiline] public string[] Dialogue;
    [Header("Text Box Objects")]
    [SerializeField] private GameObject Parent;
    [SerializeField] private TextMeshProUGUI Text;
    public Animator Portrait;
    //variables internas
    [HideInInspector] public bool Talking = false;
    private int CurrentText = 0; // numero del texto que se esta escribiendo
    private int CurrentCharacter = 0; //numero de letra que se tiene que escribir en pantalla
    private string ActualText; // texto que se esta escribiendo
    private float TalkTimer = 0; // timer entre cada letra
    private float DelayTimer = 0; // timer para que pase al siguiente texto
    private NPCRoutine NPCRoutine;
    private bool Talked = false;
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
    void SetDialogue(bool Active)
    {
        RestartDialogue();
        CurrentText = 0;
        Portrait.Play(DialoguePortrait.ToLower());

        Parent.SetActive(Active);
        Talking = Active;
    }
    void NextDialogue()
    {
        RestartDialogue();
        CurrentText += 1;

        if (CurrentText == Dialogue.Length) {
            SetDialogue(false);
            Talked = true;
        }
    }
    void Update()
    {
        //REHACER PARA QUE FUNQUE CON DIFERENTES NPCS
        //AHORA SE BUGEA Y MUESTRA 2 MENSAJES AL MISMO TIEMPO????
        //UNITY ESTA GAGA
        //TEL AVIV NO LES ESTA MANDANDO SUFICIENTES RECURSOS
        /*
        if (NPCRoutine.CanSee(NPCRoutine.Target.position) && !Talking && !Talked)
            SetDialogue(true);
        else if (!NPCRoutine.CanSee(NPCRoutine.Target.position) && Talking){
            SetDialogue(false);
            Talked = false;
        }
        */

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