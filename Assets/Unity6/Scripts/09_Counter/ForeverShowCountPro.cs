using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ずっと、カウントの値を表示する（TextMeshPro）
public class ForeverShowCountPro: MonoBehaviour 
{
	//-------------------------------------
	public CounterType kind = CounterType.Keys; //［カウンターの種類］
	//-------------------------------------

	void Update()
	{
		GetComponent<TextMeshProUGUI>().text = GameCounter.counters[kind].ToString();
	}
}
