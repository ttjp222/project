using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カウントが最終値なら、ゲームをストップする
public class OnCountFinishedStopGame : MonoBehaviour 
{
	//-------------------------------------
	public CounterType kind = CounterType.Keys; //［カウンターの種類］
	public int lastCount = 3; //［最終値］
	//-------------------------------------

	void Start ()
	{
		Time.timeScale = 1; // 時間を動かす
	}

	void Update() 
	{
		if (GameCounter.counters[kind] == lastCount) // カウンターが最終値になったら
		{
			Time.timeScale = 0; // 時間を止める
		}
	}
}
