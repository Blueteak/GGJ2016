using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BucketWomanSprite : MonoBehaviour {

	public Image myImage;
	public Image targImage;
	
	// Update is called once per frame
	void Update () 
	{
		myImage.color = targImage.color;
	}
}
