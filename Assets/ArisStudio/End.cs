using UnityEngine;

namespace ArisStudio
{
    public class End : MonoBehaviour
    {
        public GameObject endFinish, endContinue;

        public void EndCommand(string endCommand)
        {
            var l = endCommand.Split(' ');
            switch (l[1])
            {
                case "finish":
                    endContinue.SetActive(false);
                    endFinish.SetActive(true);
                    break;
                case "continue":
                    endFinish.SetActive(false);
                    endContinue.SetActive(true);
                    break;
                case "clear":
                    Clear();
                    break;
            }
        }

        public void Clear()
        {
            endContinue.SetActive(false);
            endFinish.SetActive(false);
        }
    }
}