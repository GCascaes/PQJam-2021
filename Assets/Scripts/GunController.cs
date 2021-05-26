using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private bool hasGun;
    [SerializeField]
    private float bulletVelocity;
    [SerializeField]
    private float shotsPerSecond;

    private bool shouldShoot;
    private float shootPeriod;
    private float lastShotTime;
    private Animator animator;

    public bool IsShooting { get; private set; } = false;

    private void Awake()
    {
        shootPeriod = 1 / shotsPerSecond;
        animator = GetComponent<Animator>();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (!hasGun)
            return;

        UpdateAnimatorShooting(shouldShoot);

        if (shouldShoot && Time.realtimeSinceStartup - lastShotTime > shootPeriod)
        {
            BulletController.Instantiate(bulletPrefab, transform.position, transform.rotation, bulletVelocity);
            lastShotTime = Time.realtimeSinceStartup;
            IsShooting = true;
            shouldShoot = false;
        }
        else if (!shouldShoot)
        {
            IsShooting = false;
        }
    }

    public void Shoot()
    {
        if (!hasGun)
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
