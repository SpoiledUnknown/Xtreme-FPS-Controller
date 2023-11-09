using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDS.UniversalWeaponSystem.ShootableObjects;

public class Enemy : ShootableObject
{
    public EnemyAiTutorial enemyAi;

    private void Start()
    {
        enemyAi = GetComponent<EnemyAiTutorial>();
    }

    public override void OnHit(RaycastHit hit)
    {
        enemyAi.TakeDamage(20);
    }
}
