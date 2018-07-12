using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an answer (an animal), that can be also a question (an action) for more answers.
/// </summary>
public class Answer {

    // Constants to be used in answers
    public const int No = 0;
    public const int Yes = 1;

    private string keyword;

    /// <summary>
    /// Keyword can be an answer or an action
    /// </summary>
    /// <value>The keyword.</value>
    public string Keyword
    {
        get 
        { 
            return keyword;
        }
    }

    /// <summary>
    ///  Is the "parent" question
    /// </summary>
    public Answer Question;
    /// <summary>
    /// The "parent" answer of the "parent" question
    /// </summary>
    public int ParentAnswer;
    /// <summary>
    /// Is the "children" answers
    /// </summary>
    public Answer[] Answers;

    /// <summary>
    /// Initializes a new instance of the <see cref="Answer"/> class with only a keyword (usually a pure answer)
    /// </summary>
    /// <param name="answer">Answer.</param>
    public Answer(string answer) 
    {
        SetKeyword(answer);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Answer"/> class with two answer (usually a question and two pure answer)
    /// </summary>
    /// <param name="action">Action for the question.</param>
    /// <param name="does">Answer for yes, does.</param>
    /// <param name="doesnot">Answer for no, doesn't.</param>
    public Answer (string action, string does, string doesnot) 
    {
        SetKeyword(action);

        //Obs: Question keeps null
        Answers = new Answer[2];

        Answers[Yes] = new Answer(does);
        Answers[No] = new Answer(doesnot);

        Answers[Yes].SetQuestion(this, Yes);
        Answers[No].SetQuestion(this, No);
    }

    /// <summary>
    /// Adds a new question in the answers tree.
    /// </summary>
    /// <param name="action">Action for the question.</param>
    /// <param name="answer">Answer for yes, does.</param>
    public void AddAnswer(string action, string answer)
    {
        Answer aux = new Answer(action);
        aux.SetQuestion(this.Question, this.ParentAnswer);

        aux.Answers = new Answer[2];
        aux.Answers[Yes] = new Answer(answer);
        aux.Answers[No] = this;

        aux.Answers[Yes].SetQuestion(aux, Yes);
        aux.Answers[No].SetQuestion(aux, No);

        aux.Question.Answers[aux.ParentAnswer] = aux;
    }

    /// <summary>
    /// Sets the keyword.
    /// </summary>
    /// <param name="keyword">Keyword.</param>
    public void SetKeyword(string keyword)
    {
        this.keyword = keyword;
    }

    /// <summary>
    /// Sets the "parent" question and answer.
    /// </summary>
    /// <param name="question">Parent question.</param>
    /// <param name="answer">Parent answer.</param>
    public void SetQuestion(Answer question, int answer)
    {
        this.Question = question;
        ParentAnswer = answer;
    }

    /// <summary>
    /// Tracks the questions for an answer.
    /// </summary>
    /// <returns>The questions list in order.</returns>
    public List<Answer> TrackQuestions()
    {
        List<Answer> track = new List<Answer>();
        Answer auxAns = this.Question;
        while (auxAns.Question != null)
        {
            track.Add(auxAns.Question);
            auxAns = auxAns.Question;
        }
        track.Reverse();
        return track;
    }
}
