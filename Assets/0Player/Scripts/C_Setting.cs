using System;
using System.Collections.Generic;

[Serializable]
public class C_Setting
{
    public string txtName;
    public Auto auto;
    //public WebGL webGL;
    public Spr spr;
    public Bgm bgm;
    public Se se;
    public Bg bg;

    [Serializable]
    public class Auto
    {
        public bool enable;
        public float seconds;
    }

    [Serializable]
    public class WebGL
    {
        public bool enable;
        public string dataUrl;
    }

    [Serializable]
    public class sprPre
    {
        public string nameId;
        public string sprName;
    }

    [Serializable]
    public class Spr
    {
        public List<sprPre> sprPreList= new List<sprPre>();
    }

    [Serializable]
    public class bgmPre
    {
        public string nameId;
        public string bgmName;
    }

    [Serializable]
    public class Bgm
    {
        public float volume;
        public List<bgmPre> bgmPreList = new List<bgmPre>();
    }

    [Serializable]
    public class sePre
    {
        public string nameId;
        public string seName;
    }

    [Serializable]
    public class Se
    {
        public float volume;
        public List<sePre> sePreList = new List<sePre>();
    }

    [Serializable]
    public class bgPre
    {
        public string nameId;
        public string bgName;
    }

    [Serializable]
    public class Bg
    {
        public List<bgPre> bgPreList = new List<bgPre>();
    }
}
