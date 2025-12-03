using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;	// シーン切り替えに必要

// 衝突すると、シーンを切り換える
public class OnCollisionSwitchScene : MonoBehaviour 
{
	//-------------------------------------
	public GameObject targetObject; //［目標オブジェクト］
	public string tagName; //［タグ名］
	public string sceneName;  //［シーン名］
	//-------------------------------------

	void OnCollisionEnter2D(Collision2D collision) // 衝突したとき
	{
		// 衝突したものが、目標オブジェクトか、タグ名なら
		if (collision.gameObject == targetObject ||
			collision.gameObject.tag == tagName) 
		{
			SceneManager.LoadScene (sceneName); // シーンを切り換える
		}
	}
}
