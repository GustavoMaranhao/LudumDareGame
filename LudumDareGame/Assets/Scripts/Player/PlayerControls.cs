using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ItemType
{
    Sword,
    Armor,
    Soul, 
    Potion
}

public class PlayerControls : SpriteBase
{
    [HideInInspector]
    public bool isPlayerMoving = false;
    [HideInInspector]
    public bool hasSword = false;
    [HideInInspector]
    public bool bIsAttacking = false;

    public float dashSpeed = 4f;
    [HideInInspector]
    public bool bShouldDash = false;

    public AudioSource itemPickUpSound;
    public AudioSource dashSound;
	BoxCollider2D triggerLeft;
	BoxCollider2D triggerRight;

    new void Start()
    {
        base.Start();

        UpdateHealthBar();
		triggerLeft = GetComponent("LeftAttackTrigger") as BoxCollider2D;
		triggerRight = GetComponent("RightAttackTrigger") as BoxCollider2D;
		
        baseWeaponObjRight = transform.Find("RightAttackTrigger").gameObject.GetComponent<WeaponTriggers>();

        spriteAnimator.SetBool("bIsGhost", true);
        spriteAnimator.SetTrigger("GoToIdle");
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        canAct = true;

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

        if (!bIsAttacking && Input.GetButtonDown("Fire1"))
        {
			PlayerAttack2();
        }

        if(!bIsAttacking && Input.GetButtonDown("Fire3"))
        {
            if (dashSound != null && bShouldDash == false) dashSound.Play();

            bShouldDash = true;
            spriteAnimator.SetTrigger("Dash");
        }
        
        if (bShouldDash)
        {
            var pushbackDir = Vector3.zero;
            if (spriteRenderer.flipX) pushbackDir = Vector3.left;
            else pushbackDir = Vector3.right;
            pushbackDir *= dashSpeed;
			boxCollider.enabled = false;
			rigidBody.simulated = false;
			rigidBody.isKinematic = true;
            transform.position = Vector2.Lerp(transform.position, transform.position + pushbackDir, dashSpeed * Time.fixedDeltaTime);

        }
		
		void PlayerAttack2()
        {
            bIsAttacking = true;

            spriteAnimator.SetBool("bIsWalking", false);
            if(!hasSword) spriteAnimator.SetTrigger("Attack");
            else spriteAnimator.SetTrigger("AttackWithSword");

            //yield return new WaitForSeconds(0.3f);

            if (spriteRenderer.flipX)
                baseWeaponObjLeft.ToggleWeaponState(true);
            else
                baseWeaponObjRight.ToggleWeaponState(true);

            //yield return new WaitForSeconds(atkSpeed);
			
        }
    }

    public void ItemCollected(ItemType item)
    {
        if (itemPickUpSound != null) itemPickUpSound.Play();

        switch (item)
        {
            case ItemType.Sword:
                hasSword = true;
                atkSpeed = 0.6f;
                atkDamage = 10;
				triggerLeft.size = new Vector2(3.5f, 6.9f);
				triggerRight.size = new Vector2(3.5f, 6.9f);
                break;
            case ItemType.Armor:
                defenseReduction = 3f;
                spriteAnimator.SetBool("bHasArmor", true);
                spriteAnimator.SetTrigger("GoToIdle");
                break;
            case ItemType.Soul:
                health += 70;
                UpdateHealthBar();
                spriteAnimator.SetBool("bIsGhost", false);
                spriteAnimator.SetTrigger("GoToIdle");
                break;
            case ItemType.Potion:
                health += 10;
                UpdateHealthBar();
                FloatingTextController.CreateFloatingText("+10", transform);
                break;
        }
    }

    new public void ReceiveDamage(float amount, Vector3 pushbackDirection = new Vector3(), float pushbackForce = 0f)
    {
        
		if(bShouldDash) return;
		base.ReceiveDamage(amount, pushbackDirection, pushbackForce);
		
        if (health <= 0)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<EndGameManager>().gameOver = true;
            GlobalGameManager.uiManager.toggleGameOverPanel();
        }
    }
}
