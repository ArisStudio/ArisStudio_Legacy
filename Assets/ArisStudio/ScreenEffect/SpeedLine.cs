using UnityEngine;


namespace ArisStudio.ScreenEffect
{
    [ExecuteAlways]
    public class SpeedLine : MonoBehaviour
    {
        public Material material;

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, material);
        }
    }
}