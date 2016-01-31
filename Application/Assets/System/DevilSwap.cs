using UnityEngine;
using System.Collections;

public class DevilSwap : MonoBehaviour {

	public GameObject normalDevil;
	public GameObject realDevil;

	public GameSystem gs;


	public void Start()
	{
		Invoke("Swap", Random.Range(37,76));
	}

	void Swap()
	{
		Invoke("Swap", Random.Range(37,76));
		StartCoroutine("DoS");
	}

	IEnumerator DoS()
	{
		if(gs.lost.Contains(2))
		{
			normalDevil.SetActive(false);
			realDevil.SetActive(true);
			yield return new WaitForSeconds(0.075f);
			normalDevil.SetActive(true);
			realDevil.SetActive(false);
		}
	}

}
