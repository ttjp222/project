using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ときどき、反転する
public class SometimeFlip : MonoBehaviour 
{
	//-------------------------------------
	public int maxCount = 50; //［頻度］
	//-------------------------------------
	int count = 0; // カウンター用
	bool flipFlag = false;

	void Start ()
	{
		count = 0;	// カウンターをリセット
	}

	void FixedUpdate()
	{
		count = count + 1; // カウンターに1を足して
		if (count >= maxCount)  // もし、maxCountになったら
		{
			transform.Rotate(0, 0, 180); // 180度回転して曲がる
			count = 0; // カウンターをリセット
			flipFlag = !flipFlag;	// 上下を反転
			GetComponent<SpriteRenderer>().flipY = flipFlag; 
		}
	}
}