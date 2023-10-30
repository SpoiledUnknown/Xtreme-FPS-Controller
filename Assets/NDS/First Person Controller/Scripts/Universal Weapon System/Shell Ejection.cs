using UnityEngine;

namespace NDS.UniversalWeaponSystem.ShellEjection
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShellEjection : MonoBehaviour
    {
        public float minForce;
        public float maxForce;
        public float lifeTime;
        private Rigidbody rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            float force = Random.Range(minForce, maxForce);
            rb.AddForce(transform.right * force);
            rb.AddTorque(Random.insideUnitSphere * force);

            Invoke(nameof(DestroyShell), lifeTime);
        }

        private void DestroyShell()
        {
            Destroy(this.gameObject);
        }
    }
}

