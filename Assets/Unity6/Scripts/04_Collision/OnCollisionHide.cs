using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 衝突すると、消す
public class OnCollisionHide : MonoBehaviour 
{
	//-------------------------------------
	public GameObject targetObject; //［目標オブジェクト］
	public string tagName; //［タグ名］
	public GameObject hideObject; //［消すオブジェクト］
	//-------------------------------------

	void OnCollisionEnter2D(Collision2D collision) // 衝突したとき
	{
		// 衝突したものが、目標オブジェクトか、タグ名なら
		if (collision.gameObject == targetObject ||
			collision.gameObject.tag == tagName) 
		{
    		hideObject.SetActive(false); // 非表示にする
		}
	}
}
