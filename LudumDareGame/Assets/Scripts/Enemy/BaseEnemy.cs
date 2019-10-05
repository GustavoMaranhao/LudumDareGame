using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : SpriteBase
{
    public int enemyLevel;
    public float minDistanceToPlayer = 5f;
    public float chanceToDropItems = 100f;

    private Transform playerTransform;
    private bool canAttack;
    private bool isAttacking;

    public List<GameObject> listOfItems;

    private void Update()
    {
        if (canAct && !isAttacking)
        {
            Act();
        }

        spriteRenderer.flipX = (transform.position - GlobalGameManager.player.transform.position).x >= 0;
    }

    public virtual void Act()
    {
        if (canAttack)
        {
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
            Attack();
            yield return new WaitForSeconds(atkSpeed);
        }

    }

    internal virtual void Attack()
    {
        GlobalGameManager.player.ReceiveDamage(atkDamage);
    }

    private void MoveTowardsPlayer()
    {
        if (Vector3.Magnitude(transform.position - GlobalGameManager.player.transform.position) <= minDistanceToPlayer) canAttack = true;
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, GlobalGameManager.player.transform.position, step);
    }

    internal virtual void EnemyDead()
    {
        var random = Random.Range(0, 100);
        if (random <= chanceToDropItems)
            DropItem();

        GlobalEvents.EnemyDeath(this, null);
    }

    internal virtual void DropItem()
    {
        if (listOfItems.Count > 0)
        {

            int index = Random.Range(0, listOfItems.Count);
            GameObject item = Instantiate(listOfItems[index], this.transform.position, Quaternion.identity);
            item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, -1);
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

    public void TakeDamage(int damage)
    {
        base.ReceiveDamage(damage);
        if (health <= 0) EnemyDead();
    }
}
