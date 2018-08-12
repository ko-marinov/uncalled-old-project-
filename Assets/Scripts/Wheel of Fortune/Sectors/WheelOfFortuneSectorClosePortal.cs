using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelOfFortuneSectorClosePortal : WheelOfFortuneSector {

    public override void Activate()
    {
        base.Activate();
        var portals = GameObject.FindGameObjectsWithTag("Portal");
        foreach (var portal in portals)
        {
            var portalController = portal.GetComponent<PortalController>();
            if (portalController.IsOpen())
            {
                portalController.Close();
                return;
            }
        }
    }
}
