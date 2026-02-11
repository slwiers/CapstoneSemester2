using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour, IPointerClickHandler
{

    [Header("Character")]
    [SerializeField] private string CharacterName;

    [Header("Activation (optional)")]
    [Tooltip("Optional GameObject to enable/disable when this trigger is clicked")]
    [SerializeField] private GameObject activateTarget;
    [SerializeField] private bool setActive = true;
    [SerializeField] private UnityEvent onClicked; // assign custom reactions in the Inspector


    private Camera _mainCamera;

    private void Awake() {
        _mainCamera = Camera.main;
    }

    public void OnPointerClick(PointerEventData eventData) {
        // ask DialogueManager to handle the click:
        // - if a story is playing -> advance it
        // - else -> start the provided ink story
        var dm = DialogueManager.GetInstance();
        if (dm != null) dm.HandleClick(CharacterName);

        if (activateTarget != null) {
            activateTarget.SetActive(setActive);
        }

        onClicked.Invoke();
    }

}
