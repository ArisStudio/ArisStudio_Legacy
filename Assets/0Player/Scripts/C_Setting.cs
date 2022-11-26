using System;
using System.Collections.Generic;

[Serializable]
public class C_Setting
{
    public string txtName;
    public Auto auto;
    public Spr spr;

    [Serializable]
    public class Auto
    {
        public bool enable;
        public float seconds;
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
}
