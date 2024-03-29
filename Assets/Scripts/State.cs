﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State")]
public class State : ScriptableObject
{
    [TextArea(14,10)][SerializeField] string defaultText;

    public bool oneVisit;
    public bool splitPath;
    public Sprite artwork;
    public AudioClip audioClip;
    public string locationName;
    public string itemAcquire;
    public string itemRequired;
    public string knowledgeAcquire;
    public string knowledgeRequired;
    public string splitPathKeywordAcquire;
    public string splitPathKeywordRecquired;
    public bool ClassPick;

    public CharacterCompanion.Companion acquireCompanion = CharacterCompanion.Companion.Default;
    public CharacterCompanion.LoseCompanion loseCompanion = CharacterCompanion.LoseCompanion.Default;
    [SerializeField] State splitState;
    
    [TextArea(2, 10)] [SerializeField] string option1Text;
    [SerializeField] State option1State;
    [TextArea(2, 10)] [SerializeField] string option2Text;
    [SerializeField] State option2State;
    [TextArea(2, 10)] [SerializeField] string option3Text;
    [SerializeField] State option3State;
    [TextArea(2, 10)] [SerializeField] string option4Text;
    [SerializeField] State option4State;
    [TextArea(2, 10)] [SerializeField] string option5Text;
    [SerializeField] State option5State;
    [TextArea(2, 10)] [SerializeField] string fighterText;
    [SerializeField] State fighterStates;
    [TextArea(2, 10)] [SerializeField] string wizardText;
    [SerializeField] State wizardStates;
    [TextArea(2, 10)] [SerializeField] string bardText;
    [SerializeField] State bardStates;

    [TextArea(2, 10)] [SerializeField] string hansText;
    [SerializeField] State hansStates;
    [TextArea(2, 10)] [SerializeField] string nyxText;
    [SerializeField] State nyxStates;
    [TextArea(2, 10)] [SerializeField] string timText;
    [SerializeField] State timStates;
    [TextArea(2, 10)] [SerializeField] string itemText;
    [SerializeField] State itemStates;

    [TextArea(2, 10)] [SerializeField] string knowledgeText;
    [SerializeField] State knowledgeStates;

    public State GetSplitState()
    {
        return splitState;
    }

    public string GetStateStory()
    {
        return defaultText;
    }

    public string GetOption1Text(){ return option1Text;}
    public string GetOption2Text(){ return option2Text;}
    public string GetOption3Text(){ return option3Text;}
    public string GetOption4Text(){ return option4Text;}
    public string GetOption5Text(){ return option5Text;}

    public State GetOption1State()
    {
        return option1State;
    }

    public State GetOption2State()
    {
        return option2State;
    }

    public State GetOption3State()
    {
        return option3State;
    }

    public State GetOption4State()
    {
        return option4State;
    }

    public State GetOption5State()
    {
        return option5State;
    }

    public string GetRoleStory(CharacterRole.CharacterClass role)
    {
        if(role == CharacterRole.CharacterClass.Default)
        {
            return "";
        }
        if (role == CharacterRole.CharacterClass.Fighter && GetFighterText() != "")
        {
            return "(Fighter) " + GetFighterText();
        }
        else if (role == CharacterRole.CharacterClass.Wizard && GetWizardText() != "")
        {
            return "(Wizard) " +  GetWizardText();
        }
        else if (role == CharacterRole.CharacterClass.Bard && GetBardText() != "")
        {
            return "(Bard) " +  GetBardText();
        }
        return "";
    }

    public State GetRoleState(CharacterRole.CharacterClass role)
    {
        State temp = new State();
        if (role == CharacterRole.CharacterClass.Fighter)
        {
            temp = GetFighterStates();
        }
        else if (role == CharacterRole.CharacterClass.Wizard)
        {
            temp = GetWizardStates();
        }
        else if (role == CharacterRole.CharacterClass.Bard)
        {
            temp = GetBardStates();
        }
        return temp;
    }

    public string GetCompStory(CharacterCompanion.Companion companion)
    {
        if(companion == CharacterCompanion.Companion.Default)
        {
            return "";
        }
        else if (companion == CharacterCompanion.Companion.Hans && GetHansText() != "")
        {
            return "(Hans) " + GetHansText();
        }
        else if (companion == CharacterCompanion.Companion.Nyx && GetNyxText() != "")
        {
            return "(Nyx) " + GetNyxText();
        }
        else if (companion == CharacterCompanion.Companion.Tim && GetTimText() != "")
        {
            return "(Tim) " + GetTimText();
        }
        return "";
    }

    public State GetCompState(CharacterCompanion.Companion companion)
    {
        State temp = new State();
        if (companion == CharacterCompanion.Companion.Default)
        {
            return temp;
        }
        else if (companion == CharacterCompanion.Companion.Hans)
        {
            temp = GetHansStates();
        }
        else if (companion == CharacterCompanion.Companion.Nyx)
        {
            temp = GetNyxStates();
        }
        else if (companion == CharacterCompanion.Companion.Tim)
        {
            temp = GetTimStates();
        }
        return temp;
    }

    public string GetItemStory(List<string> inventory)
    {
        if(inventory.Contains(itemRequired))
        {
            return GetItemText();
        }
        return "";
    }

    public State GetItemState(List<string> inventory)
    {
        State temp = new State();
        if (inventory.Contains(itemRequired))
        {
            temp = GetItemStates();
        }
        return temp;
    }
    
    public string GetKnowledgeStory(List<string> knowledge)
    {
        if(knowledge.Contains(knowledgeRequired))
        {
            return GetKnowledgeText();
        }
        return "";
    }

    public State GetKnowledgeState(List<string> knowledge)
    {
        State temp = new State();
        if (knowledge.Contains(knowledgeRequired))
        {
            temp = GetKnowledgeStates();
        }
        return temp;
    }

    public string GetFighterText()
    {
        return fighterText;
    }

    public string GetWizardText()
    {
        return wizardText;
    }

    public string GetBardText()
    {
        return bardText;
    }

    public State GetFighterStates()
    {
        return fighterStates;
    }

    public State GetWizardStates()
    {
        return wizardStates;
    }

    public State GetBardStates()
    {
        return bardStates;
    }

    public string GetHansText()
    {
        return hansText;
    }

    public string GetNyxText()
    {
        return nyxText;
    }

    public string GetTimText()
    {
        return timText;
    }

    public State GetHansStates()
    {
        return hansStates;
    }

    public State GetNyxStates()
    {
        return nyxStates;
    }

    public State GetTimStates()
    {
        return timStates;
    }

    public string GetItemText()
    {
        return itemText;
    }

    public State GetItemStates()
    {
        return itemStates;
    }

    public string GetKnowledgeText()
    {
        return knowledgeText;
    }

    public State GetKnowledgeStates()
    {
        return knowledgeStates;
    }

}
