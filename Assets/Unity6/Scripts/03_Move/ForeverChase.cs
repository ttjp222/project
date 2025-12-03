using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ずっと、追いかける
public class ForeverChase : MonoBehaviour 
{
	//-------------------------------------
	public GameObject targetObject; //［目標オブジェクト］
	public float speed = 3; //［速度］
	public bool ghostMode = false; //［壁を通り抜けるか］
	//-------------------------------------
	Rigidbody2D rbody;

	void Start ()
	{
		rbody = GetComponent<Rigidbody2D>();
		if (ghostMode)  // 壁を無視して移動
		{
	        rbody.bodyType = RigidbodyType2D.Kinematic;
		}
		 else  // 重力なし、回転なし
		{
			rbody.gravityScale = 0;
			rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	void FixedUpdate()
	{
		// 目標オブジェクトの方向を調べて
		Vector3 dir = (targetObject.transform.position - transform.position).normalized;
		// その方向へ指定した量で
		float vx = dir.x * speed;
		float vy = dir.y * speed;
		rbody.linearVelocity = new Vector2(vx, vy); // 移動する
        GetComponent<SpriteRenderer>().flipX = (vx < 0); // 向きを変える
	}
}
