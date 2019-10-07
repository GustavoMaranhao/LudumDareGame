using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMessageManager : MonoBehaviour
{
    public int messageUpTime;

    void Start()
    {
        GlobalEvents.OnEnemyDeath += CheckForBossDead;
    }

    private void OnDestroy()
    {
        GlobalEvents.OnEnemyDeath -= CheckForBossDead;
    }

    void CheckForBossDead(object sender, System.EventArgs e)
    {
        EnemyDeathArgs args = (EnemyDeathArgs) e;

        if(args.tag == "Boss")
        {
            gameObject.SetActive(true);
            StartCoroutine(DisableQuickMessage());
        }
    }

    IEnumerator DisableQuickMessage()
    {
        yield return new WaitForSeconds(messageUpTime);
        gameObject.SetActive(false);
    }
}
