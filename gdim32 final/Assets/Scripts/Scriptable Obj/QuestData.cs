using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum QuestState
{
    NotStarted,
    Active,
    Completed,
    Failed
}

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/QuestData")]
public class QuestData : ScriptableObject
{
    public string questName;
    [TextArea] public string description;
    public QuestState state;

    public int currentAmount;
    public int requiredAmount;

    public void ResetQuest()
    {
        state = QuestState.NotStarted;
        currentAmount = 0;
    }
}
