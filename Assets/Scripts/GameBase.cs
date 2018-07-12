using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class works as a Game Base with elements commons among other classes.
/// </summary>
public class GameBase : MonoBehaviour {

    /// <summary>
    /// Games Modes possible in game.
    /// </summary>
    public enum Modes {Intro, Guessing, Answering, AskingAnimal, AskingAction, Repeated, Win, Lose}

    // Constants to be used as indexes
    public const int No = 0;
    public const int Yes = 1;
    public const int Ok = 2;
}
