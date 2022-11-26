using Spine.Unity;
using Spine;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;
using static C_Setting;

public class C_Control : MonoBehaviour
{
    public GameObject sprBase,lableGo,bannerGo,txtGo, selectButtonGo;
    public AudioSource bgmGo,seGo;
    public RawImage bgGo;

    //Run
    bool isAuto,isClick,isBanner,txtTyping,selecting;
    int lineIndex, textLength;
    float autoTimer=0;

    string[] texts;

    // Setting
    string settingJson;
    C_Setting setting;
    // FolderPath
    string dataFolderPath, settingFolderPath, sprFolderPath, bgmFolderPath, seFolderPath,backgroundFolderPath, txtFolderPathPath;

    Dictionary<string, SkeletonAnimation> sprList = new Dictionary<string, SkeletonAnimation>();
    Dictionary<string, AudioClip> bgmList = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> seList = new Dictionary<string, AudioClip>();
    Dictionary<string, Texture2D> backgroundList = new Dictionary<string, Texture2D>();
    Dictionary<string, int> targetList = new Dictionary<string, int>();

    IEnumerator Start()
    {
        dataFolderPath = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "Data");
        settingFolderPath = Path.Combine(dataFolderPath, "setting.json");
        sprFolderPath = Path.Combine(dataFolderPath, "Spr");
        bgmFolderPath = Path.Combine(dataFolderPath, "Bgm");
        seFolderPath= Path.Combine(dataFolderPath, "SE");
        backgroundFolderPath = Path.Combine(dataFolderPath, "Image", "Background");

        using (UnityWebRequest uwr = UnityWebRequest.Get(settingFolderPath))
        {
            yield return uwr.SendWebRequest();
            settingJson = uwr.downloadHandler.text;
        }

        setting = JsonUtility.FromJson<C_Setting>(settingJson);

        isAuto = setting.auto.enable;

        txtFolderPathPath = Path.Combine(dataFolderPath, "0Txt", setting.txtName + ".txt");

        using (UnityWebRequest uwr = UnityWebRequest.Get(txtFolderPathPath))
        {
            yield return uwr.SendWebRequest();
            texts = uwr.downloadHandler.text.Split('\n');
        }

        textLength=texts.Length;

        PreLoad(setting, texts);

