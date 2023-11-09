/*Copyright © Non-Dynamic Studios*/
/*2023*/

using UnityEngine;

namespace NDS.UniversalWeaponSystem.ShootableObjects
{
    public abstract class ShootableObject : MonoBehaviour
    {

        public abstract void OnHit(RaycastHit hit);
    }
}

