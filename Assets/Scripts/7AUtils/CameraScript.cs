using System;
using DG.Tweening;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
    }

    void Update() {
    }

    public void Shake(float time, float strength, int vibrato)
    {
        Tweener shakeTween = cam.DOShakePosition(time, strength, vibrato);
    }

    public void Shake(float time, float strength)
    {
        Tweener shakeTween = cam.DOShakePosition(time, strength);
    }
    
}
