using ArisStudio.Core;
using ArisStudio.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.AsGameObject
{
    public class AsSelectButtonManager : Singleton<AsSelectButtonManager>
    {
        [Header("Select Button Audio")] [SerializeField]
        private AudioSource selectButtonAudioSource;

        [Header("Select Button 1")] [SerializeField]
        private GameObject selectButtonGo1;

        [SerializeField] private Text selectButtonText1;

        [Header("Select Button 2")] [SerializeField]
        private GameObject selectButtonGo21;

        [SerializeField] private GameObject selectButtonGo22;

        [SerializeField] private Text selectBtnText21;
        [SerializeField] private Text selectBtnText22;

        [Header("Select Button 3")] [SerializeField]
        private GameObject selectButtonGo31;

        [SerializeField] private GameObject selectButtonGo32;
        [SerializeField] private GameObject selectButtonGo33;

        [SerializeField] private Text selectBtnText31;
        [SerializeField] private Text selectBtnText32;
        [SerializeField] private Text selectBtnText33;

        private string selectButtonTarget1, selectButtonTarget2, selectButtonTarget3;

        public void AsSelectButtonInit()
        {
            selectButtonGo1.SetActive(false);
            selectButtonGo1.transform.localScale = Vector3.one;
            selectButtonText1.text = "";

            selectButtonGo21.SetActive(false);
            selectButtonGo22.SetActive(false);
            selectButtonGo21.transform.localScale = selectButtonGo22.transform.localScale = Vector3.one;
            selectBtnText21.text = selectBtnText22.text = "";

            selectButtonGo31.SetActive(false);
            selectButtonGo32.SetActive(false);
            selectButtonGo33.SetActive(false);

            selectButtonGo31.transform.localScale = selectButtonGo32.transform.localScale = selectButtonGo33.transform.localScale
                = Vector3.one;

            selectBtnText31.text = selectBtnText32.text = selectBtnText33.text = "";

            selectButtonTarget1 = selectButtonTarget2 = selectButtonTarget3 = "";
        }

        public void ClickButton(int btnNum)
        {
            selectButtonAudioSource.Play();

            string selectTarget = btnNum switch
            {
                2 => selectButtonTarget2,
                3 => selectButtonTarget3,
                _ => selectButtonTarget1
            };

            MainManager.Instance.JumpTarget(selectTarget);
        }

        public void SetButton(string[] command)
        {
            AsSelectButtonInit();

            switch (command.Length)
            {
                case 5:
                    selectBtnText21.text = command[1];
                    selectBtnText22.text = command[3];
                    selectButtonGo21.SetActive(true);
                    selectButtonGo22.SetActive(true);
                    selectButtonTarget1 = command[2];
                    selectButtonTarget2 = command[4];
                    break;

                case 7:
                    selectBtnText31.text = command[1];
                    selectBtnText32.text = command[3];
                    selectBtnText33.text = command[5];
                    selectButtonGo31.SetActive(true);
                    selectButtonGo32.SetActive(true);
                    selectButtonGo33.SetActive(true);
                    selectButtonTarget1 = command[2];
                    selectButtonTarget2 = command[4];
                    selectButtonTarget3 = command[6];
                    break;

                default:
                    selectButtonText1.text = command[1];
                    selectButtonGo1.SetActive(true);
                    selectButtonTarget1 = command[2];
                    break;
            }
        }
    }
}
