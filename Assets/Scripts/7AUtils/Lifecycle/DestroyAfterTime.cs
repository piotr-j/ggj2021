using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public SpriteRenderer fadeOutSprite;

    public float time = 1;
    private float totalTime;

    private void Start()
    {
        totalTime = time;
    }

    void Update()
    {
        time -= Time.deltaTime;

        if(fadeOutSprite != null)
        {
            fadeOutSprite.color = new Color(fadeOutSprite.color.r, fadeOutSprite.color.g, fadeOutSprite.color.b, Mathf.Clamp01(time / totalTime));
        }

        if(time <= 0)
        {
            Destroy(gameObject);
        }
    }
}