        Debug.Log("Finsh load");
    }

    void Update()
    {
        if (isAuto)
        {
            autoTimer += Time.deltaTime;
            if (autoTimer > 1)
            {
                autoTimer= 0;
                isClick = true;
            }
        }

        if (txtTyping)
        {
            autoTimer = 0;
            isClick= false;
            return;
        }

        if (selecting)
        {
            isClick = false;
            return;
        }

        if (isClick)
        {
            if (isBanner)
            {
                bannerGo.SetActive(false);
            }

            if (lineIndex < textLength)
            {
                //Debug.Log("Run Line: " + lineIndex);
                RunPlayer(lineIndex);
                lineIndex++;
            }
        }
    }

    public void SetClick(bool b)
    {
        isClick = b;
    }

    public void SetTxtTyping(bool b)
    {
        txtTyping = b;
    }

    public void SetSelecting(string t)
    {
        Debug.Log(t);
        lineIndex = targetList[t];
        selecting = false;
    }




    IEnumerator LoadAndCreateSprGameObject(string nameId, string sprName)
    {
        string atlasTxt;
        byte[] imageData, skelData;

        GameObject sprBaseGo = Instantiate(sprBase);
        GameObject sprGo = sprBaseGo.transform.Find("Spr").gameObject;

        sprBaseGo.name = nameId;

        string sprPath = Path.Combine(sprFolderPath, sprName);
        string atlasPath = sprPath + ".atlas";
        string skelPath = sprPath + ".skel";

        using (UnityWebRequest uwr = UnityWebRequest.Get(atlasPath))
        {
            yield return uwr.SendWebRequest();
            atlasTxt = uwr.downloadHandler.text;
        }
        TextAsset atlasTextAsset = new TextAsset(atlasTxt);

        using (UnityWebRequest uwr = UnityWebRequest.Get(sprPath + ".png"))
        {
            yield return uwr.SendWebRequest();
            imageData = uwr.downloadHandler.data;
        }
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(imageData);
        texture.name = sprName;
        Texture2D[] textures = new Texture2D[1];
        textures[0] = texture;

        SpineAtlasAsset SprAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, textures, Shader.Find("SFill"), true);

        AtlasAttachmentLoader attachmentLoader = new AtlasAttachmentLoader(SprAtlasAsset.GetAtlas());
        SkeletonBinary binary = new SkeletonBinary(attachmentLoader);
        binary.Scale *= 0.012f;

        using (UnityWebRequest uwr = UnityWebRequest.Get(skelPath))
        {
            yield return uwr.SendWebRequest();
            skelData = uwr.downloadHandler.data;
        }

        SkeletonData skeletonData = binary.ReadSkeletonData(sprName, skelData);
        AnimationStateData stateData = new AnimationStateData(skeletonData);
        SkeletonDataAsset sprSkeletonDataAsset = SkeletonDataAsset.CreateSkeletonDataAsset(skeletonData, stateData);
        SkeletonAnimation sprAnim = SkeletonAnimation.AddToGameObject(sprGo, sprSkeletonDataAsset);

        sprAnim.Initialize(false);
        sprAnim.Skeleton.SetSlotsToSetupPose();
        sprAnim.AnimationState.SetAnimation(0, "Idle_01", true);
        sprGo.SetActive(false);

        sprList.Add(nameId, sprAnim);
    }

    IEnumerator LoadBgm(string nameId, string bgmName)
    {
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(Path.Combine(bgmFolderPath, bgmName), AudioType.UNKNOWN))
        {
            yield return uwr.SendWebRequest();
            bgmList.Add(nameId, DownloadHandlerAudioClip.GetContent(uwr));
        }
    }

    IEnumerator LoadBackground(string nameId, string bgName)
    {
        byte[] bgData;
        Texture2D texture = new Texture2D(1, 1);

        using (UnityWebRequest uwr = UnityWebRequest.Get(Path.Combine(backgroundFolderPath, bgName)))
        {
            yield return uwr.SendWebRequest();
            bgData = uwr.downloadHandler.data;
        }

        texture.LoadImage(bgData);
        texture.name = nameId;
        backgroundList.Add(nameId, texture);
    }

    IEnumerator LoadSe(string nameId, string bgmName)
    {
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(Path.Combine(seFolderPath, bgmName), AudioType.UNKNOWN))
        {
            yield return uwr.SendWebRequest();
            seList.Add(nameId, DownloadHandlerAudioClip.GetContent(uwr));
        }
    }

    void PreLoad(C_Setting pSetting, string[] pTexts)
    {
        //Setting
        foreach (sprPre s in pSetting.spr.sprPreList)
        {
            Debug.Log("pre load spr: " + s.nameId);
            StartCoroutine(LoadAndCreateSprGameObject(s.nameId, s.sprName));
        }

        //Text
        int iTmp = 0;
        foreach (string text in pTexts)
        {
            string t = text.Trim();
            string[] l = t.Split(' ');
            switch (l[0])
            {
                // Load spr,bgm,bg(Background),se(Sound Effect)
                case "load":
                    {
                        if (l[1] == "spr")
                        {
                            StartCoroutine(LoadAndCreateSprGameObject(l[2], l[3]));
                        }
                        else if (l[1] == "bgm")
                        {
                            StartCoroutine(LoadBgm(l[2], l[3]));
                        }
                        else if (l[1] == "bg")
                        {
                            StartCoroutine(LoadBackground(l[2], l[3]));
                        }
                        else if (l[1] == "se")
                        {
                            StartCoroutine(LoadSe(l[2], l[3]));
                        }
                        else if (l[1] == "end")
                        {
                            lineIndex = iTmp;
                            Debug.Log("Load finish at " + lineIndex);
                            isClick = false;
                        }
                        break;
                    }

                case "target":
                    {
                        targetList.Add(l[1], iTmp); 
                        break;
                    }
            }
            iTmp++;
        }
    }

    //Run txt
    void RunPlayer(int li)
    {
        string lt = texts[li].Trim();

        if (lt.StartsWith("="))
        {
            isClick = false;
        }
        else if (lt != "" && !lt.StartsWith("//"))
        {
            string[] l = lt.Split(' ');
            switch (l[0])
            {
                case "jump":
                    {
                        lineIndex = targetList[l[1]];
                        Debug.Log("Jump to: "+lineIndex);
                        break;
                    }

                // Front
                case "label":
                    {
                        lableGo.GetComponent<C_Label>().SetLabelText(l[1]);
                        break;
                    }
                case "banner":
                    {
                        bannerGo.GetComponent<C_Banner>().SetBannerText(l[1]);
                        isBanner=true;
                        isClick = false;
                        break;
                    }
                case "txt":
                    {
                        txtGo.GetComponent<C_Text>().SetTxt(l[1], l[2], lt.Split('"')[1]);
                        txtTyping = true;
                        isClick = false;
                        break;
                    }
                case "txtSize":
                    {
                        txtGo.GetComponent<C_Text>().SetTxtSize(l[1]);
                        break;
                    }
                case "button":
                    {
                        selectButtonGo.GetComponent<C_SelectButton>().SetSelectButton(lt);
                        selecting = true;
                        isClick = false;
                        break;
                    }

                // Bgm
                case "bgm":
                    {
                        if (l[1] == "set")
                        {
                            bgmGo.GetComponent<C_Bgm>().SetBgm(bgmList[l[2]]);
                        }
                        else if (l[1] == "play")
                        {
                            bgmGo.GetComponent<C_Bgm>().Play();
                        }
                        else if (l[1] == "pause")
                        {
                            bgmGo.GetComponent<C_Bgm>().Pause();
                        }
                        else if (l[1] == "stop")
                        {
                            bgmGo.GetComponent<C_Bgm>().Stop();
                        }
                        break;
                    }

                // SE
                case "se":
                    {
                        if (l[1] == "set")
                        {
                            seGo.GetComponent<C_SE>().SetSE(seList[l[2]]);
                        }
                        else if (l[1] == "play")
                        {
                            seGo.GetComponent<C_SE>().Play();
                        }
                        else if (l[1] == "pause")
                        {
                            seGo.GetComponent<C_SE>().Pause();
                        }
                        else if (l[1] == "stop")
                        {
                            seGo.GetComponent<C_SE>().Stop();
                        }
                        else if (l[1] == "pre")
                        {
                            seGo.GetComponent<C_SE>().PlayPre(l[2]);
                        }
                        break;
                    }

                // Background
                case "bg":
                    {
                        if (l[1] == "set")
                        {
                            bgGo.GetComponent<C_Bg>().SetBg(backgroundList[l[2]]);
                        }
                        else if (l[1] == "show")
                        {
                            bgGo.GetComponent<C_Bg>().Show();
                        }
                        else if (l[1] == "hide")
                        {
                            bgGo.GetComponent<C_Bg>().Hide();
                        }
                        else if (l[1] == "black")
                        {
                            bgGo.GetComponent<C_Bg>().Black();
                        }
                        break;
                    }

                // Spr
                case "spr":
                    {
                        if (l[1] == "show")
                        {
                            sprList[l[2]].GetComponent<C_Spr>().Show();
                        }
                        else if (l[1] == "hide")
                        {
                            sprList[l[2]].GetComponent<C_Spr>().Hide();
                        }
                        else if (l[1] == "highlight"|| l[1] == "hl")
                        {
                            sprList[l[2]].GetComponent<C_Spr>().HighLight(l[3]);
                        }
                        else if (l[1] == "state")
                        {
                            sprList[l[2]].GetComponent<C_Spr>().SetState(l[3]);
                        }
                        //Emoticon
                        else if (l[1] == "emoticon"|| l[1] == "emo")
                        {
                            sprList[l[2]].GetComponent<C_SprEmo>().PlayEmoticon(l[3]);
                        }
                        //Animation
                        else if (l[1] == "animInit")
                        {
                            sprList[l[2]].GetComponent<C_SprAnimation>().AnimInit();
                        }
                        else if (l[1] == "down")
                        {
                            sprList[l[2]].GetComponent<C_SprAnimation>().Down();
                        }
                        else if (l[1] == "up")
                        {
                            sprList[l[2]].GetComponent<C_SprAnimation>().Up();
                        }
                        // Move
                        else if (l[1] == "x")
                        {
                            sprList[l[2]].GetComponent<C_SprMove>().SetX(l[3]);
                        }
                        else if (l[1] == "move")
                        {
                            sprList[l[2]].GetComponent<C_SprMove>().Move(l[3], l[4]);
                        }
                        else if (l[1] == "close")
                        {
                            sprList[l[2]].GetComponent<C_SprMove>().Close();
                        }
                        else if (l[1] == "back")
                        {
                            sprList[l[2]].GetComponent<C_SprMove>().Back();
                        }
                        else if (l[1] == "preShake")
                        {
                            sprList[l[2]].GetComponent<C_SprAnimation>();
                        }
                        else if (l[1] == "shakeX")
                        {
                            sprList[l[2]].GetComponent<C_SprMove>().ShakeX(l[3], l[4], l[5]);
                        }
                        else if (l[1] == "shakeY")
                        {
                            sprList[l[2]].GetComponent<C_SprMove>().ShakeY(l[3], l[4], l[5]);
                        }

                        break;
                    }
            }
        }
    }
}
