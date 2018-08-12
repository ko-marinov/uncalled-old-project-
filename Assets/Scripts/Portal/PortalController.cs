using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {

    public Animator animator;
    public float spawnPeriod;
    public GameObject monster;

	public void Open()
    {
        if (animator.GetBool(PORTAL_OPEN_PARAMETER))
        {
            return;
        }

        animator.SetBool(PORTAL_OPEN_PARAMETER, true);
        StartCoroutine(SpawnMonster());
    }

    public void Close()
    {
        animator.SetBool(PORTAL_OPEN_PARAMETER, false);
        StopAllCoroutines();
    }

    public bool IsOpen()
    {
        return animator.GetBool(PORTAL_OPEN_PARAMETER);
    }

    // private

    private const string PORTAL_OPEN_PARAMETER = "IsOpen";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            Open();
        }
        else if(Input.GetKeyDown(KeyCode.RightBracket))
        {
            Close();
        }
    }

    private IEnumerator SpawnMonster()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnPeriod);
            Instantiate(monster, transform.position, Quaternion.identity);
        }
    }
}
