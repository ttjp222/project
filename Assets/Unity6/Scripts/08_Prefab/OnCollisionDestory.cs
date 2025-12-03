using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 衝突すると、削除する
public class OnCollisionDestory : MonoBehaviour 
{
	//-------------------------------------
	public GameObject targetObject; //［目標オブジェクト］
	public string tagName; //［タグ名］
    public Options destroyOption; //［消す相手］
	//-------------------------------------

	void OnCollisionEnter2D(Collision2D collision) // 衝突したとき
	{
		// 衝突したものが、目標オブジェクトか、タグ名なら
		if (collision.gameObject == targetObject ||
			collision.gameObject.tag == tagName) 
		{
			if (destroyOption == Options.Me)
			{
				Destroy(gameObject);
			}
			else if (destroyOption == Options.You)
			{
				Destroy(collision.gameObject); // 相手を削除
			}
			else if (destroyOption == Options.YouAndMe) 
			{ 
				Destroy(collision.gameObject); // 相手を削除
				Destroy(gameObject);  // 自分を削除
			}
		}
	}
}
public enum Options{
    Me, You, YouAndMe
}