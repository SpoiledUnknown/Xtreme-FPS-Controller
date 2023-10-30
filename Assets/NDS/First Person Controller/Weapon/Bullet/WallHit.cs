﻿using UnityEngine;
using NDS.UniversalWeaponSystem.ShootableObjects;

public class WallHit : ShootableObject
{
    public GameObject particlesPrefab;

    public override void OnHit(RaycastHit hit)
    {
        GameObject instantiatedParticles = (GameObject)Instantiate(particlesPrefab, hit.point + hit.normal * 0.05f, Quaternion.LookRotation(hit.normal), transform.root.parent);
        //instantiatedParticles.GetComponent<ParticleSystem>().main.startColor = GetComponent<Renderer>().material.color;
        Destroy(instantiatedParticles, 2f);
    }
}
