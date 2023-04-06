using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArisStudio.Core;
using TMPro;
using UnityEngine;

namespace ArisStudio.Utils
{
    [AddComponentMenu("Aris Studio/Utility/Text Editor")]
    public class TextAssetEditor : MonoBehaviour
    {
        [Header("User Interface")]
        [SerializeField]
        TMP_InputField m_TextBoxEditor;
        List<string> currentEditedStory;

        public void Edit()
        {
            string currentStoryPath = SettingsManager.Instance.currentStoryFilePath;
            currentEditedStory = File.ReadAllLines(currentStoryPath).ToList();

            Revert();
        }

        public void Revert()
        {
            foreach (string line in currentEditedStory)
                m_TextBoxEditor.text += $"{line}\n";
        }

        public void Save() { }
    }
}
