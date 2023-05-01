using ArisStudio.Core;
using ArisStudio.Utils;
using DG.Tweening;
using RichText;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.AsGameObject
{
    public class AsDialogueManager : Singleton<AsDialogueManager>
    {
        [SerializeField] private Text middleText, bottomText, nameText, groupText, contentText;
        [SerializeField] private GameObject defaultText, indicator;

        public void AsDialogueInit()
        {
            middleText.text = "";
            middleText.gameObject.SetActive(false);
            bottomText.text = "";
            bottomText.gameObject.SetActive(false);

            nameText.text = "";
            groupText.text = "";
            contentText.text = "";
            defaultText.SetActive(false);
        }

        private void TypingText(string text, Text textObject)
        {
            textObject.text = "";
            textObject.gameObject.SetActive(true);
            textObject.DOText(
                text.Replace("<n>", "\n"),
                text.RichTextLength() * SettingsManager.Instance.currentTypingInterval
            ).OnComplete(() => indicator.SetActive(true));
        }

        private void SetDefaultDialogueText(string[] asDialogueCommand, int offset = 0)
        {
            indicator.SetActive(false);
            switch (asDialogueCommand.Length)
            {
                case 1:
                    nameText.text = "";
                    groupText.text = "";
                    contentText.text = "";
                    break;
                case 2:
                    nameText.text = "";
                    groupText.text = "";
                    TypingText(asDialogueCommand[1 + offset], contentText);
                    break;
                case 3:
                    nameText.text = asDialogueCommand[1 + offset];
                    groupText.text = "";
                    TypingText(asDialogueCommand[2 + offset], contentText);
                    break;
                default:
                    nameText.text = asDialogueCommand[1 + offset];
                    groupText.text = asDialogueCommand[2 + offset];
                    TypingText(asDialogueCommand[3 + offset], contentText);
                    break;
            }
        }

        private void SetDefaultDialogueTextSize(string size)
        {
            contentText.text = "";
            contentText.fontSize = size switch
            {
                "big" => 60,
                "small" => 32,
                "medium" => 42,
                _ => contentText.fontSize
            };
        }

        public void AsDialogueCommand(string[] asDialogueCommand)
        {
            switch (asDialogueCommand[0])
            {
                case "mt":
                case "middle_text":
                    middleText.gameObject.SetActive(true);
                    TypingText(asDialogueCommand[1], middleText);
                    break;

                case "bt":
                case "bottom_text":
                    bottomText.gameObject.SetActive(true);
                    TypingText(asDialogueCommand[1], bottomText);
                    break;

                case "t":
                case "txt":
                case "tc":
                    defaultText.SetActive(true);
                    SetDefaultDialogueText(asDialogueCommand);
                    break;

                case "th":
                    defaultText.SetActive(true);
                    SetDefaultDialogueText(asDialogueCommand, 1);
                    break;

                case "text":
                    switch (asDialogueCommand[1])
                    {
                        case "hide":
                            middleText.gameObject.SetActive(false);
                            bottomText.gameObject.SetActive(false);
                            contentText.gameObject.SetActive(false);
                            break;

                        case "interval":
                            SettingsManager.Instance.currentTypingInterval = float.Parse(asDialogueCommand[2]);
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
