using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArisStudio
{
    public class SelectButton : MonoBehaviour
    {
        public MainControl mainControl;

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

        private string t1, t2, t3;
        private bool isSelecting;


        private void Update()
        {
            if (!isSelecting) return;

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha1))
            {
                Select(1);
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha2))
            {
                Select(2);
            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha3))
            {
                Select(3);
            }
        }

        public void Select(int i)
        {
            isSelecting = false;

            switch (i)
            {
                case 1:
                    mainControl.SetSelect(t1);
                    break;
                case 2:
                    mainControl.SetSelect(t2);
                    break;
                case 3:
                    mainControl.SetSelect(t3);
                    break;
            }

            button1.gameObject.SetActive(false);
            button21.gameObject.SetActive(false);
            button22.gameObject.SetActive(false);
            button31.gameObject.SetActive(false);
            button32.gameObject.SetActive(false);
            button33.gameObject.SetActive(false);
        }

        public void SelectCommand(string selectCommand)
        {
            var l = selectCommand.Split('\'');
            switch (l.Length)
            {
                case 5:
                    text1.text = l[1];
                    t1 = l[3];
                    button1.gameObject.SetActive(true);
                    break;
                case 9:
                    text21.text = l[1];
                    t1 = l[3];
                    text22.text = l[5];
                    t2 = l[7];
                    button21.gameObject.SetActive(true);
                    button22.gameObject.SetActive(true);
                    break;
                case 13:
                    text31.text = l[1];
                    t1 = l[3];
                    text32.text = l[5];
                    t2 = l[7];
                    text33.text = l[9];
                    t3 = l[11];
                    button31.gameObject.SetActive(true);
                    button32.gameObject.SetActive(true);
                    button33.gameObject.SetActive(true);
                    break;
            }

            isSelecting = true;
        }
    }
}