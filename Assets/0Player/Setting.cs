using System;

[Serializable]
public class Setting
{
    public string txtName;

    [Serializable]
    public class Talk
    {
        public bool onlyTalk;
        public float volume;
        public int x;
        public int y;
        public float scale;
        public int n;
    }
}
