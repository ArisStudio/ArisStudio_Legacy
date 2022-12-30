using UnityEngine;

public class C_SpeedLine : MonoBehaviour
{
    bool sl;
    float slTimer = 0;
    float slSeconds = 1;

    // Update is called once per frame
    void Update()
    {
        if (sl)
        {
            slTimer += Time.deltaTime;
            if (slTimer > slSeconds)
            {
                slTimer = 0;
                gameObject.GetComponent<S_SpeedLine>().enabled = false;
                sl = false;
            }
        }
    }

    public void Set(string s)
    {
        slTimer = 0;
        slSeconds =float.Parse(s);
        gameObject.GetComponent<S_SpeedLine>().enabled= true;
        sl = true;
    }
}
