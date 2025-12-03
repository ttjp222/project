using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 衝突すると、表示する
public class OnCollisionShow : MonoBehaviour 
{
	//-------------------------------------
	public GameObject targetObject; //［目標オブジェクト］
	public string tagName; //［タグ名］
	public GameObject showObject; //［表示するオブジェクト］
	//-------------------------------------

	void Start()
	{
    	showObject.SetActive(false); // 非表示にする
	}

	void OnCollisionEnter2D(Collision2D collision) { // 衝突したとき
		// 衝突したものが、目標オブジェクトか、タグ名なら
		if (collision.gameObject == targetObject ||
			collision.gameObject.tag == tagName) 
		{
    		showObject.SetActive(true); // 表示する
		}
	}
}
