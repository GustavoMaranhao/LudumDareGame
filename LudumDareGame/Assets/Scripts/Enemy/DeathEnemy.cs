using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEnemy : SpriteBase
{
    public Transform scytheGameObject;
    public float scytheInvincibleDist = 30f;

    public List<GameObject> listOfItems;

    private GameObject messageToActivate;

    [HideInInspector]
    public bool bIsInvincible = true;

    private bool hasDroppedItem = false;
    private bool bDeathEventNotSent = false;

    public void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteAnimator = gameObject.GetComponent<Animator>();

        healthBar = transform.Find("HealthBar").gameObject;
        healthBarValue = healthBar.transform.Find("HealthBarValue").gameObject;
        fullHealthLength = healthBarValue.transform.lossyScale.x;

        messageToActivate = GameObject.FindGameObjectWithTag("QuickMessagePanel");
        messageToActivate.SetActive(false);
    }

    public void Update()
    {
        spriteRenderer.flipX = !((transform.position - GlobalGameManager.player.transform.position).x >= 0);

        if (Vector3.Magnitude(scytheGameObject.position - transform.position) < scytheInvincibleDist)
        {
            spriteAnimator.SetBool("bIsInvincible", true);
            bIsInvincible = true;
        }
        else
        {
            spriteAnimator.SetBool("bIsInvincible", false);
            bIsInvincible = false;
        }

        if (health <= 0 && !bDeathEventNotSent)
        {
            bDeathEventNotSent = true;
            GlobalEvents.EnemyDeath(this, new EnemyDeathArgs("Boss"));
        }

        if (canAct && bIsDead && !hasDroppedItem)
        {
            EnemyDead();
            hasDroppedItem = true;
        }
    }

    internal virtual void EnemyDead()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        DropItem();
    }

    internal virtual void DropItem()
    {
        if (listOfItems.Count > 0)
        {
            int index = Random.Range(0, listOfItems.Count);
            GameObject item = Instantiate(listOfItems[index], this.transform.localPosition, Quaternion.identity);
            item.transform.position = new Vector3(item.transform.localPosition.x, GlobalGameManager.player.transform.position.y - GlobalGameManager.player.GetComponent<SpriteRenderer>().size.y*0.35f, -1);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GlobalEvents.DeathTouched(this, null);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GlobalEvents.DeathUnTouched(this, null);
    }
}
