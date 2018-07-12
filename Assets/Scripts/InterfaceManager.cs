using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the game interface
/// </summary>
public class InterfaceManager : GameBase {

    public Text mainText;
    public InputField inputAnswer;
    public Button yesButton;
    public Button noButton;
    public Button okButton;

    public string MainText
    {
        get
        {
            return mainText.text;
        }
    }

	void Awake () 
    {
        // Finding the objects in the scene
        mainText = GameObject.Find("MainText").GetComponentInChildren<Text>(true);
        inputAnswer = GameObject.Find("InputAnswer").GetComponentInChildren<InputField>(true);
        yesButton = GameObject.Find("YesButton").GetComponentInChildren<Button>(true);
        noButton = GameObject.Find("NoButton").GetComponentInChildren<Button>(true);
        okButton = GameObject.Find("OkButton").GetComponentInChildren<Button>(true);
    }
        	
    /// <summary>
    /// Sets the main text.
    /// </summary>
    /// <param name="text">New text.</param>
    public void SetMainText(string text)
    {
        mainText.text = text;
    }

    /// <summary>
    /// Sets the interface according the mode.
    /// </summary>
    /// <param name="mode">Mode.</param>
    public void SetMode(Modes mode)
    {
        switch (mode)
        {
            case Modes.Intro:
            case Modes.Repeated:
                SetOkMode();
                break;

            case Modes.AskingAnimal:
            case Modes.AskingAction:
                SetInputMode();
                break;

            case Modes.Guessing:
            case Modes.Answering:
            case Modes.Lose:
            case Modes.Win:
                SetYesNoMode();
                break;
        }
    }

    /// <summary>
    /// Shows only ok button
    /// </summary>
    private void SetOkMode()
    {        
        inputAnswer.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        okButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Shows yes and no buttons
    /// </summary>
    private void SetYesNoMode()
    {
        inputAnswer.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
        okButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows input field and ok button
    /// </summary>
    private void SetInputMode()
    {
        inputAnswer.gameObject.SetActive(true);
        inputAnswer.ActivateInputField();
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        okButton.gameObject.SetActive(true);
    }
}
