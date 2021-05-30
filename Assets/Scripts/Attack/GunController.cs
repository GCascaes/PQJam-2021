using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private bool shootingEnabled;
    [SerializeField]
    private bool hasGun;
    [SerializeField]
    private bool continuousShootingAnimation = true;
    [SerializeField]
    private float bulletVelocity;
    [SerializeField]
    private float shotsPerSecond;
    [SerializeField]
    private List<GameObject> shootPoints;

    internal bool shootContinuously = false;

    private bool canShoot;
    private bool shouldShoot;
    private float shootPeriod;
    private float lastShotTime;
    private Animator animator;

    public bool IsShooting { get; private set; } = false;

    protected virtual void Awake()
    {
        canShoot = shootingEnabled;
        shootPeriod = 1 / shotsPerSecond;
        animator = GetComponent<Animator>();
        UpdateAnimator();
    }

    protected virtual void FixedUpdate()
    {
        if (!hasGun)
            return;

        if (!canShoot)
        {
            shouldShoot = false;
            UpdateAnimatorShooting(shouldShoot);
            return;
        }

        bool shouldShootThisFrame = shouldShoot 
            && (Time.realtimeSinceStartup - lastShotTime > shootPeriod || lastShotTime == 0);

        UpdateAnimatorShooting(continuousShootingAnimation ? shouldShoot : shouldShootThisFrame);

        if (shouldShootThisFrame)
        {
            if (shootPoints.Any())
            {
                foreach(var shootPointTransform in shootPoints.Select(x => x.transform))
                {
                    BulletController.Instantiate(bulletPrefab, shootPointTransform.position, shootPointTransform.rotation, bulletVelocity, tag);
                }
            }
            else
            {
                BulletController.Instantiate(bulletPrefab, transform.position, transform.rotation, bulletVelocity, tag);
            }
            lastShotTime = Time.realtimeSinceStartup;
            IsShooting = true;
            shouldShoot = shootContinuously;
        }
        else if (!shouldShoot)
        {
            IsShooting = false;
        }
    }

    public void Shoot()
    {
        if (!hasGun || !canShoot)
            return;

        shouldShoot = true;
    }

    public void EquipGun()
    {
        hasGun = true;
        UpdateAnimator();
    }

    public void UnequipGun()
    {
        hasGun = false;
        UpdateAnimator();
    }

    public void EnableShooting() => canShoot = true;
    
    public void DisableShooting() => canShoot = false;

    private void UpdateAnimator()
    {
        if (animator == null)
            return;
        animator.SetBool("HasGun", hasGun);
    }

    private void UpdateAnimatorShooting(bool shooting)
    {
        if (animator == null)
            return;
        animator.SetBool("Shot", shooting);
    }
}
