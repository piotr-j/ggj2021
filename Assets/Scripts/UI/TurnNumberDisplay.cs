using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TurnNumberDisplay : MonoBehaviour
{
   [SerializeField]
   TextMeshProUGUI turnText;

   [SerializeField]
   GameObject loseScreen;

   public void SetTurnNumber(int howManyLeft)
   {
      var vec = new Vector3(1.3f,1.3f,1.3f);
      turnText.transform.DOKill();
      turnText.transform.localScale = vec;
      turnText.transform.DOScale(1, 0.2f).SetEase(Ease.InCubic);
      turnText.text = howManyLeft.ToString();
      loseScreen.SetActive(howManyLeft<=0);
   }
}
