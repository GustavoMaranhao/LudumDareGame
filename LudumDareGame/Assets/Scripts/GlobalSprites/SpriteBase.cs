using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBase : MonoBehaviour
{
    public int speed = 20;
    public float atkDamage = 5f;
    public float atkSpeed = 5f;
    public float health = 20f;
    public float defenseReduction = 0f;

    public int invulnFlashes = 5;
    public float startingFlashCD = 0.5f;

    public float pushbackTimer = 0.2f;

    protected BoxCollider2D boxCollider;
    protected SpriteRenderer spriteRenderer;
    protected Animator spriteAnimator;

    protected Vector2 perpDirection;
    protected float pushbackSpeed;
    protected bool bIsPushBack = false;

    [HideInInspector]
    public bool canAct = true;
    public bool canDamage = true;

    protected bool bIsDead = false;

    protected GameObject healthBar;
    protected GameObject healthBarValue;
    protected float fullHealthLength;

    [HideInInspector]
    public WeaponTriggers baseWeaponObjLeft;
    [HideInInspector]
    public WeaponTriggers baseWeaponObjRight;

    public void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteAnimator = gameObject.GetComponent<Animator>();

        healthBar = transform.Find("HealthBar").gameObject;
        healthBarValue = healthBar.transform.Find("HealthBarValue").gameObject;
        fullHealthLength = healthBarValue.transform.lossyScale.x;

        GetComponent<Rigidbody2D>().freezeRotation = true;

        baseWeaponObjLeft = transform.Find("LeftAttackTrigger").gameObject.GetComponent<WeaponTriggers>();
        baseWeaponObjRight = transform.Find("RightAttackTrigger").gameObject.GetComponent<WeaponTriggers>();
    }

    public void FixedUpdate()
    {
        if (bIsDead && canAct && gameObject != GlobalGameManager.player.gameObject)
        {
            Destroy(gameObject, 0.5f);
        }
    }

    public void ReceiveDamage(float amount, Vector3 pushbackDirection = new Vector3(), float pushbackForce = 0f)
    {
        if (!canDamage) return;

        var newAmount = amount - defenseReduction;
        if (newAmount < 0) newAmount = 0;

        if(!bIsDead) FloatingTextController.CreateFloatingText(newAmount.ToString(), transform);

        if(pushbackForce > 0) SpritePushback(pushbackDirection, pushbackForce);

        health -= newAmount;
        if (health <= 0)
        {
            spriteAnimator.SetBool("bIsWalking", false);
            health = 0;
            bIsDead = true;
        }

        UpdateHealthBar();

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        StartCoroutine(SpriteFlash(sprites, invulnFlashes, startingFlashCD));
    }

    public void UpdateHealthBar(bool updateBackground = false)
    {
        //ScaleAround(healthBarValue, new Vector3(-healthBar.transform.position.x/2, 3.9f/2,0), new Vector3(health / fullHealthLength, 1, 1));
        healthBarValue.transform.localScale = new Vector3(health / fullHealthLength, 1, 1);
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

    protected void SpritePushback(Vector3 pushbackDirection, float pushBackForce)
    {
        bIsPushBack = true;
        //perpDirection = (Vector2)(boxCollider.transform.position - pushbackDirection) * pushBackForce;
        perpDirection = pushbackDirection * pushBackForce;
        perpDirection += (Vector2)boxCollider.transform.position;
        pushbackSpeed = pushBackForce;

        StartCoroutine(SpriteSlideBack());
    }

    IEnumerator SpriteSlideBack()
    {
        yield return new WaitForSeconds(pushbackTimer);
        if (bIsPushBack) bIsPushBack = false;
    }
}
