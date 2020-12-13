using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    #region Variables
    private static TextWriter instance;
    private List<SingularTextPart> textPart;
    private LoadLevel load;
    public GameObject levelLoader;
    #endregion

    #region Methods
    public static void AddWriterStatic(Text uiText, string theText, float characterTime, bool invisible)
    {
        instance.AddWriter(uiText, theText, characterTime, invisible);
    }

    private void AddWriter(Text uiText, string theText, float characterTime, bool invisible)
    {
        textPart.Add(new SingularTextPart(uiText, theText, characterTime, invisible));
    }

    private void Awake()
    {
        instance = this;
        textPart = new List<SingularTextPart>();
        load = levelLoader.GetComponent<LoadLevel>();
    }

    private void Update()
    {
        for (int i = 0; i < textPart.Count; i++)
        {
            bool textDone = textPart[i].Update();
            if (textDone)
            {
                textPart.RemoveAt(i);
                i--;
            }
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            load.LoadNextLevel();
        }
    }

    public class SingularTextPart
    {
        private Text text;
        private string theText;
        private float charcterTime;
        private float timer;
        private int index;
        private bool invisible;

        public SingularTextPart(Text uiText, string theText, float characterTime, bool invisible)
        {
            this.text = uiText;
            this.theText = theText;
            this.charcterTime = characterTime;
            index = 0;
            this.invisible = invisible;
        }

        public bool Update()
        {
            timer -= Time.deltaTime;
            while (timer <= 0f)
            {
                timer += charcterTime;
                index++;
                string shownText = theText.Substring(0, index);
                if (invisible)
                {
                    shownText += "<color=#00000000>"/*transparant*/ + theText.Substring(index) + "</color>";  //RGBA color
                }
                text.text = shownText;

                if (index > theText.Length)
                {
                    text = null;
                    return true;
                }
            }
            return false;
        }
    }
    #endregion
}
