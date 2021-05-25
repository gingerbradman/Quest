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
    [SerializeField] private float writeSpeed = 0.001f;
    [SerializeField] TextWriter textWriter;
    //GameObject that holds the Title Image, the object is set to inactive after first scene.
    [SerializeField] GameObject titleGameObject;

    //Text box that displays choices and story
    [SerializeField] TMP_Text textComponent;

    //GameObject parent that holds the location text, will appear after first scene.
    [SerializeField] GameObject locationTextGameObject;
    //Text box that displays location in top left
    [SerializeField] TMP_Text locationText;

    //State where the game begins, can be changed for debugging
    [SerializeField] State startingState;
    //Holds a list of all possible next states and is also what is used for input directions.
    [SerializeField] List<State> nextStates;
    [SerializeField] List<State> visitedStates;
    
    [SerializeField] List<string> inventory;
    [SerializeField] List<string> knowledge;
    [SerializeField] List<string> keywords;

    //Music
    public AudioSource audioSource;

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
        textWriter.SetUpWriter(textComponent, ConstructText(), writeSpeed, true);
        //Background startup
        background.sprite = state.artwork;

        //Slowly transition
        StartCoroutine(TransitionStartArtwork(background));

        //Initialize Scene variables
        totalVisitedScenes = 0;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        ManageState();
    }

    private void ManageState()
    {
        if(state.oneVisit == true && visitedStates.Contains(state) == false) { visitedStates.Add(state); }

        for (int i = 0; i < nextStates.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1+i))
            {
                if(textWriter.getFinishedWriting() == false)
                {
                    textWriter.QuickFinishWrite();
                    return;
                }
                if(transitionComplete == false) {return;}
                RolePick(i);
                StatePick(i);
                nextStates = ConstructState();
                textWriter.SetUpWriter(textComponent, ConstructText(), writeSpeed, true);
                AcquireCheck();
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
                if (state.locationName != "")
                {
                    locationText.text = state.locationName;
                }
                if(state.audioClip != null)
                {
                    audioSource.clip = state.audioClip;
                    audioSource.Play();
                }
            }
        }
    }

    private void StatePick(int i)
    {
        State tempState = nextStates[i];
        if(tempState.splitPath == true)
        {
            if(keywords.Contains(tempState.splitPathKeywordRecquired))
            {
                state = tempState.GetSplitState();
            }
            else{
                state = tempState;
            }
        }
        else 
        {
            state = tempState;
        }
        return;
    }

    private string ConstructText()
    {
        string text = "";
        optionIndex = nextStates.Count - 1;
        text += state.GetStateStory() + "\n";

        if(nextStates.Contains(state.GetOption1State())){text += "\n" + OptionNumber() + state.GetOption1Text();}
        if(nextStates.Contains(state.GetOption2State())){text += "\n" + OptionNumber() + state.GetOption2Text();}
        if(nextStates.Contains(state.GetOption3State())){text += "\n" + OptionNumber() + state.GetOption3Text();}
        if(nextStates.Contains(state.GetOption4State())){text += "\n" + OptionNumber() + state.GetOption4Text();}
        if(nextStates.Contains(state.GetOption5State())){text += "\n" + OptionNumber() + state.GetOption5Text();}

        if(nextStates.Contains(state.GetRoleState(role))){text += "\n" + OptionNumber() + state.GetRoleStory(role);}

        if(nextStates.Contains(state.GetCompState(companion))){text += "\n" + OptionNumber() + state.GetCompStory(companion);}

        if(nextStates.Contains(state.GetItemState(inventory))){text += "\n" + OptionNumber() + state.GetItemStory(inventory);}

        if(nextStates.Contains(state.GetKnowledgeState(knowledge))){text += "\n" + OptionNumber() + state.GetKnowledgeStory(knowledge);}

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
        
        State tempState = state.GetOption1State();

        if(state.GetOption1Text() != "")
        {
            if(tempState.oneVisit == false){
                states.Add(tempState);
            }else if(tempState.oneVisit == true && visitedStates.Contains(tempState) == false)
            {
                states.Add(tempState);
            }else
            {
                //Do nothing
            }
        }

        tempState = state.GetOption2State();

        if(state.GetOption2Text() != "")
        {
            if(tempState.oneVisit == false){
                states.Add(tempState);
            }else if(tempState.oneVisit == true && visitedStates.Contains(tempState) == false)
            {
                states.Add(tempState);
            }else
            {
                //Do nothing
            }
        }

        tempState = state.GetOption3State();

        if(state.GetOption3Text() != "")
        {
            if(tempState.oneVisit == false){
                states.Add(tempState);
            }else if(tempState.oneVisit == true && visitedStates.Contains(tempState) == false)
            {
                states.Add(tempState);
            }else
            {
                //Do nothing
            }
        }

        tempState = state.GetOption4State();

        if(state.GetOption4Text() != "")
        {
            if(tempState.oneVisit == false){
                states.Add(tempState);
            }else if(tempState.oneVisit == true && visitedStates.Contains(tempState) == false)
            {
                states.Add(tempState);
            }else
            {
                //Do nothing
            }
        }

        tempState = state.GetOption5State();

        if(state.GetOption5Text() != "")
        {
            if(tempState.oneVisit == false){
                states.Add(tempState);
            }else if(tempState.oneVisit == true && visitedStates.Contains(tempState) == false)
            {
                states.Add(tempState);
            }else
            {
                //Do nothing
            }
        }

        return states;
    }

    private List<State> GetRoleStates()
    {
        List<State> states = new List<State>();

        State tempState = state.GetRoleState(role);

        if (state.GetRoleStory(role) != "")
        {
            if (tempState.oneVisit == false)
            {
                states.Add(tempState);
            }
            else if (tempState.oneVisit == true && visitedStates.Contains(tempState) == false)
            {
                states.Add(tempState);
            }
            else
            {
                //Do nothing
            }
        }

        return states;
    }

    private List<State> GetCompStates()
    {
        List<State> states = new List<State>();

        State tempState = state.GetCompState(companion);

        if(state.GetCompStory(companion) != "")
        {
            if (state.GetCompState(companion).oneVisit == false)
            {
                states.Add(tempState);
            }
            else if (state.GetCompState(companion).oneVisit == true && visitedStates.Contains(tempState) == false)
            {
                states.Add(tempState);
            }
            else
            {
                //Do nothing
            }
        }
        return states;
    }

    private List<State> GetItemStates()
    {
        List<State> states = new List<State>();

        State tempState = state.GetItemState(inventory);

        if(state.GetItemStory(inventory) != "")
        {
            if (tempState.oneVisit == false)
            {
                states.Add(tempState);
            }
            else if (tempState.oneVisit == true && visitedStates.Contains(tempState) == false)
            {
                states.Add(tempState);
            }
            else
            {
                //Do nothing
            }
        }
        return states;
    }

    private List<State> GetKnowledgeStates()
    {
        List<State> states = new List<State>();

        State tempState = state.GetKnowledgeState(knowledge);

        if(state.GetKnowledgeStory(knowledge) != "")
        {
            if (tempState.oneVisit == false)
            {
                states.Add(tempState);
            }
            else if (tempState.oneVisit == true && visitedStates.Contains(tempState) == false)
            {
                states.Add(tempState);
            }
            else
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
        states.AddRange(GetRoleStates());
        states.AddRange(GetCompStates());
        states.AddRange(GetItemStates());
        states.AddRange(GetKnowledgeStates());

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

        if(state.loseCompanion.ToString() == companion.ToString())
        {
            companion = CharacterCompanion.Companion.Default;
        }

        if(state.acquireCompanion != CharacterCompanion.Companion.Default)
        {
            companion = state.acquireCompanion;
        }

        if(state.itemAcquire != "" && inventory.Contains(state.itemAcquire) == false)
        {
            inventory.Add(state.itemAcquire);
        }

        if(state.knowledgeAcquire != "" && knowledge.Contains(state.knowledgeAcquire) == false)
        {
            knowledge.Add(state.knowledgeAcquire);
        }

        if(state.splitPathKeywordAcquire != "" && keywords.Contains(state.splitPathKeywordAcquire) == false)
        {
            keywords.Add(state.splitPathKeywordAcquire);
        }
    }

    private void ShutOffTitle()
    {
        titleGameObject.SetActive(false);
        locationTextGameObject.SetActive(true);
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

    private YieldInstruction fadeStartInstruction = new YieldInstruction();
    IEnumerator TransitionStartArtwork(Image background)
    {
        transitionComplete = false;

        float elapsedTime = 0.0f;
        Color c = background.color;
        while(elapsedTime < transitionTime)
        {
            yield return fadeStartInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / transitionTime);
            background.color = c;
        }

        background.sprite = state.artwork;
        transitionComplete = true;
    }

    private void QuickAlphaChange(Image image, float alpha)
    {
        tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;           
    }
}
