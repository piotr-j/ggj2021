using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TeamScript : MonoBehaviour
{
    public HexTile occupiedTile { private set; get; }

    public List<GameObject> models;
    
    public void SetOccupiedTile(HexTile tile)
    {
        occupiedTile = tile;
    }

    public void HopModels (float duration, Vector3 target)
    {
        float hopDuration = (duration - .1f) / 2;
        float moveY = .5f;
        models.Shuffle();
        for (int i = 0; i < models.Count; i++)
        {
            var go = models[i];
            go.transform.DOLookAt(target, duration / 3).SetEase(Ease.InOutSine);
            float ly = go.transform.localPosition.y;
            Sequence scale1 = DOTween.Sequence()
                .Append(go.transform.DOLocalMoveY(ly + moveY, hopDuration).SetEase(Ease.InOutSine))
                .Append(go.transform.DOLocalMoveY(ly, hopDuration).SetEase(Ease.InOutSine)).SetDelay(i * .05f).OnComplete(() =>
                {
                    // look at camera?
                    //Vector3 cp = App.instance.camera.transform.position;
                    //go.transform.LookAt(new Vector3(cp.x, ly, cp.z));
                });
        }
    }

}
