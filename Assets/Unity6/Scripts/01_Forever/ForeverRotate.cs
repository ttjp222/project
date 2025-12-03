using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ずっと、回転する
public class ForeverRotate : MonoBehaviour 
{
	//-------------------------------------
	public float angle = 90; //［角度］
	//-------------------------------------
	void FixedUpdate()
	{
		transform.Rotate(0, 0, angle * Time.deltaTime);	// 回転
	}
}
