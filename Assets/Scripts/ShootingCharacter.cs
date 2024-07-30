using UnityEngine;

public class ShootingCharacter : MonoBehaviour
{
    [SerializeField] private GameObject BulletTemplate;
    [SerializeField] private Transform PlaceForNewBullets;

    public void Shoot(Vector3 target, float speed, int damage)
    {
        GameObject NewBullet = Instantiate(BulletTemplate);
        NewBullet.transform.position = PlaceForNewBullets.position;
        NewBullet.GetComponent<Bullet>().BeShot(target, PlaceForNewBullets.position, speed, damage);
    }
}