using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance { get; set; }

    public bool onTarget;

    public GameObject selectedObject;

    public GameObject interaction_Info_UI;
    Text interaction_text;

    public Image centerDotImage;
    public Image handIcon;
    public Image chopIcon;

    public bool handIsVisible;

    public GameObject selectedTree;
    public GameObject chopHolder;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Update()
    {
        // when we are moving mouse pointer to objects then this will give details and wht going on
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactableObject = selectionTransform.GetComponent<InteractableObject>();

            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>(); 

            if(choppableTree && choppableTree.playerInRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if(selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;    
                    chopHolder.gameObject.SetActive(false);
                }
            }

            if (interactableObject && interactableObject.playerInRange)
            {
                onTarget = true;    
                selectedObject = interactableObject.gameObject; 
                interaction_text.text = interactableObject.GetItemName();
                interaction_Info_UI.SetActive(true);

                if (interactableObject.CompareTag("pickable") || interactableObject.CompareTag("Tree") || interactableObject.CompareTag("Rock"))
                {
                    centerDotImage.gameObject.SetActive(false);
                    if (interactableObject.CompareTag("Tree") || interactableObject.CompareTag("Rock"))
                    {
                        chopIcon.gameObject.SetActive(true);
                    }
                    else
                    {
                        handIcon.gameObject.SetActive(true);
                    }

                    handIsVisible = true;
                }
                else
                {
                    centerDotImage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                    chopIcon.gameObject.SetActive(false);

                    handIsVisible = false;  
                }

            }
            else
            {
                onTarget = false;

                interaction_Info_UI.SetActive(false);

                if (!DialogSystem.Instance.dialogUIActive)
                {
                    centerDotImage.gameObject.SetActive(true);
                }
                handIcon.gameObject.SetActive(false);
                chopIcon.gameObject.SetActive(false);

                handIsVisible = false;  
            }

            NPC npc = selectionTransform.GetComponent<NPC>();
            if (npc && npc.playerInRange)
            {
                interaction_text.text = "Talk";
                interaction_Info_UI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E) && npc.isTalkingWithPlayer == false)
                {
                    npc.StartConversation();
                }

                if (DialogSystem.Instance.dialogUIActive)
                {
                    interaction_Info_UI.SetActive(false);
                    centerDotImage.gameObject.SetActive(false);
                }
            }
            if (DialogSystem.Instance.dialogUIActive)
            {
                interaction_Info_UI.SetActive(false);
                centerDotImage.gameObject.SetActive(false);

                handIcon.gameObject.SetActive(false);
                chopIcon.gameObject.SetActive(false);
            }

        }
        else
        {
            onTarget = false;

            interaction_Info_UI.SetActive(false);

            if (!DialogSystem.Instance.dialogUIActive)
            {
                centerDotImage.gameObject.SetActive(true);
            }

            handIcon.gameObject.SetActive(false);
            chopIcon.gameObject.SetActive(false);
            chopHolder.gameObject.SetActive(false);

            handIsVisible = false;
        }
    }

    public void DisableSelection()
    {
        handIcon.enabled = false;
        chopIcon.enabled = false;
        centerDotImage.enabled = false;
        interaction_Info_UI.SetActive(false);

        selectedObject = null;
    }

    public void EnableSelection()
    {
        handIcon.enabled = true;
        chopIcon.enabled = true;
        centerDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}