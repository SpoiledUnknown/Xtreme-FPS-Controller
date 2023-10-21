using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDS.UniversalWeaponSystem;
using System;

public class Reloading : MonoBehaviour
{
    public WeaponSystem weaponSystem;
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (weaponSystem.reloading)
        {
            animator.SetBool("IsReloading", true);
        }
        else
        {
            animator.SetBool("IsReloading", false);
        }
    }
}
