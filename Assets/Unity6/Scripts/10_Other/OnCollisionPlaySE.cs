using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 衝突すると、SEが鳴る
public class OnCollisionPlaySE : MonoBehaviour 
{
	//-------------------------------------
	public GameObject targetObject; //［目標オブジェクト］
	public string tagName; //［タグ名］
	public AudioClip se; //［鳴らすSE］
	//-------------------------------------

	void OnCollisionEnter2D(Collision2D collision)  // 衝突したとき
	{
		// 衝突したものが、目標オブジェクトか、タグ名なら
		if (collision.gameObject == targetObject ||
			collision.gameObject.tag == tagName) 
		{
			gameObject.GetComponent<AudioSource>().PlayOneShot(se);
		}
	}
}
