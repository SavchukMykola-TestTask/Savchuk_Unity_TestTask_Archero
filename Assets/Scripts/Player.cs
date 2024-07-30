using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ShootingCharacter))]
[RequireComponent(typeof(UniformlyScaledObject))]
public class Player : CharacterWithHP
{
    [SerializeField] private FloatingJoystick FloatingJoystick;
    [SerializeField] private float Speed = 5;
    [SerializeField] private float AngleOffset = 44.4f;
    [SerializeField] private float MaximalNeglectableMagnitudeOfJoystickDirection = 0.01f;
    [SerializeField] private float ShootFrequency = 1;
    [SerializeField] private float ShootSpeed = 5;
    [SerializeField] private int ShootDamage = 20;
    [SerializeField] private List<CharacterWithHP> Enemies = new();
    [SerializeField] private Door Door;
    [SerializeField] private GameObject VictoryScreen;
    [SerializeField] private GameObject DefeatScreen;

    private Rigidbody PlayerRigidbody;
    private ShootingCharacter ShootingCharacter;
    private UniformlyScaledObject UniformlyScaledObject;
    private bool IsDead = false;
    private float TimeAfterLastMovingOrShooting = 0;

    private new void Awake()
    {
        base.Awake();
        PlayerRigidbody = GetComponent<Rigidbody>();
        ShootingCharacter = GetComponent<ShootingCharacter>();
        UniformlyScaledObject = GetComponent<UniformlyScaledObject>();

        OnDeath += () =>
        {
            if (!IsDead)
            {
                UniformlyScaledObject.ChangeScale(Defeat);
                IsDead = true;
            }
        };
    }

    private void LateUpdate()
    {
        Vector3 Velocity;
        float Angle;
        if (FloatingJoystick.DirectionVector.sqrMagnitude >= MaximalNeglectableMagnitudeOfJoystickDirection * MaximalNeglectableMagnitudeOfJoystickDirection)
        {
            Velocity = FloatingJoystick.DirectionVector * Speed;
            Angle = Vector2.Angle(Velocity, Vector2.up);
            if (Velocity.x < 0)
            {
                Angle *= -1;
            }
            transform.eulerAngles = new(transform.eulerAngles.x, Angle + AngleOffset, transform.eulerAngles.z);
            Velocity = new(Velocity.x, 0, Velocity.y);
        }
        else
        {
            Velocity = Vector3.zero;
            TimeAfterLastMovingOrShooting += Time.deltaTime;
            if (TimeAfterLastMovingOrShooting >= ShootFrequency)
            {
                TimeAfterLastMovingOrShooting = 0;
                Shoot();
            }
        }
        PlayerRigidbody.velocity = Velocity;
    }

    private void Shoot()
    {
        if (CheckIfAllEnemiesDied()) return;
        int NearestEnemyIndex = Enemies.FindIndex(enemy => enemy.CurrentHP > 0);
        for(int i = 0; i < Enemies.Count; i++)
        {
            if(Enemies[i].CurrentHP > 0 && (Enemies[i].transform.position - transform.position).magnitude <= (Enemies[NearestEnemyIndex].transform.position - transform.position).magnitude)
            {
                NearestEnemyIndex = i;
            }
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Atan2(Enemies[NearestEnemyIndex].transform.position.x - transform.position.x, Enemies[NearestEnemyIndex].transform.position.z - transform.position.z) * Mathf.Rad2Deg, transform.eulerAngles.z);
        ShootingCharacter.Shoot(Enemies[NearestEnemyIndex].transform.position, ShootSpeed, ShootDamage);
    }

    private bool CheckIfAllEnemiesDied()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].CurrentHP > 0)
            {
                return false;
            }
        }
        return true;
    }

    public void CheckForOpeningDoor()
    {
        if(CheckIfAllEnemiesDied())
        {
            Door.Open();
        }
    }

    protected new void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
        if(collider.gameObject.layer == Layers.LayerWithVictoryZone)
        {
            Win();
        }
    }

    public void Defeat()
    {
        Time.timeScale = 0;
        DefeatScreen.SetActive(true);
    }

    public void Win()
    {
        Time.timeScale = 0;
        VictoryScreen.SetActive(true);
    }
}