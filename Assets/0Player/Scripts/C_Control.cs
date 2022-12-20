using Spine.Unity;
using Spine;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class C_Control : MonoBehaviour
{
    public GameObject sprBase, chBase, lableGo, bannerGo, banner2Go, txtGo, selectButtonGo, coverGo, cGo, smokeGo,dustGo, rainGo,snowGo,curtainGo, blurGo, mGo;
    public AudioSource bgmGo, seGo;
    public RawImage bgGo;

    public Text outputText;
    Shader def;
    Shader comm;

    //Run
    bool isAuto, isClick, isBanner, txtTyping, selecting;
    int lineIndex, textLength;
    float autoTimer = 0;
    float autoSeconds = 1.5f;

    string[] texts;

    // Setting
    string settingJson;
    C_Setting setting;
    // FolderPath
    string dataFolderPath, settingFolderPath, sprFolderPath, characterFolderPath, bgmFolderPath, seFolderPath, backgroundFolderPath, coverFolderPath, txtFolderPathPath;

    Dictionary<string, SkeletonAnimation> sprList = new Dictionary<string, SkeletonAnimation>();
    Dictionary<string, AudioClip> bgmList = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> seList = new Dictionary<string, AudioClip>();
    Dictionary<string, Texture2D> backgroundList = new Dictionary<string, Texture2D>();
    Dictionary<string, Texture2D> coverList = new Dictionary<string, Texture2D>();
    Dictionary<string, int> targetList = new Dictionary<string, int>();
    Dictionary<string, GameObject> chList = new Dictionary<string, GameObject>();

    IEnumerator Start()
    {
        def = Shader.Find("SFill");
        comm = Shader.Find("Comm");

        dataFolderPath = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "Data");

        settingFolderPath = Path.Combine(dataFolderPath, "Setting.json");

        using (UnityWebRequest uwr = UnityWebRequest.Get(settingFolderPath))
        {
            yield return uwr.SendWebRequest();
            settingJson = uwr.downloadHandler.text;
        }

        setting = JsonUtility.FromJson<C_Setting>(settingJson);

        isAuto = setting.auto.enable;
        autoSeconds = setting.auto.seconds;
        bgmGo.volume = setting.bgm.volume;
        seGo.volume = setting.se.volume;

        txtFolderPathPath = Path.Combine(dataFolderPath, "0Txt", setting.txtName + ".txt");

        sprFolderPath = Path.Combine(dataFolderPath, "Spr");
        bgmFolderPath = Path.Combine(dataFolderPath, "Bgm");
        seFolderPath = Path.Combine(dataFolderPath, "SE");
        backgroundFolderPath = Path.Combine(dataFolderPath, "Image", "Background");
        coverFolderPath = Path.Combine(dataFolderPath, "Image", "Cover");
        characterFolderPath = Path.Combine(dataFolderPath, "Character");

        using (UnityWebRequest uwr = UnityWebRequest.Get(txtFolderPathPath))
        {
            yield return uwr.SendWebRequest();
            texts = uwr.downloadHandler.text.Split('\n');
        }

        textLength = texts.Length;

        PreLoad(setting, texts);
    }

    void Update()
    {
        if (isAuto)
        {
            autoTimer += Time.deltaTime;
            if (autoTimer > autoSeconds)
            {
                autoTimer = 0;
                isClick = true;
            }
        }

        if (txtTyping)
        {
            autoTimer = 0;
            isClick = false;
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
                isBanner = false;
                bannerGo.SetActive(false);
                banner2Go.SetActive(false);
                blurGo.SetActive(false);
                mGo.SetActive(true);
            }

            if (lineIndex < textLength)
            {
                TryRunPlayer(texts[lineIndex].Trim());
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
        Print("Select: " + t);
        lineIndex = targetList[t];
        selecting = false;
    }

    public void Print(string s)
    {
        outputText.text += s + "\n";
    }

    public void TryRunPlayer(string s)
    {
        try
        {
            RunPlayer(s);
        }
        catch(Exception ex)
        {
            Print("Exception: <color=orange>" + s + "</color>");
            Print(ex.Message);
            Debug.LogException(ex);
        }
        finally
        {
            lineIndex++;
        }
    } 

    IEnumerator LoadAndCreateSprGameObject(string nameId, string sprName,Shader s)
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

        SpineAtlasAsset SprAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, textures,s, true);

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

        sprAnim.GetComponent<C_Spr>().Init();

        sprAnim.Initialize(false);
        sprAnim.Skeleton.SetSlotsToSetupPose();
        try
        {
            sprAnim.AnimationState.SetAnimation(0, "Idle_01", true);
        }
        catch
        {
            sprAnim.AnimationState.SetAnimation(0, "00", true);
        }
        sprGo.SetActive(false);

        Print("Load Spr: <color=cyan>" + nameId + "</color>");

        sprList.Add(nameId, sprAnim);
    }

    AudioType SelectAudioType(string sName)
    {
        if (sName.EndsWith(".ogg"))
        {
            return AudioType.OGGVORBIS;
        }
        else if (sName.EndsWith(".wav"))
        {
            return AudioType.WAV;
        }
        else if (sName.EndsWith(".mp3"))
        {
            return AudioType.MPEG;
        }
        else
        {
            return AudioType.UNKNOWN;
        }
    }

    IEnumerator LoadBgm(string nameId, string bgmName)
    {
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(Path.Combine(bgmFolderPath, bgmName), SelectAudioType(bgmName)))
        {
            yield return uwr.SendWebRequest();
            bgmList.Add(nameId, DownloadHandlerAudioClip.GetContent(uwr));
        }
        Print("Load Bgm: <color=cyan>" + nameId + "</color>");
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
        Print("Load Background: <color=cyan>" + nameId + "</color>");
    }

    IEnumerator LoadCover(string nameId, string coverName)
    {
        byte[] coverData;
        Texture2D texture = new Texture2D(1, 1);

        using (UnityWebRequest uwr = UnityWebRequest.Get(Path.Combine(coverFolderPath, coverName)))
        {
            yield return uwr.SendWebRequest();
            coverData = uwr.downloadHandler.data;
        }

        texture.LoadImage(coverData);
        texture.name = nameId;
        coverList.Add(nameId, texture);
        Print("Load Cover: <color=cyan>" + nameId + "</color>");
    }

    IEnumerator LoadSe(string nameId, string seName)
    {
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(Path.Combine(seFolderPath, seName), SelectAudioType(seName)))
        {
            yield return uwr.SendWebRequest();
            seList.Add(nameId, DownloadHandlerAudioClip.GetContent(uwr));
        }
        Print("Load SE: <color=cyan>" + nameId + "</color>");
    }

    IEnumerator LoadCharacter(string nameId, string chY, string chScale, string chName)
    {
        byte[] chData;
        Texture2D texture = new Texture2D(1, 1);

        GameObject chBaseGo = Instantiate(chBase);
        GameObject chGo = chBaseGo.transform.Find("Character").gameObject;

        chBaseGo.name = nameId;

        using (UnityWebRequest uwr = UnityWebRequest.Get(Path.Combine(characterFolderPath, chName)))
        {
            yield return uwr.SendWebRequest();
            chData = uwr.downloadHandler.data;
        }
        texture.LoadImage(chData);
        texture.name = nameId;

        SpriteRenderer sr = chGo.GetComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        sr.material.shader = Shader.Find("SFill");
        sr.material.SetFloat("_FillPhase", 1);
        sr.material.SetColor("_FillColor", new Color(0, 0, 0, 1));

        chGo.GetComponent<C_Character>().Init();
        chGo.SetActive(false);

        chBaseGo.transform.localPosition = new Vector3(0, float.Parse(chY), 0);
        chBaseGo.transform.localScale = new Vector3(float.Parse(chScale), float.Parse(chScale), 1);

        chGo.GetComponent<C_SprEmo>().InitEmoticon(chY, chScale);

        chList.Add(nameId, chGo);
        Print("Load Character: <color=cyan>" + nameId + "</color>");
    }

    void PreLoad(C_Setting pSetting, string[] pTexts)
    {
        ////Setting
        //foreach (sprPre s in pSetting.spr.sprPreList)
        //{
        //    Debug.Log("pre load spr: " + s.nameId);
        //    StartCoroutine(LoadAndCreateSprGameObject(s.nameId, s.sprName));
        //}

        //foreach (bgmPre b in pSetting.bgm.bgmPreList)
        //{
        //    Debug.Log("pre load bgm: " + b.nameId);
        //    StartCoroutine(LoadBgm(b.nameId, b.bgmName));
        //}

        //foreach (bgPre b in pSetting.bg.bgPreList)
        //{
        //    Debug.Log("pre load bg: " + b.nameId);
        //    StartCoroutine(LoadBackground(b.nameId, b.bgName));
        //}

        //foreach (sePre s in pSetting.se.sePreList)
        //{
        //    Debug.Log("pre load se: " + s.nameId);
        //    StartCoroutine(LoadSe(s.nameId, s.seName));
        //}

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
                            StartCoroutine(LoadAndCreateSprGameObject(l[2], l[3], def));
                        }
                        else if (l[1] == "sprC")
                        {
                            StartCoroutine(LoadAndCreateSprGameObject(l[2], l[3], comm));
                        }
                        else if (l[1] == "bgm")
                        {
                            StartCoroutine(LoadBgm(l[2], l[3]));
                        }
                        else if (l[1] == "bg")
                        {
                            StartCoroutine(LoadBackground(l[2], l[3]));
                        }
                        else if (l[1] == "cover")
                        {
                            StartCoroutine(LoadCover(l[2], l[3]));
                        }
                        else if (l[1] == "se")
                        {
                            StartCoroutine(LoadSe(l[2], l[3]));
                        }
                        else if (l[1] == "ch")
                        {
                            StartCoroutine(LoadCharacter(l[2], l[3], l[4], l[5]));
                        }
                        else if (l[1] == "end")
                        {
                            lineIndex = iTmp;
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
    void RunPlayer(string lt)
    {
        if (lt.StartsWith("="))
        {
            isClick = false;
        }
        else if (lt != "" && !lt.StartsWith("//"))
        {
            string[] l = lt.Split(' ');
            switch (l[0])
            {
                case "load":
                    {
                        if (l[1] == "spr")
                        {
                            StartCoroutine(LoadAndCreateSprGameObject(l[2], l[3], def));
                        }
                        else if (l[1] == "sprC")
                        {
                            StartCoroutine(LoadAndCreateSprGameObject(l[2], l[3], comm));
                        }
                        else if (l[1] == "bgm")
                        {
                            StartCoroutine(LoadBgm(l[2], l[3]));
                        }
                        else if (l[1] == "bg")
                        {
                            StartCoroutine(LoadBackground(l[2], l[3]));
                        }
                        else if (l[1] == "cover")
                        {
                            StartCoroutine(LoadCover(l[2], l[3]));
                        }
                        else if (l[1] == "se")
                        {
                            StartCoroutine(LoadSe(l[2], l[3]));
                        }
                        else if (l[1] == "ch")
                        {
                            StartCoroutine(LoadCharacter(l[2], l[3], l[4], l[5]));
                        }
                        break;
                    }

                case "jump":
                    {
                        lineIndex = targetList[l[1]];
                        Print("Jump to Line: " + lineIndex);
                        break;
                    }

                // Front
                case "label":
                    {
                        lableGo.GetComponent<C_Label>().SetLabelText(lt.Split('\'')[1]);
                        break;
                    }
                case "banner":
                    {
                        mGo.SetActive(false);
                        blurGo.SetActive(true);
                        bannerGo.GetComponent<C_Banner>().SetBannerText(lt.Split('\'')[1]);
                        isBanner = true;
                        isClick = false;
                        break;
                    }
                case "banner2":
                    {
                        mGo.SetActive(false);
                        blurGo.SetActive(true);
                        string[] bt = lt.Split('\'');
                        banner2Go.GetComponent<C_Banner2>().SetBanner2Text(bt[1], bt[3]);
                        isBanner = true;
                        isClick = false;
                        break;
                    }
                case "txt":
                    {
                        string[] tt = lt.Split('\'');
                        txtGo.GetComponent<C_Text>().SetTxt(tt[1], tt[3], tt[5]);
                        txtTyping = true;
                        isClick = false;
                        break;
                    }
                case "txtSize":
                    {
                        txtGo.GetComponent<C_Text>().SetTxtSize(l[1]);
                        break;
                    }
                case "txtHide":
                    {
                        txtGo.SetActive(false);
                        break;
                    }
                case "button":
                    {
                        selectButtonGo.GetComponent<C_SelectButton>().SetSelectButton(lt);
                        selecting = true;
                        isClick = false;
                        break;
                    }
                case "speedline":
                    {
                        cGo.GetComponent<C_SpeedLine>().Set(l[1]);
                        break;
                    }
                case "speedlineShow":
                    {
                        cGo.GetComponent<S_SpeedLine>().enabled = true;
                        break;
                    }
                case "speedlineHide":
                    {
                        cGo.GetComponent<S_SpeedLine>().enabled = false;
                        break;
                    }
                case "smoke":
                    {
                        if (l[1] == "show")
                        {
                            smokeGo.GetComponent<C_Smoke>().Show();
                        }
                        else if (l[1] == "hide")
                        {
                            smokeGo.GetComponent<C_Smoke>().Hide();
                        }
                        break;
                    }
                case "dust":
                    {
                        if (l[1] == "show")
                        {
                            dustGo.SetActive(true);
                        }
                        else if (l[1] == "hide")
                        {
                            dustGo.SetActive(false);
                        }
                        break;
                    }
                case "rain":
                    {
                        if (l[1] == "show")
                        {
                            rainGo.SetActive(true);
                        }
                        else if (l[1] == "hide")
                        {
                            rainGo.SetActive(false);
                        }
                        break;
                    }
                case "snow":
                    {
                        if (l[1] == "show")
                        {
                            snowGo.SetActive(true);
                        }
                        else if (l[1] == "hide")
                        {
                            snowGo.SetActive(false);
                        }
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
                        else if (l[1] == "down")
                        {
                            bgmGo.GetComponent<C_Bgm>().Down();
                        }
                        else if (l[1] == "v")
                        {
                            bgmGo.GetComponent<C_Bgm>().V(l[2]);
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
                        else if (l[1] == "v")
                        {
                            seGo.GetComponent<C_SE>().V(l[2]);
                        }
                        else if (l[1] == "loop")
                        {
                            seGo.GetComponent<C_SE>().Loop();
                        }
                        else if (l[1] == "once")
                        {
                            seGo.GetComponent<C_SE>().Once();
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
                        else if (l[1] == "showD")
                        {
                            bgGo.GetComponent<C_Bg>().ShowD();
                        }
                        else if (l[1] == "hideD")
                        {
                            bgGo.GetComponent<C_Bg>().HideD();
                        }
                        else if (l[1] == "shakeX")
                        {
                            bgGo.GetComponent<C_BgShake>().ShakeX(l[2], l[3], l[4]);
                        }
                        else if (l[1] == "shakeY")
                        {
                            bgGo.GetComponent<C_BgShake>().ShakeY(l[2], l[3], l[4]);
                        }
                        break;
                    }

                // Curtain
                case "curtain":
                    {
                        if (l[1] == "show")
                        {
                            curtainGo.GetComponent<C_Curtain>().Show();
                        }
                        else if (l[1] == "showD")
                        {
                            curtainGo.SetActive(true);
                        }
                        else if (l[1] == "hide")
                        {
                            curtainGo.SetActive(false);
                        }
                        else if (l[1] == "black")
                        {
                            curtainGo.GetComponent<C_Curtain>().Black();
                        }
                        else if (l[1] == "white")
                        {
                            curtainGo.GetComponent<C_Curtain>().White();
                        }
                        break;
                    }

                //Cover
                case "cover":
                    {
                        if (l[1] == "set")
                        {
                            coverGo.GetComponent<C_Cover>().SetCover(coverList[l[2]]);
                        }
                        else if (l[1] == "show")
                        {
                            coverGo.GetComponent<C_Cover>().Show();
                        }
                        else if (l[1] == "hide")
                        {
                            coverGo.GetComponent<C_Cover>().Hide();
                        }
                        break;
                    }

                //Character
                case "ch":
                    {
                        if (l[1] == "show")
                        {
                            chList[l[2]].GetComponent<C_Character>().Show();
                        }
                        else if (l[1] == "hide")
                        {
                            chList[l[2]].GetComponent<C_Character>().Hide();
                        }
                        else if (l[1] == "showD")
                        {
                            chList[l[2]].gameObject.SetActive(true);
                        }
                        else if (l[1] == "hideD")
                        {
                            chList[l[2]].gameObject.SetActive(false);
                        }
                        else if (l[1] == "highlight" || l[1] == "hl")
                        {
                            chList[l[2]].GetComponent<C_Character>().HighLight(l[3]);
                        }

                        //Comm
                        else if (l[1] == "default" || l[1] == "def")
                        {
                            chList[l[2]].GetComponent<C_Character>().Def();
                        }
                        else if (l[1] == "communication" || l[1] == "comm")
                        {
                            chList[l[2]].GetComponent<C_Character>().Comm();
                        }
                        else if (l[1] == "showC")
                        {
                            chList[l[2]].GetComponent<C_Character>().ShowC();
                        }
                        else if (l[1] == "hideC")
                        {
                            chList[l[2]].GetComponent<C_Character>().HideC();
                        }

                        //Emoticon
                        else if (l[1] == "emoticon" || l[1] == "emo")
                        {
                            chList[l[2]].GetComponent<C_SprEmo>().PlayEmoticon(l[3]);
                        }

                        //Animation
                        else if (l[1] == "empty")
                        {
                            chList[l[2]].GetComponent<C_SprAnimation>().Empty();
                        }
                        else if (l[1] == "down")
                        {
                            chList[l[2]].GetComponent<C_SprAnimation>().Down();
                        }
                        else if (l[1] == "up")
                        {
                            chList[l[2]].GetComponent<C_SprAnimation>().Up();
                        }

                        // Position
                        else if (l[1] == "x")
                        {
                            chList[l[2]].GetComponent<C_SprMove>().SetX(l[3]);
                        }
                        else if (l[1] == "move")
                        {
                            chList[l[2]].GetComponent<C_SprMove>().Move(l[3], l[4]);
                        }
                        else if (l[1] == "z")
                        {
                            chList[l[2]].GetComponent<C_SprMove>().SetZ(l[3]);
                        }
                        else if (l[1] == "close")
                        {
                            chList[l[2]].GetComponent<C_SprMove>().Close();
                        }
                        else if (l[1] == "back")
                        {
                            chList[l[2]].GetComponent<C_SprMove>().Back();
                        }
                        else if (l[1] == "shakeX")
                        {
                            chList[l[2]].GetComponent<C_SprMove>().ShakeX(l[3], l[4], l[5]);
                        }
                        else if (l[1] == "shakeY")
                        {
                            chList[l[2]].GetComponent<C_SprMove>().ShakeY(l[3], l[4], l[5]);
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
                        else if (l[1] == "showD")
                        {
                            sprList[l[2]].gameObject.SetActive(true);
                        }
                        else if (l[1] == "hideD")
                        {
                            sprList[l[2]].gameObject.SetActive(false);
                        }
                        else if (l[1] == "highlight" || l[1] == "hl")
                        {
                            sprList[l[2]].GetComponent<C_Spr>().HighLight(l[3]);
                        }
                        else if (l[1] == "state")
                        {
                            sprList[l[2]].GetComponent<C_Spr>().SetState(l[3]);
                        }

                        //Emoticon
                        else if (l[1] == "emoticon" || l[1] == "emo")
                        {
                            sprList[l[2]].GetComponent<C_SprEmo>().PlayEmoticon(l[3]);
                        }

                        //Animation
                        else if (l[1] == "empty")
                        {
                            sprList[l[2]].GetComponent<C_SprAnimation>().Empty();
                        }
                        else if (l[1] == "down")
                        {
                            sprList[l[2]].GetComponent<C_SprAnimation>().Down();
                        }
                        else if (l[1] == "up")
                        {
                            sprList[l[2]].GetComponent<C_SprAnimation>().Up();
                        }

                        // Position
                        else if (l[1] == "x")
                        {
                            sprList[l[2]].GetComponent<C_SprMove>().SetX(l[3]);
                        }
                        else if (l[1] == "move")
                        {
                            sprList[l[2]].GetComponent<C_SprMove>().Move(l[3], l[4]);
                        }
                        else if (l[1] == "z")
                        {
                            sprList[l[2]].GetComponent<C_SprMove>().SetZ(l[3]);
                        }
                        else if (l[1] == "close")
                        {
                            sprList[l[2]].GetComponent<C_SprMove>().Close();
                        }
                        else if (l[1] == "back")
                        {
                            sprList[l[2]].GetComponent<C_SprMove>().Back();
                        }
                        else if (l[1] == "shakeX")
                        {
                            sprList[l[2]].GetComponent<C_SprMove>().ShakeX(l[3], l[4], l[5]);
                        }
                        else if (l[1] == "shakeY")
                        {
                            sprList[l[2]].GetComponent<C_SprMove>().ShakeY(l[3], l[4], l[5]);
                        }

                        //Comm
                        else if (l[1] == "default" || l[1] == "def")
                        {
                            sprList[l[2]].GetComponent<C_Spr>().Def();
                        }
                        else if (l[1] == "communication" || l[1] == "comm")
                        {
                            sprList[l[2]].GetComponent<C_Spr>().Comm();
                        }
                        else if (l[1] == "showC")
                        {
                            sprList[l[2]].GetComponent<C_Spr>().ShowC();
                        }
                        else if (l[1] == "hideC")
                        {
                            sprList[l[2]].GetComponent<C_Spr>().HideC();
                        }
                        break;
                    }
            }
        }
    }
}
