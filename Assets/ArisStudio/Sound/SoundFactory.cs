using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArisStudio.Core;
using UnityEngine;
using UnityEngine.Networking;

namespace ArisStudio.Sound
{
    public class SoundFactory : MonoBehaviour
    {
        public Bgm bgm;
        public SoundEffect se;

        // private string bgmDataPath, soundEffectDataPath;

        Dictionary<string, AudioClip> bgmList = new Dictionary<string, AudioClip>();
        Dictionary<string, AudioClip> soundEffectList = new Dictionary<string, AudioClip>();

        DebugConsole debugConsole;
        SettingsManager settingsManager;

        void Awake()
        {
            debugConsole = MainControl.Instance.m_DebugConsole;
            settingsManager = MainControl.Instance.m_SettingsManager;
        }

        public void SetSoundDataPath(string rootPath)
        {
            // bgmDataPath = Path.Combine(rootPath, "Bgm");
            // soundEffectDataPath = Path.Combine(rootPath, "SoundEffect");
        }

        public void Initialize()
        {
            bgmList.Clear();
            bgm.Stop();
            bgm.ChangeVolume();

            soundEffectList.Clear();
            se.Stop();
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
                    var bgmPath = Path.Combine(
                        settingsManager.currentBGMPath,
                        soundName
                    );
                    var www = UnityWebRequestMultimedia.GetAudioClip(
                        bgmPath,
                        SelectAudioType(soundName)
                    );
                    yield return www.SendWebRequest();
                    bgmList.Add(nameId, DownloadHandlerAudioClip.GetContent(www));
                    break;
                }
                case "SoundEffect":
                {
                    var soundEffectPath = Path.Combine(
                        settingsManager.currentSFXPath,
                        soundName
                    );
                    var www = UnityWebRequestMultimedia.GetAudioClip(
                        soundEffectPath,
                        SelectAudioType(soundName)
                    );
                    yield return www.SendWebRequest();
                    soundEffectList.Add(nameId, DownloadHandlerAudioClip.GetContent(www));
                    break;
                }
            }

            debugConsole.PrintLog(
                $"Load {soundType}: <#00ff00>{soundName}</color>"
            );
        }

        #endregion


        public void SoundCommand(string soundCommand)
        {
            var l = soundCommand.Split(' ');
            switch (l[0])
            {
                case "bgm":
                    switch (l[1])
                    {
                        case "set":
                            bgm.SetBgm(bgmList[l[2]]);
                            debugConsole.PrintLog(
                                $"Set BGM: <#00ff00>{l[2]}</color>"
                            );
                            break;
                        case "play":
                            bgm.Play();
                            break;
                        case "stop":
                            bgm.Stop();
                            break;
                        case "pause":
                            bgm.Pause();
                            break;
                        case "v":
                            bgm.SetVolume(float.Parse(l[2]));
                            break;
                        case "loop":
                            bgm.Loop();
                            break;
                        case "once":
                            bgm.Once();
                            break;
                        case "down":
                            bgm.Down();
                            break;
                    }

                    break;
                case "se":
                    switch (l[1])
                    {
                        case "set":
                            se.SetSoundEffect(soundEffectList[l[2]]);
                            debugConsole.PrintLog(
                                $"Set SFX: <#00ff00>{l[2]}</color>"
                            );
                            break;
                        case "play":
                            se.Play();
                            break;
                        case "stop":
                            se.Stop();
                            break;
                        case "pause":
                            se.Pause();
                            break;
                        case "v":
                            se.SetVolume(float.Parse(l[2]));
                            break;
                        case "loop":
                            se.Loop();
                            break;
                        case "once":
                            se.Once();
                            break;
                        case "down":
                            se.Down();
                            break;
                    }

                    break;
            }
        }
    }
}
