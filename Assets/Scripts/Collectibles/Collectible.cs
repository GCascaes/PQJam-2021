using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    private CollectibleType collectibleType;
    [SerializeField]
    [Range(1, 99)]
    private int quantity = 1;
    [SerializeField]
    private AudioClip pickupAudio;
    [SerializeField]
    [Range(0f, 1f)]
    private float pickupAudioVolume;
    [SerializeField]
    [Range(-1f, 1f)]
    private float pickupAudioPitch;

    public CollectibleType Type => collectibleType;
    public int Quantity => quantity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickupAudio != null
            && collision.TryGetComponent<ItemsController>(out var _))
            SoundManager.instance.PlaySFX(pickupAudio, pickupAudioVolume, pickupAudioPitch);
    }
}
