using System.Collections;
using ArisStudio.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio.UI
{
    public class SelectButton : MonoBehaviour
    {
        public AudioSource selectSound;

        [Header("B1")] public Button button1;
        public Text text1;

        [Header("B2")] public Button button21;
        public Text text21;
        public Button button22;
        public Text text22;

        [Header("B3")] public Button button31;
        public Text text31;
        public Button button32;
        public Text text32;
        public Button button33;
        public Text text33;

        string t1, t2, t3;
        bool isSelecting;

        const float WaitSelectTime = 3f;
        const float SelectWaitTime = 0.3f;


        void Update()
        {
            if (!isSelecting) return;

            if (t1 != string.Empty && Input.GetKeyDown(KeyCode.Alpha1))
                Select(1);
            else if (t2 != string.Empty && Input.GetKeyDown(KeyCode.Alpha2))
                Select(2);
            else if (t3 != string.Empty && Input.GetKeyDown(KeyCode.Alpha3))
                Select(3);
        }

        public void Select(int i)
        {
            isSelecting = false;

            selectSound.Play();
            StartCoroutine(SelectAndWait(i));
        }

        IEnumerator WaitAndSelect(int i)
        {
            yield return new WaitForSeconds(WaitSelectTime);
            Select(i);
        }

        IEnumerator SelectAndWait(int i)
        {
            switch (i)
            {
                case 1:
                    button1.transform.localScale = Vector3.one * 0.9f;
                    button21.transform.localScale = Vector3.one * 0.9f;
                    button31.transform.localScale = Vector3.one * 0.9f;
                    yield return new WaitForSeconds(SelectWaitTime);
                    MainControl.Instance.SetSelect(t1);
                    break;
                case 2:
                    button22.transform.localScale = Vector3.one * 0.9f;
                    button32.transform.localScale = Vector3.one * 0.9f;
                    yield return new WaitForSeconds(SelectWaitTime);
                    MainControl.Instance.SetSelect(t2);
                    break;
                case 3:
                    button33.transform.localScale = Vector3.one * 0.9f;
                    yield return new WaitForSeconds(SelectWaitTime);
                    MainControl.Instance.SetSelect(t3);
                    break;
            }

            Initialize();
        }

        public void Initialize()
        {
            t1 = t2 = t3 = string.Empty;

            button1.gameObject.SetActive(false);
            button21.gameObject.SetActive(false);
            button22.gameObject.SetActive(false);
            button31.gameObject.SetActive(false);
            button32.gameObject.SetActive(false);
            button33.gameObject.SetActive(false);
        }

        public void SelectCommandWithSelect(int i, string[] selectCommand)
        {
            SelectCommand(selectCommand);
            StartCoroutine(WaitAndSelect(i));
        }


        public void SelectCommand(string[] selectCommand)
        {
            button1.transform.localScale = Vector3.one;
            button21.transform.localScale = Vector3.one;
            button22.transform.localScale = Vector3.one;
            button31.transform.localScale = Vector3.one;
            button32.transform.localScale = Vector3.one;
            button33.transform.localScale = Vector3.one;


            // var l = selectCommand.Split('\'');
            switch (selectCommand.Length)
            {
                case 3:
                    text1.text = selectCommand[1];
                    t1 = selectCommand[2];
                    button1.gameObject.SetActive(true);
                    break;
                case 5:
                    text21.text = selectCommand[1];
                    t1 = selectCommand[2];
                    text22.text = selectCommand[3];
                    t2 = selectCommand[4];
                    button21.gameObject.SetActive(true);
                    button22.gameObject.SetActive(true);
                    break;
                case 7:
                    text31.text = selectCommand[1];
                    t1 = selectCommand[2];
                    text32.text = selectCommand[3];
                    t2 = selectCommand[4];
                    text33.text = selectCommand[5];
                    t3 = selectCommand[6];
                    button31.gameObject.SetActive(true);
                    button32.gameObject.SetActive(true);
                    button33.gameObject.SetActive(true);
                    break;
            }
            // switch (selectCommand.Length)
            // {
            //     case 5:
            //         text1.text = selectCommand[1];
            //         t1 = selectCommand[3];
            //         button1.gameObject.SetActive(true);
            //         break;
            //     case 9:
            //         text21.text = selectCommand[1];
            //         t1 = selectCommand[3];
            //         text22.text = selectCommand[5];
            //         t2 = selectCommand[7];
            //         button21.gameObject.SetActive(true);
            //         button22.gameObject.SetActive(true);
            //         break;
            //     case 13:
            //         text31.text = selectCommand[1];
            //         t1 = selectCommand[3];
            //         text32.text = selectCommand[5];
            //         t2 = selectCommand[7];
            //         text33.text = selectCommand[9];
            //         t3 = selectCommand[11];
            //         button31.gameObject.SetActive(true);
            //         button32.gameObject.SetActive(true);
            //         button33.gameObject.SetActive(true);
            //         break;
            // }

            isSelecting = true;
        }
    }
}
