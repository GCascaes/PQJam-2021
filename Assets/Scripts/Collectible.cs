using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    private CollectibleType collectibleType;
    [SerializeField]
    [Range(1, 99)]
    private int quantity;

    public CollectibleType Type => collectibleType;
    public int Quantity => quantity;
}
