using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRandom : MonoBehaviour
{
    public List<GameObject> targets;
    public bool autoTrigger = true;
    
    void Awake()
    {
        if (autoTrigger)
        {
            EnableRandomTarget();
        }
    }

    public void EnableRandomTarget ()
    {
        if (targets.Count == 0) return;

        foreach (var go in targets)
        {
            go.SetActive(false);
        }
        targets.Random().SetActive(true);
    }
}
