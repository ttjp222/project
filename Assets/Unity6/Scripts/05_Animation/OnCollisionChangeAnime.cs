using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 衝突すると、アニメーションを切り換える
public class OnCollisionChangeAnime : MonoBehaviour 
{
	//-------------------------------------
	public GameObject targetObject; //［目標オブジェクト］
	public string tagName; //［タグ名］
	public string normalAnime = ""; //［通常アニメ］
	public string nextAnime = "";   //［次のアニメ］
	public int changeSec = 1;	//［戻る秒数］
	//-------------------------------------

	void OnCollisionEnter2D(Collision2D collision) // 衝突したとき
	{
		// 衝突したものが、目標オブジェクトか、タグ名なら
		if (collision.gameObject == targetObject ||
			collision.gameObject.tag == tagName) 
		{
			Animator animator = GetComponent<Animator>();
			animator.Play(nextAnime); // 次のアニメに切り換える
			Invoke("setNormalPose", changeSec); // 指定秒経ったら
		}
	}

	void setNormalPose() 
	{
		Animator animator = GetComponent<Animator>();
		animator.Play(normalAnime);
	}
}
