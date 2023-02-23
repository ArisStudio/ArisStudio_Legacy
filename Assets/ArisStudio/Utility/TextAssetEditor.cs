using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArisStudio.Core;
using TMPro;
using UnityEngine;

namespace ArisStudio.Utility
{
    [AddComponentMenu("Aris Studio/Utility/Text Editor")]
    public class TextAssetEditor : MonoBehaviour
    {
        [Header("User Interface")]
        [SerializeField] TMP_InputField m_TextBoxEditor;
        List<string> currentEditedStory;

        SettingsManager settingsManager;

        void Awake()
        {
            settingsManager = MainControl.Instance.m_SettingsManager;
        }

        public void Edit()
        {
            string currentStoryPath = settingsManager.currentStoryFilePath;
            currentEditedStory = File.ReadAllLines(currentStoryPath).ToList();

            Revert();
        }

        public void Revert()
        {
            foreach (string line in currentEditedStory)
                m_TextBoxEditor.text += $"{line}\n";
        }

        public void Save()
        {
            
        }
    }
}
