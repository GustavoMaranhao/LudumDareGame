using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : SpriteBase
{
    public int enemyLevel;
    public float minDistanceToPlayer = 1f;
    public float chanceToDropItems = 100f;

    private Transform playerTransform;
    [HideInInspector] public bool canStartAttack = false;
    [HideInInspector] public bool isAttacking = false;
    private bool hasDroppedItem = false;

    public List<GameObject> listOfItems;

    protected void Update()
    {
        if (canAct && !isAttacking && !bIsDead)
        {
            Act();
        }

        if (bIsPushBack)
        {
            transform.position = Vector2.Lerp(transform.position, perpDirection, pushbackSpeed * Time.fixedDeltaTime);
        }

        if (canAct && bIsDead && !hasDroppedItem)
        {
            EnemyDead();
            hasDroppedItem = true;
        }

        spriteRenderer.flipX = !((transform.position - GlobalGameManager.player.transform.position).x >= 0);
    }

    public virtual void Act()
    {
        if (canStartAttack)
        {
            //spriteAnimator.SetTrigger("Attack");
            if (isAttacking)
            {
                return;
            }
            StartCoroutine(AttackPlayer());
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    internal virtual IEnumerator AttackPlayer()
    {
        isAttacking = true;
        while (true)
        {
            yield return new WaitForSeconds(atkSpeed);
            spriteAnimator.SetTrigger("Attack");
            Attack();
            if (!checkIfPlayerClose()) break;
        }
        canStartAttack = false;
    }

    internal virtual void Attack()
    {
        if (bIsDead){
			GetComponent<BoxCollider2D>().enabled = false;
			canStartAttack = false;
			return;
		}

        if (!spriteRenderer.flipX)
            baseWeaponObjLeft.ToggleWeaponState(true);
        else
            baseWeaponObjRight.ToggleWeaponState(true);
    }

    internal bool checkIfPlayerClose()
    {	
        return Vector2.Distance(transform.position, GlobalGameManager.player.transform.position) <= minDistanceToPlayer;
    }

    private void MoveTowardsPlayer()
    {
        if (spriteAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;        
        //if (Vector3.Magnitude(transform.position - GlobalGameManager.player.transform.position) <= minDistanceToPlayer) canAttack = true;
        canStartAttack = checkIfPlayerClose();
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, GlobalGameManager.player.transform.position, step);
        spriteAnimator.SetBool("bIsWalking", !canStartAttack);
    }

    internal virtual void EnemyDead()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        var random = Random.Range(0, 100);
        if (random <= chanceToDropItems)
            DropItem();

        if(!gameObject.name.Contains("Demon"))
            GlobalEvents.EnemyDeath(this, new EnemyDeathArgs("CommonEnemy"));
        else
            GlobalEvents.EnemyDeath(this, new EnemyDeathArgs("FinalBoss"));
    }

    internal virtual void DropItem()
    {
        if (listOfItems.Count > 0)
        {
            int index = Random.Range(0, listOfItems.Count);
            GameObject item = Instantiate(listOfItems[index], this.transform.localPosition, Quaternion.identity);
            //item.GetComponent<ItemPickup>().SetInitialPosition(this.transform.position);
            item.transform.position = new Vector3(item.transform.localPosition.x, GlobalGameManager.player.transform.position.y - GlobalGameManager.player.GetComponent<SpriteRenderer>().size.y / 2, -1);
        }

    }

    public void StartActing()
    {
        canAct = true;
    }

    public void StopActing()
    {
        canAct = false;
    }

    public void ReceiveDamage(float amount, Vector3 pushbackDirection = new Vector3(), float pushbackForce = 0f)
    {
        spriteAnimator.SetTrigger("GoToIdle");
        spriteAnimator.SetBool("bIsWalking", false);

        baseWeaponObjLeft.ToggleWeaponState(false);
        baseWeaponObjRight.ToggleWeaponState(false);

        base.ReceiveDamage(amount, pushbackDirection, pushbackForce);
    }
}
