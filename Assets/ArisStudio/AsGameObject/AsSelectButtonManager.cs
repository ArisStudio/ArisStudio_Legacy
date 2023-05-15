using System.Collections;
using System.Collections.Generic;
using ArisStudio.Core;
using ArisStudio.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.AsGameObject
{
    public class AsSelectButtonManager : Singleton<AsSelectButtonManager>
    {
        [Header("Select Button Audio")]
        [SerializeField]
        private AudioClip m_SelectButtonAudioClip;

        [Header("1 - Select Button")]
        [SerializeField]
        private TMP_Text m_SelectButtonText1;
        private Button selectButton1;

        [Header("2 - Select Button")]
        [SerializeField]
        private TMP_Text m_SelectButtonText2_1;

        [SerializeField]
        private TMP_Text m_SelectButtonText2_2;
        private Button selectButton2_1,
            selectButton2_2;

        [Header("3 - Select Button")]
        [SerializeField]
        private TMP_Text m_SelectButtonText3_1;

        [SerializeField]
        private TMP_Text m_SelectButtonText3_2,
            m_SelectButtonText3_3;
        private Button selectButton3_1,
            selectButton3_2,
            selectButton3_3;

        AudioSource selectButtonAudioSource;
        List<Button> selectButtons = new List<Button>();
        float selectButtonInitialScale = 0.7f;
        private string selectButtonTarget1,
            selectButtonTarget2,
            selectButtonTarget3;
        int commandLength;

        void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Self initialization.
        /// </summary>
        void Initialize()
        {
            selectButtons.Add(selectButton1 = GetButtonInParent(m_SelectButtonText1.gameObject));
            selectButtons.Add(
                selectButton2_1 = GetButtonInParent(m_SelectButtonText2_1.gameObject)
            );
            selectButtons.Add(
                selectButton2_2 = GetButtonInParent(m_SelectButtonText2_2.gameObject)
            );
            selectButtons.Add(
                selectButton3_1 = GetButtonInParent(m_SelectButtonText3_1.gameObject)
            );
            selectButtons.Add(
                selectButton3_2 = GetButtonInParent(m_SelectButtonText3_2.gameObject)
            );
            selectButtons.Add(
                selectButton3_3 = GetButtonInParent(m_SelectButtonText3_3.gameObject)
            );

            // Add CanvasGroup to do some Tweening
            foreach (Button btn in selectButtons)
                btn.gameObject.AddComponent((typeof(CanvasGroup)));

            selectButtonAudioSource = CreateAudioSource();
            selectButtonAudioSource.transform.SetParent(selectButton1.transform.parent);
            selectButtonAudioSource.clip = m_SelectButtonAudioClip;
            selectButtonAudioSource.outputAudioMixerGroup =
                AsAudioManager.Instance.AsAudioMixer.FindMatchingGroups("UI")[0];
        }

        Button GetButtonInParent(GameObject go)
        {
            return go.transform.parent.GetComponent<Button>();
        }

        /// <summary>
        /// Create Audio Source.
        /// </summary>
        /// <returns>Audio Source component.</returns>
        AudioSource CreateAudioSource()
        {
            AudioSource audioSource = new GameObject(
                "SelectButtonAudioSource",
                typeof(AudioSource)
            ).GetComponent<AudioSource>();
            audioSource.playOnAwake = false;

            return audioSource;
        }

        /// <summary>
        /// Initialize select buttons.
        /// </summary>
        public void AsSelectButtonInit()
        {
            foreach (Button btn in selectButtons)
            {
                btn.transform.localScale = new Vector3(
                    selectButtonInitialScale,
                    selectButtonInitialScale,
                    1
                );
                btn.GetComponent<CanvasGroup>().alpha = 1;
                btn.interactable = true;
            }

            selectButton1.onClick.AddListener(() => ClickButton(selectButton1, 1));
            selectButton1.gameObject.SetActive(false);
            m_SelectButtonText1.text = "";

            selectButton2_1.onClick.AddListener(() => ClickButton(selectButton2_1, 1));
            selectButton2_2.onClick.AddListener(() => ClickButton(selectButton2_2, 2));
            selectButton2_1.gameObject.SetActive(false);
            selectButton2_2.gameObject.SetActive(false);
            m_SelectButtonText2_1.text = m_SelectButtonText2_2.text = "";

            selectButton3_1.onClick.AddListener(() => ClickButton(selectButton3_1, 1));
            selectButton3_2.onClick.AddListener(() => ClickButton(selectButton3_2, 2));
            selectButton3_3.onClick.AddListener(() => ClickButton(selectButton3_3, 3));
            selectButton3_1.gameObject.SetActive(false);
            selectButton3_2.gameObject.SetActive(false);
            selectButton3_3.gameObject.SetActive(false);

            m_SelectButtonText3_1.text =
                m_SelectButtonText3_2.text =
                m_SelectButtonText3_3.text =
                    "";

            selectButtonTarget1 = selectButtonTarget2 = selectButtonTarget3 = "";
        }

        /// <summary>
        /// Setting up select buttons.
        /// </summary>
        /// <param name="command"></param>
        public void SetButton(string[] command)
        {
            AsSelectButtonInit();

            switch (command.Length)
            {
                case 5:
                    m_SelectButtonText2_1.text = command[1];
                    m_SelectButtonText2_2.text = command[3];
                    selectButton2_1.gameObject.SetActive(true);
                    selectButton2_2.gameObject.SetActive(true);
                    selectButtonTarget1 = command[2];
                    selectButtonTarget2 = command[4];
                    break;

                case 7:
                    m_SelectButtonText3_1.text = command[1];
                    m_SelectButtonText3_2.text = command[3];
                    m_SelectButtonText3_3.text = command[5];
                    selectButton3_1.gameObject.SetActive(true);
                    selectButton3_2.gameObject.SetActive(true);
                    selectButton3_3.gameObject.SetActive(true);
                    selectButtonTarget1 = command[2];
                    selectButtonTarget2 = command[4];
                    selectButtonTarget3 = command[6];
                    break;

                default:
                    m_SelectButtonText1.text = command[1];
                    selectButton1.gameObject.SetActive(true);
                    selectButtonTarget1 = command[2];
                    break;
            }

            foreach (Button btn in selectButtons)
            {
                if (btn.gameObject.activeSelf)
                    btn.transform.DOScale(Vector3.one, 0.3f);
            }

            commandLength = command.Length;
            AutoSelect(commandLength); // determined if auto select button will be executed or not
        }

        /// <summary>
        /// Execute corresponding functions when the select button is clicked.
        /// </summary>
        /// <param name="buttonNumber"></param>
        public void ClickButton(Button selectButton, int buttonNumber)
        {
            selectButtonAudioSource.Play();

            string selectTarget = buttonNumber switch
            {
                3 => selectButtonTarget3,
                2 => selectButtonTarget2,
                _ => selectButtonTarget1
            };

            foreach (Button btn in selectButtons)
                btn.interactable = false;

            ClickButtonAnimation(selectButton, selectTarget);
        }

        /// <summary>
        /// Animate the select button when clicked.
        /// </summary>
        /// <param name="button"></param>
        /// <param name="selectTarget"></param>
        void ClickButtonAnimation(Button button, string selectTarget)
        {
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            CanvasGroup buttonCanvasGroup = button.GetComponent<CanvasGroup>();

            DOTween
                .Sequence()
                .Append(button.transform.DOScale(new Vector3(0.95f, 0.95f, 1), 0.2f))
                .Append(button.transform.DOScale(new Vector3(1.1f, 1.1f, 1), 0.35f))
                .OnComplete(() => buttonText.text = "")
                .Append(
                    DOTween
                        .Sequence()
                        .Append(buttonCanvasGroup.DOFade(0.15f, 0.05f))
                        .Append(buttonCanvasGroup.DOFade(0.2f, 0.05f))
                        .SetLoops(4, LoopType.Yoyo)
                )
                .OnComplete(() =>
                {
                    MainManager.Instance.IsSelectChoice = false;
                    AsSelectButtonManager.Instance.AsSelectButtonInit();
                    MainManager.Instance.JumpTarget(selectTarget);
                });
        }

        /// <summary>
        /// Auto choose select button.
        /// </summary>
        /// <param name="commandLength"></param>
        private void AutoSelect(int commandLength)
        {
            StartCoroutine(AutoSelectRoutine(commandLength));
        }

        /// <summary>
        /// When the select button has shown up, but Auto button isn't already activated.
        /// (Automatically, the AutoSelect() that we put in SetButton() will not get executed).
        /// NOTE: Add this to Auto -> Activate button OnClick.
        /// </summary>
        public void AutoSelectLate()
        {
            if (MainManager.Instance.IsAuto && MainManager.Instance.IsSelectChoice)
                AutoSelect(commandLength);
        }

        /// <summary>
        /// Stop auto select button routine from being executed.
        /// NOTE: Add this to Auto -> Deactivate button OnClick.
        /// </summary>S
        public void AutoSelectLateStop()
        {
            MainManager.Instance.IsAuto = false;

            if (MainManager.Instance.IsSelectChoice)
                StopAllCoroutines();
        }

        /// <summary>
        /// Auto choose select button main logic.
        /// </summary>
        /// <param name="commandLength"></param>
        /// <returns></returns>
        private IEnumerator AutoSelectRoutine(int commandLength)
        {
            /*
            * NOTE: Don't add MainManager.Instance.IsSelectChoice as the condition,
            * even though the condition is met, however the below function will not
            * get executed. Don't know why :(
            */

            // If this Auto and the the select button is only one.
            if (MainManager.Instance.IsAuto && commandLength == 3)
            {
                yield return new WaitForSeconds(MainManager.Instance.AutoTime);
                ClickButton(selectButton1, 1);
            }
            else
                yield break;
        }
    }
}
