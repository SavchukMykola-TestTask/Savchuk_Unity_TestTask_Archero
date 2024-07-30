using UnityEngine;
using TMPro;

public class CharacterWithHP : MonoBehaviour
{
    [SerializeField] protected int StartHP = 100;
    [SerializeField] protected TextMeshPro TextWithCurrentHP;

    private int currentHP;
    public int CurrentHP
    {
        get => currentHP;
        private set
        {
            currentHP = value;
            TextWithCurrentHP.text = (Mathf.Clamp(CurrentHP, 0, StartHP)).ToString();
        }
    }

    public event System.Action OnDeath;

    protected void Awake()
    {
        CurrentHP = StartHP;
    }

    public void GetDamaged(int damage)
    {
        CurrentHP -= damage;
        if(CurrentHP <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    protected void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == Layers.LayerWithBullet)
        {
            GetDamaged(collider.gameObject.GetComponent<Bullet>().Damage);
        }
    }
}