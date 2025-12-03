using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 画面の外に出ると、自分を削除する
public class OnOutsideSceneDestroyMe : MonoBehaviour 
{
	bool showFlag = false;

	void LateUpdate()
	{
		if (GetComponent<Renderer>().isVisible) 
		{
			showFlag = true; // １回表示されたあと
		} 
		else if (showFlag) 
		{
			Destroy(gameObject); // 画面の外に出たら、削除
		}
	}
}
