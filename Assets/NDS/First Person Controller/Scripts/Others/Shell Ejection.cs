using UnityEngine;

public class ShellEjection : MonoBehaviour
{
    public float minForce;
    public float maxForce;
    public Rigidbody rb;
    public float lifeTime = 4f;
    // Start is called before the first frame update
    void Start()
    {
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
