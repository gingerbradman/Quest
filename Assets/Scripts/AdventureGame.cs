using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;
using System.Linq;

public class AdventureGame : MonoBehaviour
{
    //GameObject that holds the Title Image, the object is set to inactive after first scene.
    [SerializeField] GameObject titleGameObject;

    //Text box that displays choices and story
    [SerializeField] TMP_Text textComponent;

    //State where the game begins, can be changed for debugging
    [SerializeField] State startingState;
    //Holds a list of all possible next states and is also what is used for input directions.
    [SerializeField] List<State> nextStates;
    [SerializeField] List<State> visitedStates;
    
    [SerializeField] List<string> inventory;
    [SerializeField] List<string> knowledge;

    //A "Class" for the player character. Their class gives them different choices throughout the game.
    public CharacterRole.CharacterClass role;

    //A Companion the player can have with them on their adventure.
    public CharacterCompanion.Companion companion;

    //Background displayed behind Text
    public Image background;
    //Temp Background used when transitioning between scenes
    public Image nextBackground;
    //Transition time for transition speed, lower the float value the faster the transition
    public float transitionTime = 5.0f;

    //Temp color used for resetting alpha of backgrounds
    private Color tempColor;
    //Bool that keeps the player from making a choice before the transitions are complete.
    private bool transitionComplete = true;

    //State is not a state machine and is the name of the current place we are in, in the story.
    State state;
    private int optionIndex;

