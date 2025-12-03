using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 衝突すると、そこにプレハブを作る
public class OnCollisionCreatePrefab : MonoBehaviour 
{
	//-------------------------------------
	public GameObject targetObject; //［目標オブジェクト］
	public string tagName; //［タグ名］
	public GameObject newPrefab; //［作るプレハブ］
    public int newZ = -5; //［描画順］
	//-------------------------------------

	void OnCollisionEnter2D(Collision2D collision) // 衝突したとき
	{
		// 衝突したものが、目標オブジェクトか、タグ名なら
		if (collision.gameObject == targetObject ||
			collision.gameObject.tag == tagName) 
		{
			// 衝突位置にプレハブを作ってその位置の手前に表示する
			GameObject newGameObject = Instantiate(newPrefab) as GameObject;
			Vector3 pos = collision.contacts[0].point;
			pos.z = newZ;
			newGameObject.transform.position = pos;
		}
	}
}
