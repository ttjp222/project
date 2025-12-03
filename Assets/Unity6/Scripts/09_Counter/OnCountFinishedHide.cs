using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カウントが最終値なら、消す
public class OnCountFinishedHide : MonoBehaviour 
{
	//-------------------------------------
	public CounterType kind = CounterType.Keys; //［カウンターの種類］
	public int lastCount = 3; //［最終値］
	public GameObject hideObject; //［消すオブジェクト］
	//-------------------------------------

	void Update()
	{
		// カウンターが最終値になったら
		if (GameCounter.counters[kind] == lastCount) 
		{
    		hideObject.SetActive(false); // 非表示にする
		}
	}
}
