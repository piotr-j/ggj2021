using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HexFlag : MonoBehaviour
{

    [Header("Configuration")]
    public Material artifactsFoundMaterial;
    public Material artifactsNotFoundMaterial;

    [Header("References")]
    public MeshRenderer flag;

    public void Excavated (bool hadArtifacts)
    {
        // show always, we dug here
        gameObject.SetActive(true);

        float duration = .5f;
        float ty = transform.localPosition.y;
        transform.localPosition = new Vector3(transform.localPosition.x, ty - 1, transform.localPosition.z);
        transform.DOLocalMoveY(ty, duration).SetEase(Ease.OutBack);

        transform.localScale = new Vector3(.1f, .1f, .1f);
        transform.DOScale(new Vector3(1, 1, 1), duration).SetEase(Ease.OutBack);
    }
}
