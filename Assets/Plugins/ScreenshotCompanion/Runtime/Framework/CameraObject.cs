﻿using System;
using UnityEngine;

namespace ScreenshotCompanionFramework
{
    [Serializable]
    public class CameraObject
    {
        public GameObject cam;

        [HideInInspector]
        public bool deleteQuestion = false;
        public KeyCode hotkey = KeyCode.None;
    }
}
