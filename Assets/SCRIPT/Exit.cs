using UnityEngine;

public class SceneExitTrigger : MonoBehaviour
{
    [Header("Transition Settings")]
    public string targetSceneName = "NextScene";
    public string exitDirection = "right";
    
    [Header("Visual (Optional)")]
    public bool showGizmo = true;
    public Color gizmoColor = Color.yellow;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneTransitionManager.Instance != null)
            {
                Vector3 playerPos = other.transform.position;
                SceneTransitionManager.Instance.TransitionToScene(
                    targetSceneName,
                    exitDirection,
                    playerPos
                );
            }
        }
    }
    
    void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = gizmoColor;
            BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
            if (boxCol != null)
            {
                Gizmos.DrawWireCube(transform.position, boxCol.size);
            }
        }
    }
}