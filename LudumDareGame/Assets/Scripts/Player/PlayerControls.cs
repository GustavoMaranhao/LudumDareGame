using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : SpriteBase
{
    private GameObject baseWeaponObjLeft;
    private GameObject baseWeaponObjRight;

    void Start()
    {
        base.Start();

        baseWeaponObjLeft = transform.Find("LeftAttackTrigger").gameObject;
        baseWeaponObjRight = transform.Find("RightAttackTrigger").gameObject;
    }

    void FixedUpdate()
    {
        base.FixedUpdate();

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        boxCollider.transform.Translate(transform.TransformDirection(input * speed * Time.deltaTime));

        if(Input.GetAxisRaw("Horizontal") != 0)
            GetComponent<SpriteRenderer>().flipX = Input.GetAxisRaw("Horizontal") < 0;

        if (canAct && Input.GetButtonUp("Fire1"))
        {
            StartCoroutine(PlayerAttack());
        }

        IEnumerator PlayerAttack()
        {
            spriteAnimator.SetBool("StartAttackAnim", true);

            if (!spriteRenderer.flipX)
                baseWeaponObjLeft.SetActive(true);
            else
                baseWeaponObjRight.SetActive(true);

            //yield return new WaitForSeconds(atkSpeed);

            yield return new WaitForSeconds(0.5f);
            spriteAnimator.SetBool("StartAttackAnim", false);

            baseWeaponObjLeft.SetActive(false);
            baseWeaponObjRight.SetActive(false);
        }

    }
}
