// using System.Linq;
using ArisStudio.Core;
using RichText;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.UI
{
    public class TextArea : MonoBehaviour
    {
        public Text tName, tGroup, tContent;
        public Slider sSlider;
        public Text tSlider;
        public Dropdown dDropdown;
        public Font defaultFont;
        public GameObject br;

        [Header("SelectButtonText")] public Text s1;
        public Text s21, s22, s31, s32, s33;

        private string tmpContent;

        private int currentPos;
        private float timer;
        private bool typing;
        // private float currentTypingInterval = 0.02f;

        // DebugConsole debugConsole;

        // void Awake()
        // {
        // }

        void Start()
        {
            // debugConsole = DebugConsole.Instance;
            // var l = Font.GetOSInstalledFontNames();
            // dDropdown.AddOptions(l.ToList());
        }

        private void Update()
        {
            if (!typing) return;

            timer += Time.deltaTime;
            if (timer < SettingsManager.Instance.currentTypingInterval) return;

            timer = 0;
            currentPos++;
            tContent.text = tmpContent.RichTextSubString(currentPos);

            if (currentPos >= tmpContent.RichTextLength())
            {
                PlayAllText();
            }
        }

        // private void SetFont(Font f)
        // {
        //     tName.font = f;
        //     tGroup.font = f;
        //     tContent.font = f;
        //     s1.font = f;
        //     s21.font = f;
        //     s22.font = f;
        //     s31.font = f;
        //     s32.font = f;
        //     s33.font = f;
        // }

        // public void SetDefaultFont()
        // {
        //     SetFont(defaultFont);
        //     MainControl.Instance.m_DebugConsole.PrintLog("Set default font");
        // }

        // public void ChangeFont()
        // {
        //     var f = Font.CreateDynamicFontFromOSFont(dDropdown.options[dDropdown.value].text, 20);
        //     SetFont(f);
        //     MainControl.Instance.m_DebugConsole.PrintLog($"Font changed to <#00ff00>{f.name}</color>");
        // }

        // public void SetTypeInterval()
        // {
        //     var value = sSlider.value / 100;
        //     MainControl.Instance.m_SettingsManager.currentTypingInterval = value;
        //     tSlider.text = $"{value:F2} s";
        // }

        public void SetText(string sName, string sGroup, string sContent)
        {
            MainControl.Instance.SetTyping(true);

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            timer = 0;
            tName.text = sName == string.Empty ? " " : sName;
            tGroup.text = sGroup == string.Empty ? " " : sGroup;
            tContent.text = string.Empty;
            tmpContent = sContent == string.Empty ? " " : sContent;
            typing = true;
            br.SetActive(false);
        }

        private void SetTextSize(string size)
        {
            tContent.text = string.Empty;
            switch (size)
            {
                case "big":
                    tContent.fontSize = 60;
                    break;
                case "small":
                    tContent.fontSize = 32;
                    break;
                case "medium":
                    tContent.fontSize = 42;
                    break;
            }
        }

        public void PlayAllText()
        {
            typing = false;
            br.SetActive(true);
            currentPos = 0;
            tContent.text = tmpContent;
            MainControl.Instance.SetTyping(false);
        }

        public void TextCommand(string[] textCommand)
        {
            // var l = textCommand.Split(' ');
            switch (textCommand[1])
            {
                case "size":
                    SetTextSize(textCommand[2]);
                    break;

                case "interval":
                    SettingsManager.Instance.currentTypingInterval = float.Parse(textCommand[2]);
                    sSlider.value = SettingsManager.Instance.currentTypingInterval * 100;
                    tSlider.text = $"{SettingsManager.Instance.currentTypingInterval:F2} s";
                    DebugConsole.Instance.PrintLog($"Set typing interval to {SettingsManager.Instance.currentTypingInterval} s");
                    break;

                // case "font":
                //     var f = Font.CreateDynamicFontFromOSFont(l[2], 20);
                //     SetFont(f);
                //     DebugConsole.PrintLog($"Font(Command) changed to <#00ff00>{f.name}</color>");
                //     break;

                case "hide":
                    gameObject.SetActive(false);
                    break;
            }
        }
    }
}
