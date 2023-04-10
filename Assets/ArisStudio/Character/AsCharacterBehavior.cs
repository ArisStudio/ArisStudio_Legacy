using UnityEngine;

namespace ArisStudio.Character
{
    public class AsCharacterBehavior : MonoBehaviour, IAsCharacter
    {
        [SerializeField] GameObject AsCharacterBase;

        private MaterialPropertyBlock mpb;
        private MeshRenderer md;

        private string materialType;

        public bool IsCommunication { get; set; }

        private static readonly int FillPhase = Shader.PropertyToID("_FillPhase");
        private static readonly int Color1 = Shader.PropertyToID("_Color");


        public void Show()
        {
            throw new System.NotImplementedException();
        }

        public void Hide()
        {
            throw new System.NotImplementedException();
        }

        public void Appear()
        {
            gameObject.SetActive(true);
        }

        public void Disappear()
        {
            gameObject.SetActive(false);
        }

        public void Highlight(float hl)
        {
            mpb = new MaterialPropertyBlock();
            mpb.SetFloat(FillPhase, 1 - hl);
            md.SetPropertyBlock(mpb);
        }

        public void Highlight(float hl, float time)
        {
            throw new System.NotImplementedException();
        }

        public void Position(float x, float y)
        {
            AsCharacterBase.transform.localPosition = new Vector3(x, y, AsCharacterBase.transform.localPosition.z);
        }

        public void Move()
        {
            throw new System.NotImplementedException();
        }

        public void Z(float z)
        {
            AsCharacterBase.transform.localPosition =
                new Vector3(AsCharacterBase.transform.localPosition.x, AsCharacterBase.transform.localPosition.y, 0);
        }

        public void Shake()
        {
            throw new System.NotImplementedException();
        }

        public void Scale(float scale)
        {
            AsCharacterBase.transform.localScale = new Vector3(scale, scale, 1);
        }

        public void Close()
        {
            AsCharacterBase.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            AsCharacterBase.transform.localPosition =
                new Vector3(AsCharacterBase.transform.localPosition.x, AsCharacterBase.transform.localPosition.y - 8.5f,
                    AsCharacterBase.transform.localPosition.z);
        }

        public void Back()
        {
            AsCharacterBase.transform.localScale = Vector3.one;
            AsCharacterBase.transform.localPosition =
                new Vector3(AsCharacterBase.transform.localPosition.x, AsCharacterBase.transform.localPosition.y + 8.5f,
                    AsCharacterBase.transform.localPosition.z);
        }

        public void State()
        {
            throw new System.NotImplementedException();
        }

        public void Emotion()
        {
            throw new System.NotImplementedException();
        }

        public void Animation()
        {
            throw new System.NotImplementedException();
        }
    }
}
