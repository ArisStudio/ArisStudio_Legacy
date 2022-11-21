using System;

[Serializable]
public class Setting
{
    public string txtName;
    public Auto auto;

    [Serializable]
    public class Auto
    {
        public bool enable;
        public float seconds;

    }
}
