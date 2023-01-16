using System;
using UnityEngine;

namespace ArisStudio.ScreenEffect
{
    public class ScreenEffectFactory : MonoBehaviour
    {
        [Header("Curtain")] public Curtain curtain;
        [Header("SpeedLine")] public SpeedLine speedLine;

        [Header("Smoke")] public GameObject smokeFront;
        public GameObject smokeBack;

        [Header("Dust")] public GameObject dust;
        [Header("Snow")] public GameObject snow;
        [Header("rain")] public GameObject rain;


        private bool sl;
        private float slTimer;
        private float slSeconds;

        private void Update()
        {
            if (!sl) return;
            slTimer += Time.deltaTime;

            if (slTimer < slSeconds) return;

            slTimer = 0;
            speedLine.enabled = false;
            sl = false;
        }

        public void Initialize()
        {
            curtain.HideD();
            speedLine.enabled = false;
            smokeFront.SetActive(false);
            smokeBack.SetActive(false);
            dust.SetActive(false);
            snow.SetActive(false);
            rain.SetActive(false);
        }
        
        public void ScreenEffectCommand(string sec)
        {
            var l = sec.Split(' ');
            switch (l[0])
            {
                #region Old ScreenEffect Command

                case "smoke":
                    switch (l[1])
                    {
                        case "show":
                            smokeBack.SetActive(true);
                            smokeFront.SetActive(true);
                            break;
                        case "hide":
                            smokeBack.SetActive(false);
                            smokeFront.SetActive(false);
                            break;
                    }

                    break;

                case "dust":
                    switch (l[1])
                    {
                        case "show":
                            dust.SetActive(true);
                            break;
                        case "hide":
                            dust.SetActive(false);
                            break;
                    }

                    break;

                case "snow":
                    switch (l[1])
                    {
                        case "show":
                            snow.SetActive(true);
                            break;
                        case "hide":
                            snow.SetActive(false);
                            break;
                    }

                    break;

                case "rain":
                    switch (l[1])
                    {
                        case "show":
                            rain.SetActive(true);
                            break;
                        case "hide":
                            rain.SetActive(false);
                            break;
                    }

                    break;

                case "speedline":
                    switch (l[1])
                    {
                        case "show":
                            speedLine.enabled = true;
                            break;
                        case "hide":
                            speedLine.enabled = false;
                            break;
                        case "s":
                            slTimer = 0;
                            slSeconds = float.Parse(l[2]);
                            speedLine.enabled = true;
                            sl = true;
                            break;
                    }

                    break;

                #endregion

                case "curtain":
                    switch (l[1])
                    {
                        case "show":
                            curtain.Show();
                            break;
                        case "hide":
                            curtain.Hide();
                            break;
                        case "showD":
                            curtain.ShowD();
                            break;
                        case "hideD":
                            curtain.HideD();
                            break;
                        case "black":
                            curtain.Black();
                            break;
                        case "white":
                            curtain.White();
                            break;
                        case "red":
                            curtain.Red();
                            break;
                    }

                    break;

                case "screen":
                    switch (l[1])
                    {
                        case "smoke":
                            switch (l[2])
                            {
                                case "show":
                                    smokeBack.SetActive(true);
                                    smokeFront.SetActive(true);
                                    break;
                                case "hide":
                                    smokeBack.SetActive(false);
                                    smokeFront.SetActive(false);
                                    break;
                            }

                            break;

                        case "dust":
                            switch (l[2])
                            {
                                case "show":
                                    dust.SetActive(true);
                                    break;
                                case "hide":
                                    dust.SetActive(false);
                                    break;
                            }

                            break;

                        case "snow":
                            switch (l[2])
                            {
                                case "show":
                                    snow.SetActive(true);
                                    break;
                                case "hide":
                                    snow.SetActive(false);
                                    break;
                            }

                            break;

                        case "rain":
                            switch (l[2])
                            {
                                case "show":
                                    rain.SetActive(true);
                                    break;
                                case "hide":
                                    rain.SetActive(false);
                                    break;
                            }

                            break;

                        case "speedline":
                            switch (l[2])
                            {
                                case "show":
                                    speedLine.enabled = true;
                                    break;
                                case "hide":
                                    speedLine.enabled = false;
                                    break;
                                case "s":
                                    slTimer = 0;
                                    slSeconds = float.Parse(l[3]);
                                    speedLine.enabled = true;
                                    sl = true;
                                    break;
                            }

                            break;
                    }

                    break;
            }
        }
    }
}