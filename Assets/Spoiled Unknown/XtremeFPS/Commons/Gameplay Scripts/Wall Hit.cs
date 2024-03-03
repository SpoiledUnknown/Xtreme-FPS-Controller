/*Copyright © Spoiled Unknown*/
/*2024*/

using UnityEngine;
using XtremeFPS.Common.WeaponSystem.ShootableObjects;

public class WallHit : ShootableObject
{
    public GameObject particlesPrefab;

    public override void OnHit(RaycastHit hit)
    {
        GameObject instantiatedParticles = Instantiate(particlesPrefab, hit.point + hit.normal * 0.05f, Quaternion.LookRotation(hit.normal), transform.root.parent);

        Destroy(instantiatedParticles, 2f);
    }
}