    //Counter to keep track of scenes visited as well as shut off title
    private int totalVisitedScenes;
    //Score that can be implemented for the player
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        //Role is preset to Default, aka none.
        role = CharacterRole.CharacterClass.Default;
        //Companion is preset to Default, aka none.
        companion = CharacterCompanion.Companion.Default;
        //Starting state is assigned as the current state
        state = startingState;
        //Get the next states for the choices
        nextStates = ConstructState();
        //Takes the story from the current state + whatever role and put them in the text
        textComponent.text = ConstructText();
        //Initialize Scene variables
        totalVisitedScenes = 0;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ManageState();
    }

    private void ManageState()
    {
        if(transitionComplete == false) {return;}

        for (int i = 0; i < nextStates.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1+i))
            {
                RolePick(i);
                AcquireCheck();
                state = nextStates[i];
                nextStates = ConstructState();
                textComponent.text = ConstructText();
                if(totalVisitedScenes == 0)
                {
                    ShutOffTitle();
                }
                totalVisitedScenes += 1;
                if (state.artwork != null)
                {
                    nextBackground.sprite = state.artwork;
                    StartCoroutine(TransitionSceneArtwork(background, nextBackground));
                }
            }
        }
    }

    private string ConstructText()
    {
        string text = "";
        optionIndex = nextStates.Count - 1;
        text += state.GetStateStory();

        if(nextStates.Contains(state.GetOption1State())){text += "\n" + OptionNumber() + state.GetOption1Text();}
        if(nextStates.Contains(state.GetOption2State())){text += "\n" + OptionNumber() + state.GetOption2Text();}
        if(nextStates.Contains(state.GetOption3State())){text += "\n" + OptionNumber() + state.GetOption3Text();}
        if(nextStates.Contains(state.GetOption4State())){text += "\n" + OptionNumber() + state.GetOption4Text();}
        if(nextStates.Contains(state.GetOption5State())){text += "\n" + OptionNumber() + state.GetOption5Text();}

        if(state.GetRoleStory(role) != ""){text += "\n" + OptionNumber() + state.GetRoleStory(role);}

        if(state.GetCompStory(companion) != ""){text += "\n" + OptionNumber() + state.GetCompStory(companion);}

        if(state.GetItemStory(inventory) != ""){text += "\n" + OptionNumber() + state.GetItemStory(inventory);}

        if(state.GetKnowledgeStory(knowledge) != ""){text += "\n" + OptionNumber() + state.GetKnowledgeStory(knowledge);}

        return text;
    }

    private string OptionNumber()
    {
        int maxOption = nextStates.Count;
        string optionNumber = (maxOption - optionIndex).ToString() + ". ";
        optionIndex --;
        return optionNumber;
    }

    private List<State> GetOptionsStates()
    {
        List<State> states = new List<State>();

        if(state.GetOption1Text() != "")
        {
            if(state.GetOption1State().oneVisit == false){
                states.Add(state.GetOption1State());
            }else if(state.GetOption1State().oneVisit == true && visitedStates.Contains(state.GetOption1State()) == false)
            {
                states.Add(state.GetOption1State());
                visitedStates.Add(state.GetOption1State());
            }else
            {
                //Do nothing
            }
        }

        if(state.GetOption2Text() != "")
        {
            if(state.GetOption2State().oneVisit == false){
                states.Add(state.GetOption2State());
            }else if(state.GetOption2State().oneVisit == true && visitedStates.Contains(state.GetOption2State()) == false)
            {
                states.Add(state.GetOption2State());
                visitedStates.Add(state.GetOption2State());
            }else
            {
                //Do nothing
            }
        }

        if(state.GetOption3Text() != "")
        {
            if(state.GetOption3State().oneVisit == false){
                states.Add(state.GetOption3State());
            }else if(state.GetOption3State().oneVisit == true && visitedStates.Contains(state.GetOption3State()) == false)
            {
                states.Add(state.GetOption3State());
                visitedStates.Add(state.GetOption3State());
            }else
            {
                //Do nothing
            }
        }

        if(state.GetOption4Text() != "")
        {
            if(state.GetOption4State().oneVisit == false){
                states.Add(state.GetOption4State());
            }else if(state.GetOption4State().oneVisit == true && visitedStates.Contains(state.GetOption4State()) == false)
            {
                states.Add(state.GetOption4State());
                visitedStates.Add(state.GetOption4State());
            }else
            {
                //Do nothing
            }
        }

        if(state.GetOption5Text() != "")
        {
            if(state.GetOption5State().oneVisit == false){
                states.Add(state.GetOption5State());
            }else if(state.GetOption5State().oneVisit == true && visitedStates.Contains(state.GetOption5State()) == false)
            {
                states.Add(state.GetOption5State());
                visitedStates.Add(state.GetOption5State());
            }else
            {
                //Do nothing
            }
        }

        return states;
    }

    private List<State> ConstructState()
    {
        List<State> states = new List<State>();

        states.AddRange(GetOptionsStates());
        states.AddRange(state.GetRoleState(role));
        states.AddRange(state.GetCompState(companion));
        states.AddRange(state.GetItemState(inventory));
        states.AddRange(state.GetKnowledgeState(knowledge));

        return states;
    }

    private void RolePick(int i)
    {
        if (state.ClassPick == false) {return;}

        if (i == 0)
        {
            role = CharacterRole.CharacterClass.Fighter;
        }
        else if (i == 1)
        {
            role = CharacterRole.CharacterClass.Wizard;
        }
        else if (i == 2)
        {
            role = CharacterRole.CharacterClass.Bard;
        }
    }

    private void AcquireCheck()
    {
        if(state.acquireCompanion != CharacterCompanion.Companion.Default)
        {
            companion = state.acquireCompanion;
        }

        if(state.itemAcquire != "")
        {
            inventory.Add(state.itemAcquire);
        }

        if(state.knowledgeAcquire != "")
        {
            knowledge.Add(state.knowledgeAcquire);
        }
    }

    private void ShutOffTitle()
    {
        titleGameObject.SetActive(false);
    }

    private YieldInstruction fadeInstruction = new YieldInstruction();
    IEnumerator TransitionSceneArtwork(Image currentBackground, Image nextBackground)
    {
        transitionComplete = false;
        float elapsedTime = 0.0f;
        Color c = currentBackground.color;
        while(elapsedTime < transitionTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / transitionTime);
            currentBackground.color = c;
        }

        elapsedTime = 0.0f;
        c = nextBackground.color;
        while(elapsedTime < transitionTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / transitionTime);
            nextBackground.color = c;
        }

        background.sprite = state.artwork;
        QuickAlphaChange(background, 1f);
        QuickAlphaChange(nextBackground, 0f);
        transitionComplete = true;
    }

    private void QuickAlphaChange(Image image, float alpha)
    {
        tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;           
    }
}
