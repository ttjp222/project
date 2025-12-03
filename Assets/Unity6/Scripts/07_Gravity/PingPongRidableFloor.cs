using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 行ったり来たり移動する（乗って動く床）
public class PingPongRidableFloor : MonoBehaviour 
{
	//-------------------------------------
    public float speed = 1.0f; //［速度］
    public float moveTime = 2.0f; //［移動時間］
	//-------------------------------------
	Rigidbody2D rbody;
    private float timer = 0.0f;
    private List<GameObject> childObjects = new List<GameObject>();

	void Start ()
    {
		// 重力や外力の影響を受けずにスクリプトで移動
		rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= moveTime) 
        {
            speed = -speed; // 方向転換
            timer = 0.0f;
        }
        CheckRemove(); // 接触しなくなったら、一緒に移動を解除
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    // 接触したら、一緒に移動
    private void OnCollisionEnter2D(Collision2D other) 
    {
        childObjects.Add(other.gameObject);
        other.gameObject.transform.parent = transform;
    }

    // 接触しなくなったら、一緒に移動を解除
    private void CheckRemove() 
    {
        List<GameObject> removeList = new List<GameObject>();
        foreach (GameObject obj in childObjects) 
        {
            Collider2D childCol = obj.GetComponent<Collider2D>();
            Collider2D floorCol = GetComponent<Collider2D>();
            if (!Physics2D.IsTouching(childCol, floorCol)) 
            {
                obj.transform.parent = null;
                removeList.Add(obj);
            }
        }
        foreach (GameObject obj in removeList) 
        {
            childObjects.Remove(obj);
        }
    }
}
