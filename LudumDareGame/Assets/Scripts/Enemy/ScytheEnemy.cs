using System.Collections;
using UnityEngine;

public class ScytheEnemy : BaseEnemy
{
    public int brainTick = 10;
    public float minDistanceToActivate = 50f;
    public int timeActive = 5;

    private float timer = 0f;
    private bool bShouldAct = true;
    private bool bIsPlayerNear = false;

    private Vector3 startingPosition;

    new public void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteAnimator = gameObject.GetComponent<Animator>();

        GetComponent<Rigidbody2D>().freezeRotation = true;

        baseWeaponObjLeft = transform.Find("LeftAttackTrigger").gameObject.GetComponent<WeaponTriggers>();
        baseWeaponObjRight = transform.Find("RightAttackTrigger").gameObject.GetComponent<WeaponTriggers>();

        startingPosition = transform.position;
    }

    protected void Update()
    {
        if (timer > brainTick)
        {
            StartCoroutine(ScytheBrainActive());
        }
        else
        {
            timer += Time.deltaTime;
        }

        bIsPlayerNear = Vector3.Magnitude(GlobalGameManager.player.transform.position - gameObject.transform.position) < minDistanceToActivate;

        spriteRenderer.flipX = !((transform.position - GlobalGameManager.player.transform.position).x >= 0);

        if (bShouldAct && bIsPlayerNear)
            base.Update();

        if (!bIsPlayerNear)
        {
            spriteAnimator.SetTrigger("GoToIdle");
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, startingPosition, step);
        }

    }

    IEnumerator ScytheBrainActive()
    {
        bShouldAct = true;

        yield return new WaitForSeconds(timeActive);

        bShouldAct = false;
        timer = 0;
    }
}
