using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBase : MonoBehaviour
{
    public int speed = 20;
    public int atkDamage = 5;
    public int atkSpeed = 5;
    public float health = 20f;

    public int invulnFlashes = 5;
    public float startingFlashCD = 0.5f;

    protected BoxCollider2D boxCollider;
    protected SpriteRenderer spriteRenderer;
    protected Animator spriteAnimator;

    protected bool canAct = true;
    protected bool bIsDead = false;

    private GameObject healthBar;
    private GameObject healthBarValue;
    private float fullHealthLength;

    public void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteAnimator = gameObject.GetComponent<Animator>();

        healthBar = transform.Find("HealthBar").gameObject;
        healthBarValue = healthBar.transform.Find("HealthBarValue").gameObject;
        fullHealthLength = healthBarValue.transform.lossyScale.x;

        GetComponent<Rigidbody2D>().freezeRotation = true;
    }

    public void FixedUpdate()
    {
        if (bIsDead && canAct)
            Destroy(gameObject, 0.5f);

        if (Input.GetKeyUp("v"))
            ReceiveDamage(5);
    }

    public void ReceiveDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            bIsDead = true;
        }

        //ScaleAround(healthBarValue, new Vector3(-healthBar.transform.position.x/2, 3.9f/2,0), new Vector3(health / fullHealthLength, 1, 1));
        healthBarValue.transform.localScale = new Vector3(health / fullHealthLength, 1, 1);

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        StartCoroutine(SpriteFlash(sprites, invulnFlashes, startingFlashCD));
    }

    public void ScaleAround(GameObject target, Vector3 pivot, Vector3 newScale)
    {
        Vector3 A = target.transform.localPosition;
        Vector3 B = pivot;

        Vector3 C = A - B; // diff from object pivot to desired pivot/origin

        float RS = newScale.x / target.transform.localScale.x; // relataive scale factor

        // calc final position post-scale
        Vector3 FP = B + C * RS;

        // finally, actually perform the scale/translation
        target.transform.localScale = newScale;
        target.transform.localPosition = FP;
    }

    IEnumerator SpriteFlash(SpriteRenderer[] sprites, int numTimes, float intialDelay, bool disable = false)
    {
        canAct = false;

        for (int loop = 1; loop <= numTimes; loop++)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i].tag == "UnblinkingSprite") continue;
                if (disable)
                    sprites[i].enabled = false;
                else
                    sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, 0.5f);
            }

            yield return new WaitForSeconds(intialDelay / loop);

            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i].tag == "UnblinkingSprite") continue;
                if (disable)
                    sprites[i].enabled = true;
                else
                    sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, 1);
            }

            yield return new WaitForSeconds(intialDelay / loop);
        }

        canAct = true;
    }
}
