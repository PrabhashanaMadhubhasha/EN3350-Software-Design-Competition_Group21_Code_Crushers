using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    private Transform saveSlots;
    private Transform slot_1;
    private Transform slot_2;
    private Transform slot_3;
    List<Transform> slotsList = new List<Transform>();
    Dictionary<Transform, int> rewardLevels = new Dictionary<Transform, int>();

    private int rewardLevelDivider = 100;
    private int baseScore = 1000;
    List<int> rewardCoins = new List<int> { 0, 0, 0 };

    private void Awake()
    {
        saveSlots = transform.GetChild(0).GetChild(0).Find("SaveSlots");
        slot_1 = saveSlots.Find("Slot1");
        slot_2 = saveSlots.Find("Slot2");
        slot_3 = saveSlots.Find("Slot3");

        slotsList.Add(slot_1);
        slotsList.Add(slot_2);
        slotsList.Add(slot_3);        

        foreach (Transform slot in slotsList)
        {
            rewardLevels.Add(slot, 0);
        }

        for (int i = 0; i < slotsList.Count; i++)
        {
            SetSlot(i + 1);
        }
    }

    // Set the score related data to corresponding save slot
    private void SetSlot(int slotNumber)
    {
        Transform slot = slotsList[slotNumber - 1];
        string score = PlayerPrefs.GetString("slot_" + slotNumber.ToString() + "_playerScore", "Not Saved");

        if(score != null)
        {
            Debug.Log("score "+slotNumber +"= "+  score);
            slot.Find("scoreText").GetComponent<Text>().text = score;

            try
            {
                CalculateReward(slotNumber, int.Parse(score));
            }
            catch (System.FormatException)
            {
                Debug.LogError("Failed to convert player score to an integer. Invalid format" );
            }         
        }     
    }

    // Calculate the reward based on player score in corresponding saved game
    private void CalculateReward(int slotNumber, int score)
    {
        Transform slot = slotsList[slotNumber - 1];
        string rewardLevel = PlayerPrefs.GetString("slot_" + slotNumber.ToString() + "_rewardLevel", "No rewardLevel found");

        try
        {
            int endRewardLevel = score / rewardLevelDivider;
            rewardLevels[slot] = endRewardLevel;

            for (int i = int.Parse(rewardLevel); i < endRewardLevel; i++)
            {
                Debug.Log("CalculateReward " + endRewardLevel);
                rewardCoins[slotNumber - 1] += (i + 1) * baseScore;
            }
        }
        catch (System.FormatException)
        {
            Debug.LogError("Failed to convert player reward level to an integer. Invalid format");
        }

        slot.Find("rewardCoinsText").GetComponent<Text>().text = rewardCoins[slotNumber - 1].ToString();
    }

    // Collect and Add reward coins to reward coins account
    public void CollectReward(int slotNumber)
    {
        Transform slot = slotsList[slotNumber - 1];
        PlayerPrefs.SetString("slot_" + slotNumber.ToString() + "_rewardCoins", rewardCoins[slotNumber - 1].ToString());
        PlayerPrefs.SetString("slot_" + slotNumber.ToString() + "_rewardLevel", rewardLevels[slot].ToString());

        rewardCoins[slotNumber - 1] = 0;
        slot.Find("rewardCoinsText").GetComponent<Text>().text = rewardCoins[slotNumber - 1].ToString();
    }
}
