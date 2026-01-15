using UnityEngine;
using System.Collections;

public class PushableObject : MonoBehaviour
{
    [Header("Push Settings")]
    public float gridSize = 1f;
    public float pushSpeed = 5f;
    public LayerMask obstacleLayer;
    public string pushableTag = "Pushable";

    private bool isMoving = false;

    // プレイヤーが最初に当たった方向
    private Vector2 pushDir = Vector2.zero;

    void Start()
    {
        SnapToGrid();
    }

    void SnapToGrid()
    {
        float x = Mathf.Round(transform.position.x / gridSize) * gridSize;
        float y = Mathf.Round(transform.position.y / gridSize) * gridSize;
        transform.position = new Vector3(x, y, transform.position.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // プレイヤー → ブロック方向
        Vector2 fromPlayer =
            (Vector2)(transform.position - collision.transform.position);

        pushDir = RoundToFourDirections(fromPlayer);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isMoving) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerMovement player =
            collision.gameObject.GetComponent<PlayerMovement>();
        if (player == null) return;

        Vector2 inputDir =
            RoundToFourDirections(player.LastMoveDirection);

        if (inputDir == Vector2.zero) return;
        if (inputDir != pushDir) return;

        TryPush(pushDir);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        pushDir = Vector2.zero;
    }

    Vector2 RoundToFourDirections(Vector2 input)
    {
        if (input == Vector2.zero) return Vector2.zero;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            return new Vector2(Mathf.Sign(input.x), 0);
        else
            return new Vector2(0, Mathf.Sign(input.y));
    }

    void TryPush(Vector2 direction)
    {
        Vector3 nextPos =
            transform.position +
            new Vector3(direction.x, direction.y, 0) * gridSize;

        if (CanMoveTo(nextPos))
        {
            StartCoroutine(MoveToPosition(nextPos));
        }
    }

    bool CanMoveTo(Vector3 position)
    {
        float radius = gridSize * 0.4f;

        if (Physics2D.OverlapCircle(position, radius, obstacleLayer))
            return false;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(position, radius);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject != gameObject &&
                hit.CompareTag(pushableTag))
                return false;
        }

        return true;
    }

    IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true;

        // ★ プレイヤーを止める
        PlayerMovement.canMove = false;

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                pushSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = target;

        // ★ プレイヤーの移動を再開
        PlayerMovement.canMove = true;

        isMoving = false;
    }
}
