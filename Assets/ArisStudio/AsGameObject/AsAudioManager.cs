using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArisStudio.AsGameObject.Audio;
using ArisStudio.Core;
using ArisStudio.Utils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;

namespace ArisStudio.AsGameObject
{
    public class AsAudioManager : Singleton<AsAudioManager>
    {
        [field: SerializeField] public AudioMixer AsAudioMixer { get; private set; }
        [SerializeField]
        private GameObject asBgmListBase,
            asSfxListBase;
        private readonly Dictionary<string, AsAudio> audioList = new Dictionary<string, AsAudio>();

        private const float FadeEndVolume = 0f;
        private const float FadeDuration = 1f;

        public void AsAudioInit()
        {
            foreach (var i in audioList)
                Destroy(i.Value.gameObject);
            audioList.Clear();

            DebugConsole.Instance.PrintLog("AsAudioInit");
            // Todo: Init volume with setting menu
        }

        # region Load Audio

        public void LoadAsAudio(string[] asAudioLoadCommand, string audioType)
        {
            StartCoroutine(CreateAsAudio(asAudioLoadCommand[2], asAudioLoadCommand[3], audioType));
        }

        private static AudioType SelectAudioType(string audioName)
        {
            return audioName.Split('.').Last() switch
            {
                "wav" => AudioType.WAV,
                // "mp3" => AudioType.MPEG,
                "ogg" => AudioType.OGGVORBIS,
                _ => AudioType.UNKNOWN
            };
        }

        private IEnumerator CreateAsAudio(string nameId, string audioName, string audioType)
        {
            GameObject audioListBase;
            string audioPath;
            bool isLoop;

            if (audioType == "sfx")
            {
                audioListBase = asSfxListBase;
                audioPath = SettingsManager.Instance.currentSFXPath;
                isLoop = false;
            }
            else
            {
                audioListBase = asBgmListBase;
                audioPath = SettingsManager.Instance.currentBGMPath;
                isLoop = true;
            }

            var audioGo = new GameObject(nameId).AddComponent<AudioSource>();
            audioGo.transform.SetParent(audioListBase.transform);

            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(
                Path.Combine(audioPath, audioName),
                SelectAudioType(audioName)
            );
            yield return www.SendWebRequest();

            var asAudio = AsAudio.GetAsAudio(audioGo.gameObject);
            asAudio.AsAudioInit(DownloadHandlerAudioClip.GetContent(www), nameId, isLoop);

            audioList.Add(nameId, asAudio);

            DebugConsole.Instance.PrintLoadLog(audioType, audioName, nameId);
        }

        # endregion

        public void AsAudioCommand(string[] asAudioCommand)
        {
            switch (asAudioCommand[2])
            {
                case "play":
                    audioList[asAudioCommand[1]].Play();
                    break;
                case "pause":
                    audioList[asAudioCommand[1]].Pause();
                    break;
                case "stop":
                    audioList[asAudioCommand[1]].Stop();
                    break;

                case "v":
                case "volume":
                    audioList[asAudioCommand[1]].SetVolume(float.Parse(asAudioCommand[3]));
                    break;

                case "fade":
                    audioList[asAudioCommand[1]].Fade(
                        ArrayHelper.IsIndexInRange(asAudioCommand, 3)
                            ? float.Parse(asAudioCommand[3])
                            : FadeEndVolume,
                        ArrayHelper.IsIndexInRange(asAudioCommand, 4)
                            ? float.Parse(asAudioCommand[4])
                            : FadeDuration
                    );

                    break;

                case "loop":
                    audioList[asAudioCommand[1]].Loop();
                    break;
                case "once":
                    audioList[asAudioCommand[1]].Once();
                    break;
            }
        }
    }
}
