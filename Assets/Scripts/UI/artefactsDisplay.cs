using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;


public class artefactsDisplay : MonoBehaviour
{
	[SerializeField]
	SingleArtifactDisplay[] artifactDisplays;

	[SerializeField]
	GameObject winScreen;

	[SerializeField]
	GameObject[] artifactItemImages;

	[SerializeField]
	GameObject artifactFoundScreen;

	List<int> gotArtifacts = new List<int>();


	void Start()
	{
		gotArtifacts.Add(0);
		gotArtifacts.Add(0);
		gotArtifacts.Add(0);
		gotArtifacts.Add(0);
		gotArtifacts.Add(0);
		gotArtifacts.Add(0);
		gotArtifacts.Add(0);
	}


	public void SetArtifactDisplay(int howMany)
	{
		for (int i = 0; i < artifactDisplays.Length; i++) artifactDisplays[i].SetState(i < howMany);

		if (howMany <= 0) return;

		if (howMany >= 5)
		{
			winScreen.SetActive(true); // nigdy nie pokaze ostatniego artefaktu zebranego bo od razu koniec gry
			return;
		}
		
		artifactFoundScreen.SetActive(true);
		foreach (var t in artifactItemImages) t.SetActive(false);
		var gotNow = Random.Range(0, gotArtifacts.Count-1);
		artifactItemImages[gotNow].SetActive(true);
		gotArtifacts.Remove(gotNow);
	}
}