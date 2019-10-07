using UnityEngine;
using UnityEngine.UI;

public class DamagePopText : MonoBehaviour
{
    public Animator textAnimator;
    public Animator healTextAnimator;

    void Start()
    {
        if (textAnimator != null)
        {
            AnimatorClipInfo[] clipInfo = textAnimator.GetCurrentAnimatorClipInfo(0);
            Destroy(gameObject, clipInfo[0].clip.length);
        }

        if (healTextAnimator != null)
        {
            AnimatorClipInfo[] clipInfo = healTextAnimator.GetCurrentAnimatorClipInfo(0);
            Destroy(gameObject, clipInfo[0].clip.length);
        }
    }

    public void SetText(string text)
    {
        textAnimator.GetComponent<Text>().text = text;
    }

    public void SetHealText(string text)
    {
        healTextAnimator.GetComponent<Text>().text = text;
    }
}
