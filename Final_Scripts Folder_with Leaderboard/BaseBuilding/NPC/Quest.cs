using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]   
public class Quest
{
    [Header("Bools")]
    public bool accepted;
    public bool declined;
    public bool initialDialogCompleted;
    public bool isCompleted;

    public bool hasNoRequirements;

    [Header("Type Of NPC")]
    public bool isCitizen;
    public bool isAgent;
    public bool isHealer;
    public bool isChef;
    public bool isOthers;

    [Header("Quest Info")]
    public QuestInfo info;
}
