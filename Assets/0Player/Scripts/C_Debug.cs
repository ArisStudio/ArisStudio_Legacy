using UnityEngine;
using UnityEngine.UI;

public class C_Debug : MonoBehaviour
{
    public GameObject con;

    public InputField input;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RunCommand();
            input.ActivateInputField();
        }

    }

    public void RunCommand()
    {
        string t = input.text;
        con.GetComponent<C_Control>().TryRunPlayer(t);
        con.GetComponent<C_Control>().Print("Input: <color=lime>" + t + "</color>");
        input.text = string.Empty;
    }
}
