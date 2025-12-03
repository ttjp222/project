using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カウントが最終値なら、表示する
public class OnCountFinishedShow : MonoBehaviour 
{
	//-------------------------------------
	public CounterType kind = CounterType.Keys; //［カウンターの種類］
	public int lastCount = 3; //［最終値］
	public GameObject showObject;//［表示するオブジェクト］
	//-------------------------------------
	Vector3 disp_pos; // 表示位置

	void Start()
	{
    	showObject.SetActive(false); // 非表示にする
	}

	void Update()
	{
		// カウンターが最終値になったら
		if (GameCounter.counters[kind] == lastCount) 
		{
    		showObject.SetActive(true); // 表示する
		}
	}
}
