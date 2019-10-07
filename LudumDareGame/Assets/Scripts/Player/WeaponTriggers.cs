using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTriggers : MonoBehaviour
{
    public float pushbackForce = 10f;

    public bool pushToTheLeft = false;
    public bool pushToTheRight = false;

    public AudioSource weaponSound;
    public AudioSource punchSound;

    PlayerControls basePlayer;

    private bool isInHurtArea = false;
    private Collider2D collision;

    private void Awake()
    {
        basePlayer = GetComponentInParent<PlayerControls>();        
    }

    public void ToggleWeaponState(bool state)
    {
        isInHurtArea = state;
        gameObject.SetActive(state);
    }

    private void Update()
    {
        if (isInHurtArea && collision != null)
        {
            if (!GetComponentInParent<SpriteBase>().canDamage) return;
            if (collision.tag == "Enemy" && transform.tag != "EnemyAttack")
            {
                if (weaponSound != null && GlobalGameManager.player.hasSword) weaponSound.Play();
                if (punchSound != null && !GlobalGameManager.player.hasSword) punchSound.Play();

                if (collision.GetComponent<DeathEnemy>() != null)
                {
                    if(!collision.GetComponent<DeathEnemy>().bIsInvincible)
                        collision.gameObject.GetComponent<DeathEnemy>().ReceiveDamage(GlobalGameManager.player.atkDamage);
                }
                else
                {
                    var lookDirection = Vector3.zero;
                    if (pushToTheLeft && !pushToTheRight) lookDirection = Vector3.left;
                    if (pushToTheRight && !pushToTheLeft) lookDirection = Vector3.right;
                    collision.gameObject.GetComponent<BaseEnemy>().ReceiveDamage(GlobalGameManager.player.atkDamage, lookDirection, pushbackForce);
                }
            }
            if (collision.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerControls>().ReceiveDamage(GetComponentInParent<SpriteBase>().atkDamage);
            }
            GetComponentInParent<SpriteBase>().canDamage = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInHurtArea = true;
        this.collision = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInHurtArea = false;
    }
}
