using UnityEngine;
using UnityEngine.InputSystem;

// マウスでタッチすると、ドラッグを開始する
public class OnMouseDownDragStart : MonoBehaviour
{
    //-------------------------------------
    public int drawOrder = 100; //［描画順］
    //-------------------------------------
    bool dragging;
    float zObj;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        zObj = transform.position.z;
    }

    void Update()
    {
        var cam = Camera.main; if (!cam || Mouse.current == null) return;

        // 押した瞬間：自分にヒットしたらドラッグ開始
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            var hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1 << gameObject.layer);
            dragging = hit.collider && hit.collider.gameObject == gameObject;
            if (dragging && sr) sr.sortingOrder = drawOrder;
        }

        // 離した瞬間：ドラッグ終了
        if (Mouse.current.leftButton.wasReleasedThisFrame) dragging = false;

        // ドラッグ中：マウス位置へ移動（元のZは維持）
        if (dragging)
        {
            var sp = Mouse.current.position.ReadValue();
            var d  = Mathf.Abs(cam.transform.position.z - zObj);
            var wp = cam.ScreenToWorldPoint(new Vector3(sp.x, sp.y, d));
            transform.position = new Vector3(wp.x, wp.y, zObj);
        }
    }
#if UNITY_EDITOR
    void OnValidate()
    {
        if (!GetComponent<Collider2D>())
        {
            Debug.LogWarning($"{name}: Collider2D がありません。", this);
        }
    }
#endif
}
