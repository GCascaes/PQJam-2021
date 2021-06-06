using UnityEngine;

public class ArmOnDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject bombToArm;

    private void Awake()
    {
        if (TryGetComponent<HealthController>(out var healthController)
            && bombToArm != null
            && bombToArm.TryGetComponent<BombController>(out var bombController))
        {
            healthController.RegisterDeathAction(() => bombController.Arm());
        }
    }
}
