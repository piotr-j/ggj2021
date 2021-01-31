using DG.Tweening;
using UnityEngine;

public class Tweens {


    public static (Sequence, Sequence) Squeeze(GameObject squeezedObject, float strengthScale = 1f, float timeScale = 1f, float startScaleX = 1f, float startScaleY = 1f)
    {
        float strength = 0.2f * strengthScale;

        float startTime = 0.05f * timeScale;
        float backTime = 0.6f * timeScale;

        Sequence xScale = DOTween.Sequence()
            .Append(squeezedObject.transform.DOScaleX(startScaleX + startScaleX * strength, startTime).SetEase(Ease.InOutQuad))
            .Append(squeezedObject.transform.DOScaleX(startScaleX, backTime).SetEase(Ease.OutElastic))
            ;

        Sequence yScale = DOTween.Sequence()
            .Append(squeezedObject.transform.DOScaleY(startScaleY + startScaleY * strength, startTime).SetEase(Ease.InOutQuad))
            .Append(squeezedObject.transform.DOScaleY(startScaleY, backTime).SetEase(Ease.OutElastic))
            .PrependInterval(0.05f * timeScale);

        return (xScale, yScale);
    }


    public static void JellyHit(GameObject squeezedObject)
    {
        Sequence scale1 = DOTween.Sequence()
            .Append(squeezedObject.transform.DOScaleX(0.6f, 0.2f).SetEase(Ease.OutCubic))
            .Join(squeezedObject.transform.DOScaleY(1.5f, 0.2f).SetEase(Ease.OutCubic))

            .Append(squeezedObject.transform.DOScaleX(1.4f, 0.2f).SetEase(Ease.OutCubic))
            .Join(squeezedObject.transform.DOScaleY(0.7f, 0.2f).SetEase(Ease.OutCubic))

            .Append(squeezedObject.transform.DOScaleX(1, 0.2f).SetEase(Ease.OutCubic))
            .Join(squeezedObject.transform.DOScaleY(1, 0.2f).SetEase(Ease.OutCubic));
    }

    public static Tween ThrobForever(GameObject obj, float targetX = 1.1f, float targetY = 1.1f)
    {
        Sequence scale1 = DOTween.Sequence()
            .Append(obj.transform.DOScale(new Vector3(targetX, targetY, 1), 0.2f).SetEase(Ease.InOutSine))
            .Append(obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).SetEase(Ease.InOutSine))
            .SetLoops(-1);
        return scale1;
    }

}
