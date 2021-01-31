using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InterestingArea : MonoBehaviour
{
    public SpriteRenderer sprite;

    private Camera cam;
    private Tween throb;

    void Start()
    {
        cam = App.instance.camera;
        // something nicer?
        throb = Tweens.ThrobForever(sprite.gameObject);
    }
    
    void Update()
    {
        // lets see if this will do :d
        sprite.transform.LookAt(cam.transform);
    }

    public void Revealed(HexTile tile)
    {
        // TODO hide animation first?
        throb?.Kill();
        Destroy(gameObject);
    }
}
