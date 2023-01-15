using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ArisStudio.Sound
{
    public class SoundFactory : MonoBehaviour
    {
        public DebugConsole debugConsole;

        private string bgmDataPath, soundEffectDataPath;

        Dictionary<string, AudioClip> bgmList = new Dictionary<string, AudioClip>();
        Dictionary<string, AudioClip> soundEffectList = new Dictionary<string, AudioClip>();

        public void SetSoundDataPath(string rootPath)
        {
            bgmDataPath = Path.Combine(rootPath, "Bgm");
            soundEffectDataPath = Path.Combine(rootPath, "SoundEffect");
        }

        public void Initialize()
        {
            bgmList.Clear();
            soundEffectList.Clear();
        }

        # region Load Sound

        public void LoadBgm(string nameId, string bgmName)
        {
            StartCoroutine(LoadSound(nameId, bgmName, "Bgm"));
        }

        public void LoadSoundEffect(string nameId, string soundEffectName)
        {
            StartCoroutine(LoadSound(nameId, soundEffectName, "SoundEffect"));
        }

        private AudioType SelectAudioType(string soundName)
        {
            if (soundName.EndsWith(".ogg"))
            {
                return AudioType.OGGVORBIS;
            }
            else if (soundName.EndsWith(".wav"))
            {
                return AudioType.WAV;
            }
            else
            {
                return AudioType.UNKNOWN;
            }
        }

        private IEnumerator LoadSound(string nameId, string soundName, string soundType)
        {
            switch (soundType)
            {
                case "Bgm":
                {
                    var bgmPath = Path.Combine(bgmDataPath, soundName);
                    var www = UnityWebRequestMultimedia.GetAudioClip(bgmPath, SelectAudioType(soundName));
                    yield return www.SendWebRequest();
                    bgmList.Add(nameId, DownloadHandlerAudioClip.GetContent(www));
                    break;
                }
                case "SoundEffect":
                {
                    var soundEffectPath = Path.Combine(soundEffectDataPath, soundName);
                    var www = UnityWebRequestMultimedia.GetAudioClip(soundEffectPath, SelectAudioType(soundName));
                    yield return www.SendWebRequest();
                    soundEffectList.Add(nameId, DownloadHandlerAudioClip.GetContent(www));
                    break;
                }
            }

            debugConsole.PrintLog($"Load {soundType}: <color=lime>{soundName}</color>");
        }

        #endregion


        public void SoundCommand(string soundCommand)
        {
        }
    }
}