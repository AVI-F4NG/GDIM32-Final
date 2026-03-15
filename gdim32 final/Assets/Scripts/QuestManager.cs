using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public List<QuestData> allQuests;

    public static event Action<QuestData> OnQuestUpdated;

    void Awake() => Instance = this;

    public void StartQuest(string name)
    {
        QuestData quest = allQuests.Find(q => q.questName == name);
        if (quest != null && quest.state == QuestState.NotStarted)
        {
            quest.state = QuestState.Active;
            OnQuestUpdated?.Invoke(quest);
        }
    }

    public void AdvanceQuest(string name, int amount)
    {
        QuestData quest = allQuests.Find(q => q.questName == name);
        if (quest != null && quest.state == QuestState.Active)
        {
            quest.currentAmount += amount;

            if (quest.currentAmount >= quest.requiredAmount)
            {
                CompleteQuest(name);
            }
            else
            {
                OnQuestUpdated?.Invoke(quest); // Quest "Changed" but not finished
            }
        }
    }

    public void CompleteQuest(string name)
    {
        QuestData quest = allQuests.Find(q => q.questName == name);
        if (quest != null)
        {
            quest.state = QuestState.Completed;
            OnQuestUpdated?.Invoke(quest);
        }
    }
}
