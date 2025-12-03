using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ずっと、カウントの値を表示する（Text）
public class ForeverShowCountOld: MonoBehaviour 
{
	//-------------------------------------
	public CounterType kind = CounterType.Keys; //［カウンターの種類］
	//-------------------------------------

	void Update()
	{
		GetComponent<Text>().text = GameCounter.counters[kind].ToString();
	}
}
