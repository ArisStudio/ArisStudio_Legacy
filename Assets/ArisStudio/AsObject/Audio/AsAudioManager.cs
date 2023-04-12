using System.Collections;
using System.Collections.Generic;
using ArisStudio.Core;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace ArisStudio.Audio
{
    public class AsAudioManager : MonoBehaviour
    {
        [SerializeField] private AsBgm asBgm;
        [SerializeField] private AsSfx asSfx;

        private SettingsManager settingsManager;

        private readonly Dictionary<string, AudioClip> bgmList = new Dictionary<string, AudioClip>();
        private readonly Dictionary<string, AudioClip> sfxList = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            settingsManager = SettingsManager.Instance;
        }

        public void AsAudio_LoadCommand(string[] asAudioLoadCommand, string audioType)
        {
            LoadAudio(asAudioLoadCommand[2], asAudioLoadCommand[3], audioType);
        }

        private void LoadAudio(string nameId, string audioName, string audioType)
        {
            StartCoroutine(CreatAudioClip(nameId, audioName, audioType));
        }

        private static AudioType SelectAudioType(string audioName)
        {
            return audioName.Split('.')[-1] switch
            {
                "wav" => AudioType.WAV,
                // "mp3" => AudioType.MPEG,
                "ogg" => AudioType.OGGVORBIS,
                _ => AudioType.UNKNOWN
            };
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator CreatAudioClip(string nameId, string audioName, string audioType)
        {
            string audioPath;
            Dictionary<string, AudioClip> audioList;

            if (audioType == "sfx")
            {
                audioList = sfxList;
                audioPath = settingsManager.currentSFXPath;
            }
            else
            {
                audioList = bgmList;
                audioPath = settingsManager.currentBGMPath;
            }

            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioPath, SelectAudioType(audioName));
            yield return www.SendWebRequest();

            audioList.Add(nameId, DownloadHandlerAudioClip.GetContent(www));

            DebugConsole.Instance.PrintLoadLog(audioType, audioName, nameId);
        }

        public void AsAudioCommand(string[] asAudioCommand)
        {
            switch (asAudioCommand[0])
            {
                case "bgm":
                    switch (asAudioCommand[1])
                    {
                        case "set":
                            asBgm.SetAudio(bgmList[asAudioCommand[2]]);
                            break;

                        case "play":
                            asBgm.Play();
                            break;
                        case "pause":
                            asBgm.Pause();
                            break;
                        case "stop":
                            asBgm.Stop();
                            break;

                        case "v":
                        case "volume":
                            asBgm.SetVolume(float.Parse(asAudioCommand[2]));
                            break;

                        case "fade":
                            switch (asAudioCommand.Length)
                            {
                                case 2:
                                    asBgm.Fade();
                                    break;
                                case 3:
                                    asBgm.Fade(float.Parse(asAudioCommand[2]));
                                    break;
                                default:
                                    asBgm.Fade(float.Parse(asAudioCommand[2]), float.Parse(asAudioCommand[3]));
                                    break;
                            }

                            break;

                        case "loop":
                            asBgm.Loop();
                            break;
                        case "once":
                            asBgm.Once();
                            break;
                    }

                    break;

                case "sfx":
                    switch (asAudioCommand[1])
                    {
                        case "set":
                            asSfx.SetAudio(sfxList[asAudioCommand[2]]);
                            break;

                        case "play":
                            asSfx.Play();
                            break;
                        case "pause":
                            asSfx.Pause();
                            break;
                        case "stop":
                            asSfx.Stop();
                            break;

                        case "v":
                        case "volume":
                            asSfx.SetVolume(float.Parse(asAudioCommand[2]));
                            break;

                        case "fade":
                            switch (asAudioCommand.Length)
                            {
                                case 2:
                                    asSfx.Fade();
                                    break;
                                case 3:
                                    asSfx.Fade(float.Parse(asAudioCommand[2]));
                                    break;
                                default:
                                    asSfx.Fade(float.Parse(asAudioCommand[2]), float.Parse(asAudioCommand[3]));
                                    break;
                            }

                            break;

                        case "loop":
                            asSfx.Loop();
                            break;
                        case "once":
                            asSfx.Once();
                            break;
                    }

                    break;
            }
        }
    }
}
