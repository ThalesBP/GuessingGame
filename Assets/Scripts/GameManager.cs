using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the game
/// </summary>
public class GameManager : GameBase {

    /// <summary>
    /// Current game mode.
    /// </summary>
    public Modes mode;

    /// <summary>
    /// Register the top answer of the tree.
    /// </summary>
    public Answer headAnswer;

    /// <summary>
    /// The current answer along the game.
    /// </summary>
    public Answer currentAnswer;

    /// <summary>
    /// The new animal the user is inputing.
    /// </summary>
    public string newAnimal;

    /// <summary>
    /// Lists all animals for checking repeatance.
    /// </summary>
    public List<Answer> allAnimals;

    /// <summary>
    /// Tracks the player's answers.
    /// </summary>
    public List<Answer> answerTrack;

    /// <summary>
    /// Tracks the answer's questions.
    /// </summary>
    public List<Answer> questionTrack;

    /// <summary>
    /// The current interface manager.
    /// </summary>
    public InterfaceManager interfaceManager;
    // Use this for initialization
	void Start () 
    {
        mode = Modes.Intro; // It begins with Intro Mode
        headAnswer = currentAnswer = new Answer("live in water", "shark", "monkey"); // Initialize the first question

        // Initialize the lists
        allAnimals = new List<Answer>();
        answerTrack = new List<Answer>();
        questionTrack = new List<Answer>();

        // Add the current answers to all animals
        allAnimals.Add(currentAnswer.Answers[No]);
        allAnimals.Add(currentAnswer.Answers[Yes]);

        // Finds current interface manager and initialize add the listeners at buttons
        interfaceManager = GameObject.Find("Canvas").GetComponentInChildren<InterfaceManager>(true);
        interfaceManager.yesButton.onClick.AddListener(delegate
            {
                AnswerYes();
            });
        interfaceManager.noButton.onClick.AddListener(delegate
            {
                AnswerNo();
            });
        interfaceManager.okButton.onClick.AddListener(delegate
            {
                AnswerOk();
            });
	}
	
	// Update is called once per frame
	void Update () 
    {
        interfaceManager.SetMode(mode);

        if (interfaceManager.okButton.IsActive())
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                AnswerOk();
            }

