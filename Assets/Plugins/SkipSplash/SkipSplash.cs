/* ---------------------------------------------------------------- */
/*                    Skip Unity Splash Screen                      */
/*                      Create by psygames                          */
/*            https://github.com/psygames/UnitySkipSplash           */
/* ---------------------------------------------------------------- */

/// <summary>
/// Skip Unity Splash didn't work in Unity 2020 and newer.
/// So, I use my own way to get rid of this.
/// Checkout my repo for more details:
/// https://github.com/kiraio-moe/remove-unity-splash-screen
/// </summary>

#if !UNITY_EDITOR && !UNITY_2020_OR_NEWER
using UnityEngine;
using UnityEngine.Rendering;

public class SkipSplash
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void BeforeSplashScreen()
    {
#if UNITY_WEBGL
        Application.focusChanged += Application_focusChanged;
#else
        System.Threading.Tasks.Task.Run(AsyncSkip);
#endif
    }

#if UNITY_WEBGL
    private static void Application_focusChanged(bool obj)
    {
        Application.focusChanged -= Application_focusChanged;
        SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
    }
#else
    private static void AsyncSkip()
    {
        SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
    }
#endif
}
#endif
