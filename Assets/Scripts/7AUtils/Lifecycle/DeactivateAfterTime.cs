using UnityEngine;

public class DeactivateAfterTime : MonoBehaviour
{

    public GameObject deactivatedObject;

    public float activationTime;
    public float deactivationTime;

    private float timer = 0;
    private bool deactivated = false;
    private bool activated = false;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (!activated && timer >= activationTime)
        {
            activated = true;
            deactivatedObject.SetActive(true);
        }

        if (deactivatedObject != null && !deactivated && timer >= deactivationTime)
        {
            deactivated = true;
            deactivatedObject.SetActive(false);
        }
    }
}
