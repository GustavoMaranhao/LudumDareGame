using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public List<GameObject> listOfItems;
    public int chestDisappearTime = 5;

    private bool bIsPlayerNear = false;
    private Animator spriteAnimator;

    private void Start()
    {
        spriteAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (bIsPlayerNear)
        {
            if (Input.GetButtonUp("Fire2"))
            {
                spriteAnimator.SetTrigger("OpenChest");
                DropItem();
                StartCoroutine(ChestDisappear());
            }
        }
    }

    IEnumerator ChestDisappear()
    {
        yield return new WaitForSeconds(chestDisappearTime);
        spriteAnimator.SetTrigger("VanishChest");
        Destroy(gameObject, spriteAnimator.GetCurrentAnimatorStateInfo(0).length);
    }

    internal virtual void DropItem()
    {
        if (listOfItems.Count > 0)
        {
            int index = Random.Range(0, listOfItems.Count);
            GameObject item = Instantiate(listOfItems[index], this.transform.localPosition, Quaternion.identity);
            item.transform.position = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, -1);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bIsPlayerNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        bIsPlayerNear = false;
    }
}
