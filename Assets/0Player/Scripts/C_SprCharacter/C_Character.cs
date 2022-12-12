using UnityEngine;

public class C_Character : MonoBehaviour
{
    Shader def, comm;

    SpriteRenderer sr;
    MaterialPropertyBlock mpb;

    //Show
    float showTime = 0;
    float changeShowTime = 0.4f;

    bool showing, hiding;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (showing)
        {
            showTime += Time.deltaTime;
            if (showTime >= changeShowTime || 1 - showTime / changeShowTime <= 0.05f)
            {
                showTime = 0;
                sr.material.SetFloat("_FillPhase", 0);
                showing = false;
            }
            else
            {
                sr.material.SetFloat("_FillPhase", 1 - showTime / changeShowTime);
            }
        }

        else if (hiding)
        {
            showTime += Time.deltaTime;
            if (showTime >= changeShowTime || showTime / changeShowTime >= 0.95f)
            {
                showTime = 0;
                sr.material.SetFloat("_FillPhase", 1);
                hiding = false;
                gameObject.SetActive(false);
            }
            else
            {
                sr.material.SetFloat("_FillPhase", showTime / changeShowTime);
            }
        }
    }
    public void Init()
    {
        sr = GetComponent<SpriteRenderer>();

        def = Shader.Find("SFill");
        comm = Shader.Find("Comm");
    }

    public void Show()
    {
        showing = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        hiding = true;
    }

    public void HighLight(string f)
    {
        sr.material.SetFloat("_FillPhase", 1 - float.Parse(f));
    }

    public void ShowC()
    {
        Comm();
        gameObject.SetActive(true);
    }

    public void HideC()
    {
        Def();
        gameObject.SetActive(false);
    }

    public void Def()
    {
        sr.material.shader = def;
    }

    public void Comm()
    {
        sr.material.shader = comm;
    }
}

