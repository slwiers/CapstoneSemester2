using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI; // added for Button/Text
using UnityEngine.SceneManagement;


public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class TagSpritePair {

        [Tooltip("Sprite to assign to GameObjects with the matching Unity tag")]
        public Sprite sprite;
        public bool isFace;

        [Header("Ink tag matching (optional)")]
        [Tooltip("If empty, legacy behavior: the Ink tag must equal the Unity tag string. Otherwise, this is the 'key' part of an Ink tag in the form 'key:value' or 'key=value'.")]
        public string inkKey;

    }
    [SerializeField] private SpriteRenderer characterSpriteRenderer;
    [SerializeField] private SpriteRenderer faceSpriteRenderer;

    [Header("Tag -> Sprite mapping (change GameObject sprites by tag)")]
    [Tooltip("List of mappings. You can use either the legacy mode (leave 'inkKey' empty and use the Unity tag as the Ink tag), or set 'inkKey' (and optionally 'inkValue') to match Ink tags like 'character:happy'.")]
    [SerializeField] private TagSpritePair[] tagSpritePairs;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialoguetext;

    // choice UI
    [Header("Choice UI")]
    [Tooltip("Button prefab (should have a UnityEngine.UI.Button component and a Text child)")]
    [SerializeField] private Button buttonPrefab;
    [Tooltip("Parent transform where choice buttons will be instantiated")]
    [SerializeField] private RectTransform choicesContainer;
    [Tooltip("Optional offset (local) applied to each created button")]
    [SerializeField] private Vector2 choiceLocalOffset = Vector2.zero;
    [Tooltip("Manual spacing used when choicesContainer does not have a Layout Group")]
    [SerializeField] private float choiceSpacing = 40f;
    // You can call SetChoicesContainer(Transform) at runtime to change where buttons spawn.
    public void SetChoicesContainer(Transform t) { choicesContainer = t as RectTransform; }

    public Story currentStory;
    //private TextAsset currentInkJSON; // remember which ink file started the story

    public bool dialogueIsPlaying;
    
    
    private static DialogueManager instance;

    public GameObject roomCycle1;
    public GameObject roomCycle2;
    public GameObject gameDirections;

    [SerializeField] private float typingSpeed = 0.04f;
    private Coroutine displyLineCororoutine;

    public GameObject objectToTurnOff;
    public GameObject objectToTurnOff2;

    public GameObject loadingScreen;
    public Slider slider;

    public ClayPieceManagement pieceManagement;

    public LevelLoader levelLoader;

    public WinCondition winCondition;

    public RoomStateManager roomStateManager;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the Scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }
    }

    // public entrypoint: handle pointer clicks from triggers
    public void HandleClick(string CharacterName)
    {
        // If a story is currently playing, advance it instead of restarting
        if (dialogueIsPlaying)
        {
            ContinueStory();
            return;
        }

        if(GlobalDialogueManager.currentStory == null)
        {
            GlobalDialogueManager.CreateStory();
        }

        currentStory = GlobalDialogueManager.currentStory;
        GlobalDialogueManager.JumpToCharacter(CharacterName);

        EnterDialogueMode();
        
    }

    public void EnterDialogueMode()
    {
        
        dialogueIsPlaying = true;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);

        roomCycle1.SetActive(false);
        roomCycle2.SetActive(false);
        gameDirections.SetActive(false);

        // show the first line (require a click to advance to each subsequent line)
        ShowNextLine();
    }

    public void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel?.SetActive(false);
        dialoguetext.text = "";
        RemoveChildren();
        currentStory = null;
        //currentInkJSON = null;

        roomCycle1.SetActive(true);
        roomCycle2.SetActive(true);
        gameDirections.SetActive(true);
    }

    // made public so other classes (e.g. triggers) can advance the story
    public void ContinueStory()
    {
        // advance by a single line (or show choices) per click
        ShowNextLine();
    }

    // Show exactly one line from the story, or present choices if no more lines
    void ShowNextLine()
    {
        RemoveChildren();

        if (currentStory == null)
        {
            ExitDialogueMode();
            return;
        }

        // If there's more content, show the next line (single Continue call)
        if (currentStory.canContinue)
        {

            // string line = currentStory.Continue().Trim();
            //dialoguetext.text = line;
            if (displyLineCororoutine != null)
            {
                StopCoroutine(displyLineCororoutine);
            }

            displyLineCororoutine = StartCoroutine(DisplayLine(currentStory.Continue().Trim()));

            

            // Apply any tag-driven sprite changes produced by this line/story state
            ApplyTagsAndChangeSprites(currentStory.currentTags);

            // If story produced choices immediately after this line, show them now
            if (currentStory.currentChoices.Count > 0)
            {
                CreateChoices();
            }
            return;
        }

        // No more direct content; if there are choices, display them
        if (currentStory.currentChoices.Count > 0)
        {
            // Keep the last line visible (do not clear) and show choices below
            CreateChoices();
            return;
        }

        // Nothing to show and no choices -> end
        ExitDialogueMode();
    }

    private IEnumerator DisplayLine(string line)
    {
        dialoguetext.text = "";

        foreach (char letter in line.ToCharArray())
        {
            if (Input.GetButton("Click"))
            {
                dialoguetext.text = line;
                break;
            }

            dialoguetext.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

    }

    // Applies sprite changes for any tags present in the current Ink story state.
    // For each configured TagSpritePair, if the story's currentTags contains the pair.tag,
    // all GameObjects with that Unity tag will have their SpriteRenderer (or UI Image) updated.
    void ApplyTagsAndChangeSprites(List<string> tags)
    {
        if (tags == null || tags.Count == 0) return;
        if (tagSpritePairs == null || tagSpritePairs.Length == 0) return;

        foreach (var pair in tagSpritePairs)
        {
            if (pair == null) continue;

            // Determine if this mapping matches any of the current Ink tags.
            bool mappingMatches = false;

            if (string.IsNullOrEmpty(pair.inkKey))
            {
                // Legacy behavior: an Ink tag must exactly equal the unityTag string
                mappingMatches = true;
            }
            else
            {

                // New behavior: look for tags of form "key:value" or "key=value"
                foreach (var t in tags)
                {
                    if (t != pair.inkKey) continue;
                    mappingMatches = true; // key matched, value wildcard
                    break;
                }
            }

            if (!mappingMatches) continue;

            SpriteRenderer animatedSpriteRenderer = pair.isFace? faceSpriteRenderer:characterSpriteRenderer;
            animatedSpriteRenderer.sprite = pair.sprite;
            
        }
        if (tags.Count > 0)
                {
                    if (tags[0] == "valepuzzle1")
                    {

                        levelLoader.LoadLevel(10);

                    }
                    if (tags[0] == "valepuzzle2")
                    {
                        levelLoader.LoadLevel(11);

                    }
                    if (tags[0] == "valepuzzle3")
                    {
                        levelLoader.LoadLevel(12);

                    }
                    if (tags[0] == "horacepuzzle1")
                    {
                        levelLoader.LoadLevel(13);
                        Debug.Log("Scene Loaded");

                    }
                    if (tags[0] == "horacepuzzle2")
                    {
                        levelLoader.LoadLevel(14);
                        Debug.Log("Scene Loaded");

                    }
                    if (tags[0] == "horacepuzzle3")
                    {
                        levelLoader.LoadLevel(15);
                        Debug.Log("Scene Loaded");

                    }
                    if (tags[0] == "introtrans")
                    {
                        loadingScreen.SetActive(true);
                        levelLoader.LoadLevel(6);
                        Debug.Log("Scene Loaded");

                    }
                    if (tags[0] == "TurnOff")
                    {

                        objectToTurnOff.SetActive(false);
                    }
                    if (tags[0] == "TurnOn")
                    {

                        objectToTurnOff.SetActive(true);
                    }
                    if (tags[0] == "ClayPiece1")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.triggerClayPiece1 = true;

                    }
                    if (tags[0] == "ByeBye")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.killClayPiece1 = true;
                        Debug.Log("Worked");

                    }
                    if (tags[0] == "ClayPiece2")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.triggerClayPiece2 = true;

                    }
                    if (tags[0] == "ByeBye2")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.killClayPiece2 = true;

                    }
                    if (tags[0] == "ClayPiece3")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.triggerClayPiece3 = true;

                    }
                    if (tags[0] == "ByeBye3")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.killClayPiece3 = true;

                    }
                    if (tags[0] == "ClayPiece4")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.triggerClayPiece4 = true;
                        

                    }
                    if (tags[0] == "ByeBye4")
                    {
                        Debug.Log("In");
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.killClayPiece4 = true;
                        Debug.Log("True");

                    }
                    if (tags[0] == "ClayPiece5")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.triggerClayPiece5 = true;

                    }
                    if (tags[0] == "ByeBye5")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.killClayPiece5 = true;

                    }
                    if (tags[0] == "ClayPiece6")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.triggerClayPiece6 = true;

                    }
                    if (tags[0] == "ByeBye6")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.killClayPiece6 = true;

                    }
                    if (tags[0] == "ClayPiece7")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.triggerClayPiece7 = true;


                    }
                    if (tags[0] == "ByeBye7")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.killClayPiece7 = true;

                    }
                    if (tags[0] == "ClayPiece8")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.triggerClayPiece8 = true;

                    }
                    if (tags[0] == "ByeBye8")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.killClayPiece8 = true;

                    }
                    if (tags[0] == "ClayPiece9")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.triggerClayPiece9 = true;

                    }
                    if (tags[0] == "ByeBye9")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.killClayPiece9 = true;

                    }
                    if (tags[0] == "DPlant")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.triggerDPlant = true;

                    }
                    if (tags[0] == "ByeByePlant")
                    {
                        ClayPieceManagement instance = FindAnyObjectByType<ClayPieceManagement>();
                        instance.killDPlant = true;

                    }
                    if(tags[0] == "WakeyWakey")
                    {
                        objectToTurnOff2.SetActive(true);
                    }
                    if (tags[0] == "saved1")
                    {
                        WinCondition instance = FindAnyObjectByType<WinCondition>();
                        instance.savedNPC1 = true;
                        Debug.Log("Saved Character 1");
                    }
                    if (tags[0] == "saved2")
                    {
                        WinCondition instance = FindAnyObjectByType<WinCondition>();
                        instance.savedNPC2 = true;
                        Debug.Log("Saved Character 2");
                    }
                    if (tags[0] == "saved3")
                    {
                        WinCondition instance = FindAnyObjectByType<WinCondition>();
                        instance.savedNPC3 = true;
                        Debug.Log("Saved Character 3");
                    }
                    if (tags[0] == "saved4")
                    {
                        WinCondition instance = FindAnyObjectByType<WinCondition>();
                        instance.savedNPC4 = true;
                        Debug.Log("Saved Character 4");
                    }
                    if (tags[0] == "saved5")
                    {
                        WinCondition instance = FindAnyObjectByType<WinCondition>();
                        instance.savedNPC5 = true;
                        Debug.Log("Saved Character 5");
                    }
                    if (tags[0] == "FoxesPuzzle1")
                    {
                        levelLoader.LoadLevel(18);
                        Debug.Log("Scene Loaded");

                    }
                    if (tags[0] == "FoxesPuzzle2")
                    {
                        levelLoader.LoadLevel(19);
                        Debug.Log("Scene Loaded");

                    }
                    if (tags[0] == "FoxesPuzzle3")
                    {
                        levelLoader.LoadLevel(20);
                        Debug.Log("Scene Loaded");

                    }
                    if(tags[0] == "DamDestroy")
                    {
                        RoomStateManager instance = FindAnyObjectByType<RoomStateManager>();
                        instance.valeDamDown = true;
                    }
                    if (tags[0] == "MatrixUp")
                    {
                        RoomStateManager instance = FindAnyObjectByType<RoomStateManager>();
                        instance.matrixUp = true;
                    }
                    if (tags[0] == "MatrixDown")
                    {
                        RoomStateManager instance = FindAnyObjectByType<RoomStateManager>();
                        instance.matrixUp = false;
                    }

        }
    }

    // build and show choice buttons for current choices
    void CreateChoices()
    {
            if (buttonPrefab == null || choicesContainer == null)
            {
                Debug.LogWarning("Button prefab or choices container not assigned in DialogueManager.");
                return;
            }

            // detect if container has any layout groups — if so, layout will handle spacing
            bool hasLayout = choicesContainer.GetComponent<VerticalLayoutGroup>() != null
                             || choicesContainer.GetComponent<HorizontalLayoutGroup>() != null
                             || choicesContainer.GetComponent<GridLayoutGroup>() != null;

            for (int i = 0; i < currentStory.currentChoices.Count; i++)
            {
                Choice choice = currentStory.currentChoices[i];
                Button button = CreateChoiceView(choice.text.Trim(), i, hasLayout);
                if (button == null) continue;
                int choiceIndex = choice.index; // capture for closure
                button.onClick.AddListener(delegate
                {
                    OnClickChoiceButton(choiceIndex);
                });
            }
    }

    // Destroys all the children of the choicesContainer
    void RemoveChildren () {
        if (choicesContainer == null) return;
        int childCount = choicesContainer.childCount;
        for (int i = childCount - 1; i >= 0; --i) {
            Destroy (choicesContainer.GetChild (i).gameObject);
        }
    }

    Button CreateChoiceView (string text, int index, bool containerHasLayout) {
        if (buttonPrefab == null || choicesContainer == null) {
            Debug.LogWarning("Button prefab or choices container not assigned in DialogueManager.");
            return null;
        }

        // Create the button (instantiate without parent so we can control parenting explicitly)
        Button choice = Instantiate(buttonPrefab);
        // Parent into the choicesContainer and keep local transform (so RectTransform anchors/pos work)
        choice.transform.SetParent(choicesContainer, false);
        choice.transform.localScale = Vector3.one;

        // Try to set text - support both Text (legacy) and TMP
        Text uiText = choice.GetComponentInChildren<Text>();
        if (uiText != null) {
            uiText.text = text;
        } else {
            TextMeshProUGUI tmp = choice.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null) tmp.text = text;
        }

        // apply optional local offset (useful if you want to nudge the buttons)
        RectTransform r = choice.GetComponent<RectTransform>();
        if (r != null) {
            // If the container has a layout group, let it control positions.
            if (!containerHasLayout) {
                Vector2 basePos = choiceLocalOffset;
                // stack downward by index
                basePos.y -= index * choiceSpacing;
                r.anchoredPosition = basePos;
            } else {
                // if user provided an offset, apply as small nudge (layout groups may override)
                if (choiceLocalOffset != Vector2.zero) r.anchoredPosition += choiceLocalOffset;
            }
        }

        // Make the button expand to fit the text if it has a HorizontalLayoutGroup
        HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
        if (layoutGroup != null) layoutGroup.childForceExpandHeight = false;

        return choice;
    }

    // When we click the choice button, tell the story to choose that choice!
    void OnClickChoiceButton (int choiceIndex) {
        if (currentStory == null) return;
        currentStory.ChooseChoiceIndex (choiceIndex);
        // after choosing, show the next line (or choices) — requires another click to continue beyond that line
        ShowNextLine();
    }

}
