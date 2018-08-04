using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {

    public Animator animator;

	public void Open()
    {
        animator.SetBool(PORTAL_OPEN_PARAMETER, true);
    }

    public void Close()
    {
        animator.SetBool(PORTAL_OPEN_PARAMETER, false);
    }

    // private

    private const string PORTAL_OPEN_PARAMETER = "IsOpen";
}
