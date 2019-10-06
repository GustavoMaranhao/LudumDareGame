using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ItemType
{
    Sword,
    Armor,
    Soul
}

public class PlayerControls : SpriteBase
{
    [HideInInspector]
    public bool isPlayerMoving = false;

    private bool hasSword = false;
    [HideInInspector]
    public bool bIsAttacking = false;

    public float dashSpeed = 5f;
    [HideInInspector]
    public bool bShouldDash = false;

    void Start()
    {
        base.Start();

        spriteAnimator.SetBool("bIsGhost", true);
        spriteAnimator.SetTrigger("GoToIdle");
    }

    void FixedUpdate()
    {
        base.FixedUpdate();

        //Only move if not attacking
        if (!spriteAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !bShouldDash)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
                boxCollider.transform.Translate(transform.TransformDirection(input * speed * Time.deltaTime));

                isPlayerMoving = true;

                GetComponent<SpriteRenderer>().flipX = Input.GetAxisRaw("Horizontal") < 0;

                if (!bIsAttacking) spriteAnimator.SetBool("bIsWalking", true);
            }
            else
            {
                isPlayerMoving = false;
                spriteAnimator.SetBool("bIsWalking", false);
            }
        }
        else
        {
            isPlayerMoving = false;
            spriteAnimator.SetBool("bIsWalking", false);
        }

        if (!bIsAttacking && Input.GetButtonUp("Fire1"))
        {
            StartCoroutine(PlayerAttack());
        }

        if(!bIsAttacking && Input.GetButtonUp("Fire3"))
        {
            bShouldDash = true;
            spriteAnimator.SetTrigger("Dash");
        }
        
        if (bShouldDash)
        {
            var pushbackDir = Vector3.zero;
            if (spriteRenderer.flipX) pushbackDir = Vector3.left;
            else pushbackDir = Vector3.right;
            pushbackDir *= dashSpeed;
            transform.position = Vector2.Lerp(transform.position, transform.position + pushbackDir, dashSpeed * Time.fixedDeltaTime);
        }

        IEnumerator PlayerAttack()
        {
            bIsAttacking = true;

            spriteAnimator.SetBool("bIsWalking", false);
            if(!hasSword) spriteAnimator.SetTrigger("Attack");
            else spriteAnimator.SetTrigger("AttackWithSword");

            yield return new WaitForSeconds(0.3f);

            if (spriteRenderer.flipX)
                baseWeaponObjLeft.ToggleWeaponState(true);
            else
                baseWeaponObjRight.ToggleWeaponState(true);

            yield return new WaitForSeconds(atkSpeed);
        }
    }

    public void ItemCollected(ItemType item)
    {
        switch (item)
        {
            case ItemType.Sword:
                hasSword = true;
                atkSpeed = 0.6f;
                atkDamage = 10;
                break;
            case ItemType.Armor:
                defenseReduction = 2f;
                spriteAnimator.SetBool("bHasArmor", true);
                spriteAnimator.SetTrigger("GoToIdle");
                break;
            case ItemType.Soul:
                health += 10;
                UpdateHealthBar();
                spriteAnimator.SetBool("bIsGhost", false);
                spriteAnimator.SetTrigger("GoToIdle");
                break;
        }
    }

    public void ReceiveDamage(float amount, Vector3 pushbackDirection = new Vector3(), float pushbackForce = 0f)
    {
        base.ReceiveDamage(amount, pushbackDirection, pushbackForce);

        if (health <= 0)
        {
            GlobalGameManager.uiManager.toggleGameOverPanel();
        }
    }
}
