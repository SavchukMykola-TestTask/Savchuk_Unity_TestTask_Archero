using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem ParticlesOnCollision;

    private Rigidbody ThisRigidbody;

    public int Damage { get; private set; }

    private void Awake()
    {
        ThisRigidbody = GetComponent<Rigidbody>();
    }

    public void BeShot(Vector3 target, Vector3 startPosition, float speed, int damage)
    {
        Damage = damage;
        Vector3 Velocity = (target - startPosition).normalized * speed;
        ThisRigidbody.velocity = Velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        ParticlesOnCollision.transform.parent = null;
        ParticlesOnCollision.transform.position = transform.position;
        ParticlesOnCollision.Play();
        Destroy(gameObject);
    }
}