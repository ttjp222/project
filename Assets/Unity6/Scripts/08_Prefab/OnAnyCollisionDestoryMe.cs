using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 何と衝突しても、自分を削除する
public class OnAnyCollisionDestoryMe : MonoBehaviour 
{	
	void OnCollisionEnter2D(Collision2D collision) // 衝突したとき
	{
		Destroy(gameObject);  // 自分を削除
	}
}
