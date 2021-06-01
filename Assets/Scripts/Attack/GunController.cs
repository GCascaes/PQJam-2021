using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    internal GameObject bulletPrefab;
    [SerializeField]
    private bool shootingEnabled;
    [SerializeField]
    private bool hasGun;
    [SerializeField]
    private bool continuousShootingAnimation = true;
    [SerializeField]
    private float shootingAnimationDelay = 0;
    [SerializeField]
    private float bulletVelocity;
    [SerializeField]
    private float shotsPerSecond;
    [SerializeField]
    protected List<GameObject> shootPoints;

    internal bool shootContinuously = false;
    internal float currentBulletVelocity;

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

        bool shouldStartShootingThisFrame = shouldShoot
            && !IsShooting
            && (Time.realtimeSinceStartup - lastShotTime > shootPeriod || lastShotTime == 0);

        if (shouldStartShootingThisFrame)
            StartCoroutine(StartShooting());
    }

    public void Shoot(float overrideBulletVelocity = 0)
    {
        if (!hasGun || !canShoot)
            return;

        if (overrideBulletVelocity <= 0 || overrideBulletVelocity > bulletVelocity)
            overrideBulletVelocity = bulletVelocity;

        currentBulletVelocity = overrideBulletVelocity;
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

    private IEnumerator StartShooting()
    {
        IsShooting = true;
        UpdateAnimatorShooting(IsShooting);

        yield return new WaitForSecondsRealtime(shootingAnimationDelay);

        if (shootPoints.Any())
        {
            foreach (var shootPointTransform in shootPoints.Select(x => x.transform))
            {
                ProjectileController.Instantiate(bulletPrefab, shootPointTransform.position, shootPointTransform.rotation, currentBulletVelocity, tag);
            }
        }
        else
        {
            ProjectileController.Instantiate(bulletPrefab, transform.position, transform.rotation, currentBulletVelocity, tag);
        }

        lastShotTime = Time.realtimeSinceStartup;
        shouldShoot = shootContinuously;

        yield return new WaitForFixedUpdate();

        if (!continuousShootingAnimation || !shouldShoot)
            UpdateAnimatorShooting(false);

        IsShooting = false;
    }

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
