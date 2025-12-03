using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カウンター本体
public class GameCounter : MonoBehaviour 
{
	//-------------------------------------
	public CounterType kind = CounterType.Keys; //［カウンターの種類］
	public int startCount = 0; //［初期値］
	//-------------------------------------
    public static Dictionary<CounterType, int> counters = new Dictionary<CounterType, int>();

	void Start()
	{
		counters[kind] = startCount;
	}
}
public enum CounterType { // カウンターの種類
    Keys, Hearts, Miss, Score, Gold, ItemA, ItemB, ItemC
}