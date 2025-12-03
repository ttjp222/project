using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ずっと、移動する（垂直）
public class ForeverMoveV : MonoBehaviour
{
	//-------------------------------------
	public float speed = 1; //［速度］
	//-------------------------------------
	void FixedUpdate()
	{
		transform.Translate(0 ,speed * Time.deltaTime, 0); // 垂直に移動
	}
}

