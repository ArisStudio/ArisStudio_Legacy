using System.Collections.Generic;
using ArisStudio.Core;
using ArisStudio.UI;
using ArisStudio.Utils;
using KoganeUnityLib;
using UnityEngine;

namespace ArisStudio.AsGameObject
{
    public class AsDialogueManager : Singleton<AsDialogueManager>
    {
        [SerializeField] CanvasGroup m_DialogueContainer, m_DefaultText;
        [SerializeField] TMP_Typewriter m_MiddleText, m_BottomText, m_NameText, m_GroupText, m_ContentText;
        [SerializeField] DialogueIndicatorAnimation m_Indicator;

        List<TMP_Typewriter> currentlyTyping = new List<TMP_Typewriter>(); // store TMP that currently typing.

        void Start()
        {
            m_DialogueContainer.gameObject.SetActive(false); // hide dialogue panel
        }

        /// <summary>
        /// Initialize the dialogue components.
        /// </summary>
        public void AsDialogueInit()
        {
            m_DialogueContainer.gameObject.SetActive(true);

            m_MiddleText.m_TMP_Text.text = "";
            m_MiddleText.gameObject.SetActive(false);
            m_BottomText.m_TMP_Text.text = "";
            m_BottomText.gameObject.SetActive(false);

            m_NameText.m_TMP_Text.text = "";
            m_GroupText.m_TMP_Text.text = "";
            m_ContentText.m_TMP_Text.text = "";
            m_DefaultText.gameObject.SetActive(false);
        }

        /// <summary>
        /// Make a typing effect on a TMP.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="tmp_typewriter"></param>
        private void TypingText(string text, TMP_Typewriter tmp_typewriter)
        {
            MainManager.Instance.IsTyping = true;
            currentlyTyping.Add(tmp_typewriter);
            m_Indicator.StopAnimate();

            tmp_typewriter.Play
            (
                text        : text,
                speed       : SettingsManager.Instance.currentTypingSpeed,
                onComplete  : () => {
                    MainManager.Instance.IsTyping = false;
                    m_Indicator.StartAnimate();
                },
                // ルビがある行とない行で高さが変動しないようにするにはtrue
                fixedLineHeight: false,
                // 1行目にルビがある時、TextMeshProのMargin機能を使って位置調整
                autoMarginTop: true
            );
        }

        /// <summary>
        /// Skip all registered typing text.
        /// </summary>
        public void SkipTypingText()
        {
            foreach (TMP_Typewriter typewriter in currentlyTyping)
                typewriter.Skip();

            currentlyTyping.Clear();
        }

        private void SetDefaultDialogueText(string[] asDialogueCommand, int offset = 0)
        {
            switch (asDialogueCommand.Length)
            {
                case 1:
                    m_NameText.m_TMP_Text.text = "";
                    m_GroupText.m_TMP_Text.text = "";
                    m_ContentText.m_TMP_Text.text = "";
                    break;
                case 2:
                    m_NameText.m_TMP_Text.text = "";
                    m_GroupText.m_TMP_Text.text = "";
                    TypingText(asDialogueCommand[1 + offset], m_ContentText);
                    break;
                case 3:
                    m_NameText.m_TMP_Text.text = asDialogueCommand[1 + offset];
                    m_GroupText.m_TMP_Text.text = "";
                    TypingText(asDialogueCommand[2 + offset], m_ContentText);
                    break;
                default:
                    m_NameText.m_TMP_Text.text = asDialogueCommand[1 + offset];
                    m_GroupText.m_TMP_Text.text = asDialogueCommand[2 + offset];
                    TypingText(asDialogueCommand[3 + offset], m_ContentText);
                    break;
            }
        }

        private void SetDefaultDialogueTextSize(string size)
        {
            m_ContentText.m_TMP_Text.text = "";
            m_ContentText.m_TMP_Text.fontSize = size switch
            {
                "big" => 60,
                "small" => 32,
                "medium" => 42,
                _ => m_ContentText.m_TMP_Text.fontSize
            };
        }

        public void AsDialogueCommand(string[] asDialogueCommand)
        {
            switch (asDialogueCommand[0])
            {
                case "mt":
                case "middle_text":
                case "mtc":
                    m_MiddleText.gameObject.SetActive(true);
                    TypingText(asDialogueCommand[1], m_MiddleText);
                    break;

                case "bt":
                case "bottom_text":
                case "btc":
                    m_BottomText.gameObject.SetActive(true);
                    TypingText(asDialogueCommand[1], m_BottomText);
                    break;

                case "t":
                case "txt":
                case "tc":
                    m_DefaultText.gameObject.SetActive(true);
                    SetDefaultDialogueText(asDialogueCommand);
                    break;

                case "th":
                    m_DefaultText.gameObject.SetActive(true);
                    SetDefaultDialogueText(asDialogueCommand, 1);
                    break;

                case "text":
                    switch (asDialogueCommand[1])
                    {
                        case "hide":
                            m_DialogueContainer.gameObject.SetActive(false);
                            break;

                        case "speed": // formerly interval
                            SettingsManager.Instance.currentTypingSpeed = float.Parse(asDialogueCommand[2]);
                            break;

                        case "size":
                            SetDefaultDialogueTextSize(asDialogueCommand[2]);
                            break;
                    }
                    break;
            }
        }
    }
}
