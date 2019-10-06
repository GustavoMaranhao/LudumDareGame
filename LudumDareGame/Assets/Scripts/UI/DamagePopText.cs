using UnityEngine;
using UnityEngine.UI;

public class DamagePopText : MonoBehaviour
{
    public Animator textAnimator;

    void Start()
    {
        AnimatorClipInfo[] clipInfo = textAnimator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
    }

    public void SetText(string text)
    {
        textAnimator.GetComponent<Text>().text = text;
    }
}
