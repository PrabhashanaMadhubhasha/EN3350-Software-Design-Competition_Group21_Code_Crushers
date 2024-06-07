using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public bool playerInRange;

    public bool isTalkingWithPlayer;

    public int activeHourAgent;
    public int deactiveHourAgent;

    public int costForHealer;
    public int costForChef;

    Text npcDialogText;

    Button optionButton1;
    Text optionButton1Text;

    Button optionButton2;
    Text optionButton2Text;

    public List<Quest> quests;
    public Quest currentActiveQuest = null;
    public int activeQuestIndex = 0;
    public bool firstTimeInteraction = true;
    public int currentDialog;

    private void Start()
    {
        npcDialogText = DialogSystem.Instance.dialogText;

        optionButton1 = DialogSystem.Instance.option1BTN;
        optionButton1Text = DialogSystem.Instance.option1BTN.transform.Find("Text").GetComponent<Text>();

        optionButton2 = DialogSystem.Instance.option2BTN;
        optionButton2Text = DialogSystem.Instance.option2BTN.transform.Find("Text").GetComponent<Text>();
    }

    public void StartConversation()
    {
        isTalkingWithPlayer = true;

        LookAtPlayer();

        // Interacting with the NPC for the first time
        if (firstTimeInteraction)
        {
            firstTimeInteraction = false;
            currentActiveQuest = quests[activeQuestIndex]; // 0 at start
            StartQuestInitialDialog();
            currentDialog = 0;
        }
        else // Interacting with the NPC after the first time
        {
            
            // If we return after declining the quest
            if (currentActiveQuest.declined)
            {

                DialogSystem.Instance.OpenDialogUI();

                npcDialogText.text = currentActiveQuest.info.comebackAfterDecline;

                SetAcceptAndDeclineOptions();
            }


            // If we return while the quest is still in progress
            if (currentActiveQuest.accepted && currentActiveQuest.isCompleted == false)
            {
                if (AreQuestRequirmentsCompleted())
                {
                    FinishQuestAfterCompletedRequirements();
                }
                else
                {
                    DialogSystem.Instance.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.info.comebackInProgress;

                    optionButton1Text.text = "[Close]";
                    optionButton1.onClick.RemoveAllListeners();
                    optionButton1.onClick.AddListener(() => {
                        DialogSystem.Instance.CloseDialogUI();
                        isTalkingWithPlayer = false;
                    });
                }
            }
 
            // If there is another quest available
            if (currentActiveQuest.initialDialogCompleted == false)
            {
                StartQuestInitialDialog();
            }

            if (currentActiveQuest.isCompleted == true)
            {
                FinishTheQuest();

            }
        }

    }

    private void SetAcceptAndDeclineOptions()
    {
        optionButton1Text.text = currentActiveQuest.info.acceptOption;
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            AcceptedQuest();
        });

        optionButton2.gameObject.SetActive(true);
        optionButton2Text.text = currentActiveQuest.info.declineOption;
        optionButton2.onClick.RemoveAllListeners();
        optionButton2.onClick.AddListener(() => {
            DeclinedQuest();
        });
    }

    private void SubmitRequiredItems()
    {
        string firstRequiredItem = currentActiveQuest.info.firstRequirmentItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        if (firstRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(firstRequiredItem, firstRequiredAmount);
        }


        string secondtRequiredItem = currentActiveQuest.info.secondRequirmentItem;
        int secondRequiredAmount = currentActiveQuest.info.secondRequirementAmount;

        if (secondtRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(secondtRequiredItem, secondRequiredAmount);
        }

    }

    private bool AreQuestRequirmentsCompleted()
    {
        // Checking the we have completed the requirement
        if (currentActiveQuest.isCitizen)
        {
            if(AssetsManager.Instance.totalNetEnergyPowerConsumptionForCurrentDay > PowerConsumptionAPIManager.Instance.totalMaximumNetPowerConsumptionForSpecificDay ||
                AssetsManager.Instance.totalNetWaterCapacityConsumptionForCurrentDay > PowerConsumptionAPIManager.Instance.totalMaximumNetPowerConsumptionForSpecificDay ||
                AssetsManager.Instance.totalNetFoodMassConsumptionForCurrentDay > PowerConsumptionAPIManager.Instance.totalMaximumNetPowerConsumptionForSpecificDay ||
                AssetsManager.Instance.currentCitizenHappiness < 2.5f)
            {
                return false;
            }
            else
            {
                return true;    
            }
        }
        else if (currentActiveQuest.isAgent) // Agent will activate in only Day time 
        {
            if (DayNightSystem.Instance.currentHour >= activeHourAgent && DayNightSystem.Instance.currentHour <= deactiveHourAgent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (currentActiveQuest.isHealer) // For get medicine from healer we should have required coins
        {
            if (AssetsManager.Instance.currentCoins >= costForHealer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (currentActiveQuest.isChef) // For get rice from chef we should have required coins
        {
            if (AssetsManager.Instance.currentCoins >= costForChef)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    // Start the first dialog of current Quest
    private void StartQuestInitialDialog()
    {
        DialogSystem.Instance.OpenDialogUI();

        npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
        optionButton1Text.text = "Next";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            currentDialog++;
            CheckIfDialogDone();
        });

        optionButton2.gameObject.SetActive(false);
    }

    // If cuurent dilog is done then next go to the next dialog
    private void CheckIfDialogDone()
    {
        if (currentDialog == currentActiveQuest.info.initialDialog.Count - 1) // If its the last dialog 
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];

            currentActiveQuest.initialDialogCompleted = true;

            if (AreQuestRequirmentsCompleted())
            {
                FinishQuestAfterCompletedRequirements();
            }
            else
            {
                SetAcceptAndDeclineOptions();
            }
        }
        else  // If there are more dialogs
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];

            optionButton1Text.text = "Next";
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() => {
                currentDialog++;
                CheckIfDialogDone();
            });
        }
    }

    private void AcceptedQuest()
    {
        currentActiveQuest.accepted = true;
        currentActiveQuest.declined = false;

        if (currentActiveQuest.hasNoRequirements)
        {
            npcDialogText.text = currentActiveQuest.info.comebackCompleted;
            optionButton1Text.text = "[Take Reward]";
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() => {
                ReceiveRewardAndCompleteQuest();
            });
            optionButton2.gameObject.SetActive(false);
        }
        else
        {
            npcDialogText.text = currentActiveQuest.info.acceptAnswer;
            CloseDialogUI();
        }

    }

    private void CloseDialogUI()
    {
        optionButton1Text.text = "[Close]";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        });
        optionButton2.gameObject.SetActive(false);
    }

    private void ReceiveRewardAndCompleteQuest()
    {
        if (currentActiveQuest.isOthers)
        {
            activeQuestIndex++;
            currentActiveQuest.isCompleted = true;
        }
        else
        {
            firstTimeInteraction = true;
        }

        if (currentActiveQuest.info.rewardItem1 != "")
        {
            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem1);
        }

        if (currentActiveQuest.info.rewardItem2 != "")
        {
            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem2);
        }

        // Start Next Quest 
        if (activeQuestIndex < quests.Count)
        {
            currentActiveQuest = quests[activeQuestIndex];
            currentDialog = 0;
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;

            if (currentActiveQuest.isAgent)
            {
                BuyingSystem.Instance.openMarketPlace = true;
            }

        }
        else // No more quest available for this character
        {
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        }

    }

    private void DeclinedQuest()
    {
        currentActiveQuest.declined = true;

        npcDialogText.text = currentActiveQuest.info.declineAnswer;
        CloseDialogUI();
    }

    private void FinishQuestAfterCompletedRequirements()
    {
        DialogSystem.Instance.OpenDialogUI();

        npcDialogText.text = currentActiveQuest.info.comebackCompleted;

        if (currentActiveQuest.isAgent)
        {
            optionButton1Text.text = "[Ok Lets do some Trades]";
        }
        else if (currentActiveQuest.isHealer)
        {
            optionButton1Text.text = "[Ok Heal Me]";
        }
        else if (currentActiveQuest.isChef)
        {
            optionButton1Text.text = "[Ok Give Some Foods]";
        }
        else
        {
            optionButton1Text.text = "[Ok See You Later]";
        }
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            ReceiveRewardAndCompleteQuest();
        });
    }

    private void FinishTheQuest()
    {
        DialogSystem.Instance.OpenDialogUI();

        npcDialogText.text = currentActiveQuest.info.finalWords;

        optionButton1Text.text = "[Close]";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        });
    }

    // When we interact with the NPC this character will see at the player
    public void LookAtPlayer()
    {
        var player = PlayerState.Instance.playerBody.transform;
        Vector3 direction = player.position - transform.position;   
        transform.rotation = Quaternion.LookRotation(direction);


        var yRotation = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}
