using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 近づいたら、追いかける
public class OnNearChase : MonoBehaviour 
{
	//-------------------------------------
	public GameObject targetObject; //［目標オブジェクト］
    public float speed = 3; //［速度］
    public float limitDistance = 5; //［限界距離］
    public bool gravityFlag = true; //［重力を有効にする］
	//-------------------------------------
    Rigidbody2D rbody;
    bool flipFlag = false;

	void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        if (gravityFlag == false)
        {
            rbody.gravityScale = 0; // 重力を無効にする
        }
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, targetObject.transform.position);
        if (distance <= limitDistance)  // 限界距離より近いと追いかける
        {
            Vector2 direction = (targetObject.transform.position - transform.position).normalized;
            if (gravityFlag == true) 
            {
                rbody.linearVelocity = new Vector2(direction.x * speed, rbody.linearVelocity.y); // 水平移動のみ制御、垂直方向は物理に任せる
            }
            else 
            {
                rbody.linearVelocity = direction * speed;
            }
        } 
        else 
        { // そうでなかったら待ち伏せ
            if (gravityFlag == true) 
            {
                rbody.linearVelocity = new Vector2(0, rbody.linearVelocity.y);
            } 
            else 
            {
                rbody.linearVelocity = Vector2.zero;
            }
        }
        if (rbody.linearVelocity.x > 0)  // 移動量がプラスなら、右向き
        {
            flipFlag = false;
        }
        if (rbody.linearVelocity.x < 0)  // 移動量がマイナスなら、左向き
        {
            flipFlag = true;
        }
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = flipFlag;
    }
}
