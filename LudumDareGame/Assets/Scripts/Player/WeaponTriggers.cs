using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTriggers : MonoBehaviour
{
    public float weaponDamage = 10f;

    PlayerControls basePlayer;

    private void Awake()
    {
        basePlayer = GetComponentInParent<PlayerControls>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<BaseEnemy>().ReceiveDamage(weaponDamage);
        }
    }
}
