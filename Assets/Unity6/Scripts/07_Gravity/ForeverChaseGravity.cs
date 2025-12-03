using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ずっと、追いかける（重力対応版）
public class ForeverChaseGravity : MonoBehaviour 
{
	//-------------------------------------
	public GameObject targetObject; //［目標オブジェクト］
	public float speed = 3; //［速度］
	//-------------------------------------
	Rigidbody2D rbody;

	void Start ()
	{
		// 衝突時に回転させない
		rbody = GetComponent<Rigidbody2D>();
		rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	void FixedUpdate()
	{
		// 目標オブジェクトの方向を調べて
		Vector3 dir = (targetObject.transform.position - transform.position).normalized;
		// その方向へ指定した量で進む（重力をかけたまま）
		float vx = dir.x * speed;
		rbody.linearVelocity = new Vector2(vx, rbody.linearVelocity.y);
		// 進む方向で左右の向きを変える
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		sprite.flipX = (vx < 0); 
	}
}