        if (interfaceManager.yesButton.IsActive() && interfaceManager.noButton.IsActive())
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                AnswerYes();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                AnswerNo();
            }
        }

        switch (mode)
        {
            case Modes.Intro:
                // When te game restarts the current answer receives the head of tree and the lists are cleaned
                currentAnswer = headAnswer; 
                answerTrack.Clear();
                questionTrack.Clear();
                interfaceManager.SetMainText("Think about an animal...");
                break;

            case Modes.Guessing:
                // Guess the animal throught an action
                interfaceManager.SetMainText("Does this animal " + currentAnswer.Keyword);
                break;

            case Modes.Answering:
                // When there is no more questions, guess an animal
                interfaceManager.SetMainText("You thought about " + currentAnswer.Keyword);
                break;

            case Modes.AskingAnimal:
                // Case it is not the animal the player was thinking, it asks the one
                interfaceManager.SetMainText("What was the animal you thought about?");
                break;

            case Modes.AskingAction:
                // Then asks an action that differs from the other animal
                interfaceManager.SetMainText("The " + newAnimal + " can _______, but " + currentAnswer.Keyword + " cannot. (Fill with an action that this animal can do, like \"dig\")");
                break;

            case Modes.Repeated:
                // Case this animal already exists, show which question should be answered different
                if (currentAnswer.ParentAnswer == Yes)
                    interfaceManager.SetMainText("But " + newAnimal + " can't " + currentAnswer.Question.Keyword);
                else
                    interfaceManager.SetMainText("But " + newAnimal + " can " + currentAnswer.Question.Keyword);
                break;

            case Modes.Lose:
                // Player wins
                interfaceManager.SetMainText("Ok, you won. Play again?");
                break;

            case Modes.Win:
                // Game wins
                interfaceManager.SetMainText("I win! Play again?");
                break;
        }
	}

    void AnswerYes()
    {
        switch (mode)
        {
            case Modes.Guessing:
                // Case is guessing, jump for next YES answer
                answerTrack.Add(currentAnswer);
                currentAnswer = currentAnswer.Answers[Answer.Yes];
                if (currentAnswer.Answers == null)
                    mode = Modes.Answering;
                break;

            case Modes.Answering:
                // Case player answers YES, game wins
                mode = Modes.Win;
                break;

            case Modes.Lose:
                // Case players wants to play again, go to Intro;
                mode = Modes.Intro;
                break;

            case Modes.Win:
                // Case players wants to play again, go to Intro;
                mode = Modes.Intro;
                break;
        }
    }

    void AnswerNo()
    {
        switch (mode)
        {
            case Modes.Guessing:
                // Case is guessing, jump for next NO answer
                answerTrack.Add(currentAnswer);
                currentAnswer = currentAnswer.Answers[Answer.No];
                if (currentAnswer.Answers == null)
                    mode = Modes.Answering;
                break;

            case Modes.Answering:
                // Case player answers NO, ask the correct animal
                mode = Modes.AskingAnimal;
                break;

            case Modes.Lose:
                // Case players doesn't want to play again, quit the game;
                Application.Quit();
                break;

            case Modes.Win:
                // Case players doesn't want to play again, quit the game;
                Application.Quit();
                break;
        }
    }

    void AnswerOk()
    {
        switch (mode)
        {
            case Modes.Intro:
                // After player thinks about an animal, go to guessing
                mode = Modes.Guessing;
                break;

            case Modes.AskingAnimal:
                // Ask which was the animal the player thought
                if (interfaceManager.inputAnswer.text == "")
                {
                    interfaceManager.inputAnswer.placeholder.color = new Color(1f, 0f, 0f, 0.5f);
                }
                else
                {
                    newAnimal = interfaceManager.inputAnswer.text;
                    interfaceManager.inputAnswer.text = "";

                    // Ask the action if the match verification is ok
                    mode = Modes.AskingAction;

                    // Verifies if this answer matches with anoter answer in all animals
                    foreach (Answer answer in allAnimals)
                    {
                        // If it matches...
                        if (answer.Keyword == newAnimal)
                        {
                            // Takes the smallest index
                            int count = (answerTrack.Count > questionTrack.Count ? answerTrack.Count : questionTrack.Count);

                            // Tracks the question sequence for this answer that already exists
                            questionTrack = answer.TrackQuestions();

                            // Walks 
                            for (int ans = 0; ans < count; ans++)
                            {
//                            Debug.Log(answerTrack[ans].Keyword + " = " + questionTrack[ans].Keyword + " ?");
                                if (answerTrack[ans].Keyword != questionTrack[ans].Keyword)
                                {
                                    currentAnswer = answerTrack[ans];
                                    break;
                                }
                            }
                            mode = Modes.Repeated;
                            break;
                        }
                    }
                }
                break;

            case Modes.AskingAction:
                if (interfaceManager.inputAnswer.text == "")
                {
                    interfaceManager.inputAnswer.placeholder.color = new Color(1f, 0f, 0f, 0.5f);
                }
                else
                {
                    currentAnswer.AddAnswer(interfaceManager.inputAnswer.text, newAnimal);
                    allAnimals.Add(currentAnswer.Question.Answers[Yes]);

//                foreach (Answer answer in allAnimals)
//                {
//                    Debug.Log(answer.Keyword);
//                }
//                foreach (Answer answer in answerTrack)
//                {
//                    Debug.Log(answer.Keyword);
//                }
//
                    interfaceManager.inputAnswer.text = "";
                    mode = Modes.Lose;
                }
                break;

            case Modes.Repeated:
                mode = Modes.Lose;
                break;
        }
    }
}
