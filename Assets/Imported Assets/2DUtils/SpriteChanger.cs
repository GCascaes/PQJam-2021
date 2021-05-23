using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sprites;
    [SerializeField] float time = 0.3f;

    float currentTime;
    int index = 0;
    private void Start()
    {
        index = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[index];
    }

    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= time)
        {
            index++;

            if (index >= sprites.Length)
                index = 0;

            spriteRenderer.sprite = sprites[index];

            currentTime = 0;
        }
    }
}
