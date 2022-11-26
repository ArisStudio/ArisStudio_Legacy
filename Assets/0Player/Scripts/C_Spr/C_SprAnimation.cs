using UnityEngine;

public class C_SprAnimation : MonoBehaviour
{
    Animator sprAnimator;

    void Start()
    {
        sprAnimator=gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimInit()
    {
        sprAnimator.Play("Empty");
    }

    public void Down()
    {
        sprAnimator.Play("SprDown");
    }

    public void Up()
    {
        sprAnimator.Play("SprUp");
    }
}
