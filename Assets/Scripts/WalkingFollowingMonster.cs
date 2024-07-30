using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WalkingFollowingMonster : Monster
{
    [SerializeField] private float MovingSpeed = 2;
    [SerializeField] private float NeededDistanceFromPlayerForShooting;

    private Rigidbody Rigidbody;

    private new void Awake()
    {
        base.Awake();
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (Vector3.Distance(transform.position, ThePlayerTarget.position) <= NeededDistanceFromPlayerForShooting)
        {
            Rigidbody.velocity = Vector3.zero;
            TimeAfterLastMovingOrShooting += Time.deltaTime;
            if (TimeAfterLastMovingOrShooting >= ShootFrequency)
            {
                TimeAfterLastMovingOrShooting = 0;
                Shoot();
            }
        }
        else
        {
            TimeAfterLastMovingOrShooting = 0;
            Vector3 Velocity = (ThePlayerTarget.position - transform.position);
            Velocity.y = 0;
            Velocity = Velocity.normalized * MovingSpeed;
            Rigidbody.velocity = Velocity;
        }
    }
}