using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 衝突すると、カウントを変更する
public class OnCollisionCountChange : MonoBehaviour 
{
	//-------------------------------------
	public GameObject targetObject; //［目標オブジェクト］
	public string tagName; //［タグ名］
	public CounterType kind = CounterType.Keys; //［カウンターの種類］
	public int addValue = 1; //［増加量］
	//-------------------------------------

	void OnCollisionEnter2D(Collision2D collision)  // 衝突したとき
	{
		// 衝突したものが、目標オブジェクトか、タグ名なら
		if (collision.gameObject == targetObject ||
			collision.gameObject.tag == tagName) 
		{
			// カウンターの値を変更する
			GameCounter.counters[kind] = GameCounter.counters[kind] + addValue;
		}
	}
}
