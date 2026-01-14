using UnityEngine;

public class ShowTextOnApproach : MonoBehaviour
{
    public GameObject textBox;

    private bool playerInside = false;

    void Start()
    {
        textBox.SetActive(false);
    }

    void Update()
    {
        // 一時メッセージ表示中なら表示しない
        if (MessageDisplay.Instance != null &&
            MessageDisplay.Instance.IsDisplaying())
        {
            textBox.SetActive(false);
            return;
        }

        // プレイヤーが中にいる時だけ表示
        textBox.SetActive(playerInside);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            textBox.SetActive(false);
        }
    }
}
