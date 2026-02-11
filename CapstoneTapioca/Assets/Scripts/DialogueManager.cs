using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI; // added for Button/Text


public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class TagSpritePair {
        [Tooltip("Unity tag of GameObjects to update (this must be a valid Unity Tag)")]
        public string unityTag;

        [Tooltip("Sprite to assign to GameObjects with the matching Unity tag")]
        public Sprite sprite;

        [Header("Ink tag matching (optional)")]
        [Tooltip("If empty, legacy behavior: the Ink tag must equal the Unity tag string. Otherwise, this is the 'key' part of an Ink tag in the form 'key:value' or 'key=value'.")]
        public string inkKey;

        [Tooltip("If set, the Ink tag's value must equal this. Leave empty to match any value for the key.")]
        public string inkValue;
    }

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

    private Story currentStory;
    //private TextAsset currentInkJSON; // remember which ink file started the story

    private bool dialogueIsPlaying;
    
    
    private static DialogueManager instance;

    public GameObject roomCycle1;
    public GameObject roomCycle2;


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

        // show the first line (require a click to advance to each subsequent line)
        ShowNextLine();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel?.SetActive(false);
        dialoguetext.text = "";
        RemoveChildren();
        currentStory = null;
        //currentInkJSON = null;

        roomCycle1.SetActive(true);
        roomCycle2.SetActive(true);
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
            string line = currentStory.Continue().Trim();
            dialoguetext.text = line;

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
            if (string.IsNullOrEmpty(pair.unityTag) || pair.sprite == null) continue;

            // Determine if this mapping matches any of the current Ink tags.
            bool mappingMatches = false;

            if (string.IsNullOrEmpty(pair.inkKey))
            {
                // Legacy behavior: an Ink tag must exactly equal the unityTag string
                if (tags.Contains(pair.unityTag)) mappingMatches = true;
            }
            else
            {
                // New behavior: look for tags of form "key:value" or "key=value"
                foreach (var t in tags)
                {
                    if (string.IsNullOrEmpty(t)) continue;
                    string[] parts = t.Split(new char[] { ':', '=' }, 2);
                    if (parts.Length == 0) continue;
                    if (parts[0] != pair.inkKey) continue;
                    if (string.IsNullOrEmpty(pair.inkValue))
                    {
                        mappingMatches = true; // key matched, value wildcard
                        break;
                    }
                    if (parts.Length == 2 && parts[1] == pair.inkValue)
                    {
                        mappingMatches = true;
                        break;
                    }
                }
            }

            if (!mappingMatches) continue;

            GameObject[] gos;
            try
            {
                gos = GameObject.FindGameObjectsWithTag(pair.unityTag);
            }
            catch
            {
                // If the tag does not exist in Unity's tag manager, skip it
                Debug.LogWarning($"DialogueManager: Unity tag '{pair.unityTag}' not found when applying sprite.");
                continue;
            }

            foreach (var go in gos)
            {
                if (go == null) continue;
                // 2D sprite renderer
                var sr = go.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = pair.sprite;
                    continue;
                }

                // UI Image (for Canvas-based objects)
                var img = go.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                {
                    img.sprite = pair.sprite;
                }
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
            button.onClick.AddListener(delegate {
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
