using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 時間切れになると、自分を削除する
public class OnTimeoutDestroyMe : MonoBehaviour 
{
	//-------------------------------------
	public float limitSec = 3; //［秒数］
	//-------------------------------------

	void Start()
	{
		Destroy(gameObject, limitSec); // 指定秒後に消滅する予約
	}
}
