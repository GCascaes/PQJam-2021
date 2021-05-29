using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private bool shootingEnabled;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private bool hasGun;
    [SerializeField]
    private float bulletVelocity;
    [SerializeField]
    private float shotsPerSecond;

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

        UpdateAnimatorShooting(shouldShoot);

        if (shouldShoot && Time.realtimeSinceStartup - lastShotTime > shootPeriod)
        {
            BulletController.Instantiate(bulletPrefab, transform.position, transform.rotation, bulletVelocity, tag);
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
