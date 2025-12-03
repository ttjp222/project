using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;	// シーン切り替えに必要

// カウントが最終値なら、シーンを切り換える
public class OnCountFinishedSwitchScene : MonoBehaviour 
{
	//-------------------------------------
	public CounterType kind = CounterType.Keys; //［カウンターの種類］
	public int lastCount = 3; //［最終値］
	public string sceneName;  //［シーン名］
	//-------------------------------------

	void Update()
	{
		if (GameCounter.counters[kind] == lastCount) // カウンターが最終値になったら
		{
			SceneManager.LoadScene (sceneName); // シーンを切り換える
		}
	}
}
