using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(ShootingCharacter))]
[RequireComponent(typeof(UniformlyScaledObject))]
public class Monster : CharacterWithHP
{
    [SerializeField] private int DamageForPlayer;
    [SerializeField] private Player ThePlayer;
    [SerializeField] protected Transform ThePlayerTarget;
    [SerializeField] protected float ShootFrequency = 1;
    [SerializeField] private float ShootSpeed = 5;
    [SerializeField] private int ShootDamage = 20;
    [SerializeField] private int CoinsForKilling = 10;
    [SerializeField] private CoinsSaver CoinsSaver;

    protected float TimeAfterLastMovingOrShooting = 0;
    private ShootingCharacter ShootingCharacter;
    private UniformlyScaledObject UniformlyScaledObject;

    private bool IsDead = false;

    protected new void Awake()
    {
        base.Awake();
        ShootingCharacter = GetComponent<ShootingCharacter>();
        UniformlyScaledObject = GetComponent<UniformlyScaledObject>();

        OnDeath += () =>
        {
            if (!IsDead)
            {
                UniformlyScaledObject.ChangeScale(() => gameObject.SetActive(false));
                CoinsSaver.IncreaseCoinsQuantity(CoinsForKilling);
                ThePlayer.CheckForOpeningDoor();
                IsDead = true;
            }
        };
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == Layers.LayerWithPlayer)
        {
            collision.gameObject.GetComponent<CharacterWithHP>().GetDamaged(DamageForPlayer);
        }
    }

    protected void Shoot()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Atan2(ThePlayerTarget.position.x - transform.position.x, ThePlayerTarget.position.z - transform.position.z) * Mathf.Rad2Deg, transform.eulerAngles.z);
        ShootingCharacter.Shoot(ThePlayerTarget.position, ShootSpeed, ShootDamage);
    }
}