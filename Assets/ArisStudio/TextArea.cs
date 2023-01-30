using System.Linq;
using RichText;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio
{
    public class TextArea : MonoBehaviour
    {
        public DebugConsole debugConsole;
        public MainControl mainControl;
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
        private float typingInterval = 0.02f;

        private void Start()
        {
            var l = Font.GetOSInstalledFontNames();
            dDropdown.AddOptions(l.ToList());
        }

        private void Update()
        {
            if (!typing) return;

            timer += Time.deltaTime;
            if (timer < typingInterval) return;

            timer = 0;
            currentPos++;
            tContent.text = tmpContent.RichTextSubString(currentPos);

            if (currentPos >= tmpContent.RichTextLength())
            {
                PlayAllText();
            }
        }

        private void SetFont(Font f)
        {
            tName.font = f;
            tGroup.font = f;
            tContent.font = f;
            s1.font = f;
            s21.font = f;
            s22.font = f;
            s31.font = f;
            s32.font = f;
            s33.font = f;
        }

        public void SetDefaultFont()
        {
            SetFont(defaultFont);
            debugConsole.PrintLog("Set default font");
        }

        public void ChangeFont()
        {
            var f = Font.CreateDynamicFontFromOSFont(dDropdown.options[dDropdown.value].text, 20);
            SetFont(f);
            debugConsole.PrintLog($"Font changed to <color=lime>{f.name}</color>");
        }

        public void SetTypeInterval()
        {
            var value = sSlider.value / 100;
            typingInterval = value;
            tSlider.text = $"{value:F2} s";
        }

        public void SetText(string sName, string sGroup, string sContent)
        {
            mainControl.SetTyping(true);

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
            mainControl.SetTyping(false);
        }

        public void TextCommand(string textCommand)
        {
            var l = textCommand.Split(' ');
            switch (l[1])
            {
                case "size":
                    SetTextSize(l[2]);
                    break;

                case "interval":
                    typingInterval = float.Parse(l[2]);
                    sSlider.value = typingInterval * 100;
                    tSlider.text = $"{typingInterval:F2} s";
                    debugConsole.PrintLog($"Set typing interval to {typingInterval} s");
                    break;

                case "font":
                    var f = Font.CreateDynamicFontFromOSFont(l[2], 20);
                    SetFont(f);
                    debugConsole.PrintLog($"Font(Command) changed to <color=lime>{f.name}</color>");
                    break;

                case "hide":
                    gameObject.SetActive(false);
                    break;
            }
        }
    }
}