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
        Dictionary<string, AudioClip> sfxList = new Dictionary<string, AudioClip>();

        // DebugConsole debugConsole;

        void Awake()
        {
        }

        void Start()
        {
            // debugConsole = DebugConsole.Instance;
        }

        public void Initialize()
        {
            bgmList.Clear();
            bgm.Stop();
            bgm.ChangeVolume();

            sfxList.Clear();
            se.Stop();
        }

        # region Load Sound

        public void Sound_LoadCommand(string[] soundLoadCommand)
        {
            switch (soundLoadCommand[1])
            {
                case "bgm":
                    LoadSound(soundLoadCommand[2], soundLoadCommand[3], "bgm");
                    break;

                case "sfx":
                    LoadSound(soundLoadCommand[2], soundLoadCommand[3], "sfx");
                    break;
            }
        }

        private void LoadSound(string nameId, string soundName, string soundType)
        {
            StartCoroutine(CreatAudioClip(nameId, soundName, soundType));
        }

        private AudioType SelectAudioType(string soundName)
        {
            return soundName.EndsWith(".ogg") ? AudioType.OGGVORBIS : soundName.EndsWith(".wav") ? AudioType.WAV : AudioType.UNKNOWN;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator CreatAudioClip(string nameId, string soundName, string soundType)
        {
            var bgmPath = Path.Combine(SettingsManager.Instance.currentBGMPath, soundName);
            var www = UnityWebRequestMultimedia.GetAudioClip(bgmPath, SelectAudioType(soundName));
            yield return www.SendWebRequest();

            switch (soundType)
            {
                case "bgm":
                {
                    bgmList.Add(nameId, DownloadHandlerAudioClip.GetContent(www));
                    break;
                }
                case "sfx":
                {
                    sfxList.Add(nameId, DownloadHandlerAudioClip.GetContent(www));
                    break;
                }
            }

            DebugConsole.Instance.PrintLoadLog(soundType, soundName, nameId);
        }

        #endregion

        public void SoundCommand(string[] soundCommand)
        {
        }


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
                            DebugConsole.Instance.PrintLog(
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
                            se.SetSoundEffect(sfxList[l[2]]);
                            DebugConsole.Instance.PrintLog(
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
