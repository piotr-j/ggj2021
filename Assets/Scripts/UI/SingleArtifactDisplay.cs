using UnityEngine;


class SingleArtifactDisplay : MonoBehaviour
{
	[SerializeField] GameObject doneImage;
	[SerializeField] GameObject nopeImage;


	public void SetState(bool state)
	{
		doneImage.SetActive(state);
		nopeImage.SetActive(!state);
	}

}