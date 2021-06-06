using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPowerUpController : GunController
{
    [SerializeField]
    private float quickShotBulletVelocity;
    [SerializeField]
    private float quickShotDuration;
    [SerializeField]
    private List<GameObject> spreadShotExtraPoints;
    [SerializeField]
    private float spreadShotDuration;
    [SerializeField]
    private GameObject superShotBulletPrefab;
    [SerializeField]
    private float superShotDuration;

    private Coroutine quickShot;
    private Coroutine spreadShot;
    private Coroutine superShot;

    private bool shouldCancel = false;

    public void ActivateQuickShot()
    {
        if (quickShot != null)
            StopCoroutine(quickShot);
        quickShot = StartCoroutine(QuickShotRoutine());
    }

    public void ActivateSpreadShot()
    {
        if (spreadShot != null)
            StopCoroutine(spreadShot);
        spreadShot = StartCoroutine(SpreadShotRoutine());
    }

    public void ActivateSuperShot()
    {
        if (superShot != null)
            StopCoroutine(superShot);
        superShot = StartCoroutine(SuperShotRoutine());
    }

    private IEnumerator QuickShotRoutine()
    {
        yield return CancelOtherPowerUps();

        var bulletVelocity = currentBulletVelocity;
        currentBulletVelocity = quickShotBulletVelocity;

        yield return WaitForTimeOrCancel(quickShotDuration);

        currentBulletVelocity = bulletVelocity;
    }

    private IEnumerator SpreadShotRoutine()
    {
        yield return CancelOtherPowerUps();

        shootPoints.AddRange(spreadShotExtraPoints);

        yield return WaitForTimeOrCancel(spreadShotDuration);

        foreach (var extraPoint in spreadShotExtraPoints)
            shootPoints.Remove(extraPoint);
    }

    private IEnumerator SuperShotRoutine()
    {
        yield return CancelOtherPowerUps();

        var oldBulletPrefab = bulletPrefab;
        bulletPrefab = superShotBulletPrefab;

        yield return WaitForTimeOrCancel(superShotDuration);

        bulletPrefab = oldBulletPrefab;
    }

    private IEnumerator CancelOtherPowerUps()
    {
        shouldCancel = true;
        yield return new WaitForEndOfFrame();
        shouldCancel = false;
    }

    private IEnumerator WaitForTimeOrCancel(float time)
    {
        var startTime = Time.time;
        yield return new WaitUntil(() => shouldCancel || Time.time - startTime > time);
    }
}
