using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Play : MonoBehaviour, IPointerClickHandler
{
    public RawImage background;
    public AudioSource bgm;
    public GameObject txt, label, banner;
    public Button btn1, btn2, btn3, coverBtn;

    bool isAuto = false;
    float autoT;

    bool isClick = false;
    bool endClick;
    int lineNum;

    bool isSelect = false;
    bool isBanner = false;

    string[] txts;
    string dataPath, txtPath, bgmPath, backgroundPath, jsonPath;

    Setting setting;

    Dictionary<string, SkeletonAnimation> sprList = new Dictionary<string, SkeletonAnimation>();
    Dictionary<string, AudioClip> bgmList = new Dictionary<string, AudioClip>();
    Dictionary<string, Texture2D> backgroundList = new Dictionary<string, Texture2D>();

    //web
    string settingJson;

    IEnumerator Start()
    {
        dataPath = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "Data");
        jsonPath = Path.Combine(dataPath, "setting.json");

        using (UnityWebRequest uwr = UnityWebRequest.Get(jsonPath))
        {
            yield return uwr.SendWebRequest();
            settingJson = uwr.downloadHandler.text;
        }

        setting = JsonUtility.FromJson<Setting>(settingJson);
        isAuto = setting.auto.enable;

        txtPath = Path.Combine(dataPath, "0Txt", setting.txtName + ".txt");
        bgmPath = Path.Combine(dataPath, "Bgm");
        backgroundPath = Path.Combine(dataPath, "Image", "Background");

        using (UnityWebRequest uwr = UnityWebRequest.Get(txtPath))
        {
            yield return uwr.SendWebRequest();
            txts = uwr.downloadHandler.text.Split('\n');
        }

        endClick = true;
        lineNum = 0;
        autoT = 0;
    }

    public void SetLine(int n)
    {
        lineNum = n - 1;
        isSelect = true;
        endClick = true;
        btn1.gameObject.SetActive(false);
        btn2.gameObject.SetActive(false);
        btn3.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isAuto)
        {
            if (!isClick && endClick)
            {
                autoT += Time.deltaTime;
                if (autoT > setting.auto.seconds)
                {
                    autoT = 0;
                    SetClick();
                }
            }
        }

        if (isSelect)
        {
            if (endClick)
            {
                isSelect = false;
                coverBtn.gameObject.SetActive(false);
                isClick = true;
            }
        }
        else
        {
            if (lineNum < txts.Length)
            {
                if (isClick)
                {
                    if (txt.GetComponent<Txt>().IsTxtActive())
                    {
                        txt.GetComponent<Txt>().PlayTxtAll();
                        isClick = false;
                    }
                    else
                    {
                        TxtPlay(lineNum);
                        lineNum++;
                    }
                }
            }
            else
            {

            }
        }
    }

    void SetClick()
    {
        if (isBanner)
        {
            banner.SetActive(false);
            isBanner = false;
        }
        isClick = true;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        SetClick();
    }

    void TxtPlay(int lineNum)
    {
        string line = txts[lineNum].Trim();
        if (line.StartsWith("="))
        {
            isClick = false;
        }
        else if (line != "" && !line.StartsWith("//"))
        {
            string[] l = line.Split('&');
            switch (l[0])
            {
                case "Jump":
                    {
                        lineNum = int.Parse(l[1]);
                        break;
                    }

                // Front
                case "Label":
                    {
                        label.GetComponent<Label>().SetLabelTxt(l[1]);
                        break;
                    }
                case "Banner":
                    {
                        banner.GetComponent<Banner>().SetBannerTxt(l[1]);
                        isBanner = true;
                        isClick = false;
                        break;
                    }
                case "Button":
                    {
                        coverBtn.gameObject.SetActive(true);

                        if (l.Length == 3)
                        {
                            btn2.GetComponent<SelectButton>().SetButton(l[1], l[2]);
                        }
                        else if (l.Length == 5)
                        {
                            btn1.GetComponent<SelectButton>().SetButton(l[1], l[2]);
                            btn2.GetComponent<SelectButton>().SetButton(l[3], l[4]);
                        }
                        else if (l.Length == 7)
                        {
                            btn1.GetComponent<SelectButton>().SetButton(l[1], l[2]);
                            btn2.GetComponent<SelectButton>().SetButton(l[3], l[4]);
                            btn3.GetComponent<SelectButton>().SetButton(l[5], l[6]);
                        }
                        isSelect = false;
                        isClick = false;
                        endClick = false;
                        break;
                    }

                //Txt
                case "Txt":
                    {
                        txt.GetComponent<Txt>().SetTxt(l[1], l[2], l[3]);
                        isClick = false;
                        break;
                    };
                case "TxtHide":
                    {
                        txt.SetActive(false);
                        break;
                    };

                //Bgm
                case "LoadBgm":
                    {
                        StartCoroutine(LoadBgm(l[1], l[2]));
                        break;
                    };
                case "SetBgm":
                    {
                        SetBgm(l[1]);
                        break;
                    };

                //Background
                case "LoadBg":
                    {
                        StartCoroutine(LoadBackground(l[1], l[2]));
                        break;
                    };
                case "SetBg":
                    {
                        background.texture = backgroundList[l[1]];
                        background.gameObject.SetActive(true);
                        break;
                    };
                case "BgHide":
                    {
                        background.gameObject.SetActive(false);
                        break;
                    }

                // Spr 
                case "LoadSpr":
                    {
                        StartCoroutine(CreateNewSpineGameObject(l[1], l[2]));
                        break;
                    }

                case "SprHL":
                    {
                        sprList[l[1]].GetComponent<SprState>().HighLight(l[2]);
                        break;
                    }
                case "SprState":
                    {
                        sprList[l[1]].GetComponent<SprState>().SetState(l[2]);
                        break;
                    }
                case "SprShow":
                    {
                        sprList[l[1]].gameObject.SetActive(true);
                        break;
                    }
                case "SprHide":
                    {
                        sprList[l[1]].GetComponent<SprState>().Hide();
                        break;
                    }
                case "SprClose":
                    {
                        sprList[l[1]].GetComponent<SprState>().Close();
                        break;
                    }
                case "SprBack":
                    {
                        sprList[l[1]].GetComponent<SprState>().Back();
                        break;
                    }
                case "SprMove":
                    {
                        sprList[l[1]].GetComponent<SprState>().Move(l[2], l[3]);
                        break;
                    }
                case "SprX":
                    {
                        sprList[l[1]].GetComponent<SprState>().SetPosition(l[2]);
                        break;
                    }
                case "SprShakeX":
                    {
                        sprList[l[1]].GetComponent<SprState>().ShakeX(l[2], l[3], l[4]);
                        break;
                    }
                case "SprShakeY":
                    {
                        sprList[l[1]].GetComponent<SprState>().ShakeY(l[2], l[3], l[4]);
                        break;
                    }
                case "SprEmo":
                    {
                        sprList[l[1]].GetComponent<SprState>().PlayEmoticon(l[2]);
                        break;
                    }
            }
        }

    }

    IEnumerator LoadBgm(string nameId, string bgmName)
    {
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(Path.Combine(bgmPath, bgmName), AudioType.UNKNOWN))
        {
            yield return uwr.SendWebRequest();
            bgmList.Add(nameId, DownloadHandlerAudioClip.GetContent(uwr));
        }
    }
    void SetBgm(string nameId)
    {
        bgm.clip = bgmList[nameId];
        bgm.loop = true;
        bgm.Play();
    }

    IEnumerator LoadBackground(string nameId, string bgName)
    {
        byte[] bgData;
        Texture2D texture = new Texture2D(1, 1);

        using (UnityWebRequest uwr = UnityWebRequest.Get(Path.Combine(backgroundPath, bgName)))
        {
            yield return uwr.SendWebRequest();
            bgData = uwr.downloadHandler.data;
        }

        texture.LoadImage(bgData);
        texture.name = nameId;
        backgroundList.Add(nameId, texture);
    }

    IEnumerator CreateNewSpineGameObject(string nameId, string sprName)
    {
        SpineAtlasAsset runtimeAtlasAsset;
        SkeletonDataAsset runtimeSkeletonDataAsset;
        SkeletonAnimation runtimeSkeletonAnimation;

        string atlasPath, skelPath, sprPath, atlasTxt;
        byte[] imageData, bgData, skelData;

        sprPath = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "Data", "Spr", sprName);
        atlasPath = sprPath + ".atlas.prefab";
        skelPath = sprPath + ".skel.prefab";

        using (UnityWebRequest uwr = UnityWebRequest.Get(atlasPath))
        {
            yield return uwr.SendWebRequest();
            atlasTxt = uwr.downloadHandler.text;
        }

        TextAsset atlasTextAsset = new TextAsset(atlasTxt);

        Texture2D[] textures = new Texture2D[1];
        Texture2D texture = new Texture2D(1, 1);

        using (UnityWebRequest uwr = UnityWebRequest.Get(sprPath + ".png"))
        {
            yield return uwr.SendWebRequest();
            imageData = uwr.downloadHandler.data;
        }

        texture.LoadImage(imageData);
        texture.name = sprName;
        textures[0] = texture;

        runtimeAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, textures, Shader.Find("SFill"), true);

        AtlasAttachmentLoader attachmentLoader = new AtlasAttachmentLoader(runtimeAtlasAsset.GetAtlas());
        SkeletonBinary binary = new SkeletonBinary(attachmentLoader);

        binary.Scale *= 0.012f;

        using (UnityWebRequest uwr = UnityWebRequest.Get(skelPath))
        {
            yield return uwr.SendWebRequest();
            skelData = uwr.downloadHandler.data;
        }

        SkeletonData skeletonData = binary.ReadSkeletonData(sprName, skelData);

        AnimationStateData stateData = new AnimationStateData(skeletonData);

        runtimeSkeletonDataAsset = SkeletonDataAsset.CreateSkeletonDataAsset(skeletonData, stateData);
        runtimeSkeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject(nameId, runtimeSkeletonDataAsset);

        runtimeSkeletonAnimation.Initialize(false);
        runtimeSkeletonAnimation.Skeleton.SetSlotsToSetupPose();
        runtimeSkeletonAnimation.AnimationState.SetAnimation(0, "Idle_01", true);
        //runtimeSkeletonAnimation.AnimationState.SetAnimation(1, "00", false);
        runtimeSkeletonAnimation.transform.Translate(Vector3.down * 9);

        runtimeSkeletonAnimation.gameObject.AddComponent<SprState>();
        runtimeSkeletonAnimation.gameObject.SetActive(false);

        GameObject emotion = Instantiate(GameObject.Find("Emotion"), runtimeSkeletonAnimation.transform);
        sprList.Add(nameId, runtimeSkeletonAnimation);
    }
}
