/*Copyright © Non-Dynamic Studios*/
/*2023*/

using UnityEngine;
using NDS.UniversalWeaponSystem.ShootableObjects;

public class MovableGameobjectHit : ShootableObject
{
    public GameObject particlesPrefab;
    public float impactForce;

    public override void OnHit(RaycastHit hit)
    {
        GameObject instantiatedParticles = (GameObject)Instantiate(particlesPrefab, hit.point + hit.normal * 0.05f, Quaternion.LookRotation(hit.normal), transform.root.parent);
        Destroy(instantiatedParticles, 2f);

        GetComponent<Rigidbody>().AddForceAtPosition(-hit.normal * impactForce, hit.point);
    }
}
