using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 衝突すると、ゲームをストップする
public class OnCollisionStopGame : MonoBehaviour 
{
	//-------------------------------------
	public GameObject targetObject; //［目標オブジェクト］
	public string tagName; //［タグ名］
	//-------------------------------------

	void Start ()
	{
		Time.timeScale = 1; // 時間を動かす
	}

	void OnCollisionEnter2D(Collision2D collision) // 衝突したとき
	{
		// 衝突したものが、目標オブジェクトか、タグ名なら
		if (collision.gameObject == targetObject ||
			collision.gameObject.tag == tagName) 
		{
			Time.timeScale = 0; // 時間を止める
		}
	}
}
