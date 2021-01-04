using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TextManager : MonoBehaviour
{
    #region Singleton
    public static TextManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Variables

    public Text TextUI;
    public GameObject tutorialWindow;
    private string _textToShow;
    private float timer = 2f;
    private float rageTimer = 5f;
    private float lastTutorialTimer = 3f;
    [SerializeField]
    private GameObject triggerCrate;
    [SerializeField]
    private GameObject triggerRagebar;
    [SerializeField]
    private GameObject triggerBridge;
    [SerializeField]
    private GameObject triggerFox;
    [SerializeField]
    private GameObject triggerDodge;
    [SerializeField]
    private GameObject triggerMushroom;
    [SerializeField]
    private GameObject triggerGoodbye;
    [SerializeField]
    private GameObject triggerLast;


    #endregion

    #region Methods
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            TextWriter.AddWriterStatic(TextUI, StartCutscene(), 0.05f, true);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {

            TriggerTutorial();
        }
    }

    private string StartCutscene()
    {
        _textToShow = "The rage inside of me.. it's unbearable. One year ago today, a peasant killed my family. I managed to escape but at what cost? .... " +
          "Im all alone, surrounded by the same trees day after day. " +
          "The foxes here are vicious .... I must escape this forest!";
        return _textToShow;
    }

    public void TriggerTutorial()
    {
        if (TextUI != null)
        {
            if (triggerCrate == null)
            {
                TutorialCrate(TextUI);
            }
            if (triggerRagebar == null)
            {
                TutorialRageBar(TextUI);
            }
            if (triggerBridge == null)
            {
                TutorialBridge(TextUI);
            }
            if (triggerFox == null)
            {
                TutorialFox(TextUI);
            }
            if (triggerDodge == null)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    TutorialDodge(TextUI);
                }
                rageTimer -= Time.deltaTime;
                if (rageTimer <= 0)
                {
                    TutorialActivateRage(TextUI);
                }
            }
            if (triggerMushroom == null)
            {
                TutorialMushroom(TextUI);
            }
            if (triggerGoodbye == null)
            {
                TutorialGoodbye(TextUI);
            }
            if (triggerLast == null)
            {
                lastTutorialTimer -= Time.deltaTime;
                TutorialLast(TextUI);
                if (lastTutorialTimer <= 0)
                {
                    tutorialWindow.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// The methods gets called when the player goes in to a specific trigger. 
    /// </summary>
    /// <returns></returns>
    #region TutorialText
    private string TutorialCrateText()
    {
        _textToShow = "Press 'A' to destroy the crate.";
        return _textToShow;
    }
    private string TutorialCrate(Text textUI)
    {
        _textToShow = "Press 'A' to destroy the crate.";
        TextUI.text = _textToShow;
        return _textToShow;
    }
    private string TutorialRageBar(Text textUI)
    {
        _textToShow = "See that red under your healthbar? " + "\n" +
            "That is your ragebar, it increases with every successfull hit.";
        TextUI.text = _textToShow;
        return _textToShow;
    }
    private string TutorialBridge(Text textUI)
    {
        _textToShow = "If you walk up this bridge we are going to try to test out your skills.";
        TextUI.text = _textToShow;
        return _textToShow;
    }
    private string TutorialFox(Text textUI)
    {
        _textToShow = "See that fox over there? kill it.";
        TextUI.text = _textToShow;
        return _textToShow;
    }
    private string TutorialDodge(Text textUI)
    {
        _textToShow = "Try to avoid the attack from the fox by pressing 'B'. ";
        TextUI.text = _textToShow;
        return _textToShow;
    }
    private string TutorialActivateRage(Text textUI)
    {
        _textToShow = "Press 'LB' to activate your rage and get some kick ass moves. ";
        TextUI.text = _textToShow;
        return _textToShow;
    }
    private string TutorialMushroom(Text textUI)
    {
        _textToShow = "If you were a pussy ass motherfucker and got hurt, " + "\n"
            + "don't worry" + "\n" +
            "there is a mushroom down the brigde that gives you health. ";
        TextUI.text = _textToShow;
        return _textToShow;
    }
    private string TutorialGoodbye(Text textUI)
    {
        _textToShow = " See that icon over the foxes head?  " + "\n" +
            "Press 'RB' to lock on a target";
        TextUI.text = _textToShow;
        return _textToShow;
    }
    private string TutorialLast(Text textUI)
    {
        _textToShow = " Oh by the way, look out for the ranged enemies," + "\n" +
            "okay fuck off now.";
        TextUI.text = _textToShow;
        return _textToShow;
    }
    #endregion
    #endregion
}
