using UnityEngine;
using TMPro;

public class MessageDisplay : MonoBehaviour
{
    public static MessageDisplay Instance;

    [Header("UI")]
    public GameObject textBox;     // 黒背景（TextBox）
    public TMP_Text messageText;  // 表示テキスト

    [Header("Settings")]
    public float displayDuration = 2f;   // 表示時間（秒）
    public float disappearDistance = 3f; // 移動距離で消える

    private float displayTimer;
    private bool isDisplaying = false;

    private Transform player;
    private Vector3 lastPlayerPosition;
    private float movedDistance = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 最初は非表示
        if (textBox != null)
            textBox.SetActive(false);

        if (messageText != null)
            messageText.text = "";

        // プレイヤー取得
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            lastPlayerPosition = player.position;
        }
    }

    void Update()
    {
        if (!isDisplaying) return;

        // プレイヤー移動距離で消す
        if (player != null)
        {
            float distanceMoved = Vector3.Distance(player.position, lastPlayerPosition);
            movedDistance += distanceMoved;
            lastPlayerPosition = player.position;

            if (movedDistance >= disappearDistance)
            {
                HideMessage();
                return;
            }
        }

        // 時間経過で消す
        displayTimer -= Time.deltaTime;
        if (displayTimer <= 0f)
        {
            HideMessage();
        }
    }

    /// <summary>
    /// メッセージを表示する
    /// </summary>
    public void ShowMessage(string message)
    {
        if (messageText == null || textBox == null) return;

        messageText.text = message;
        textBox.SetActive(true);

        isDisplaying = true;
        displayTimer = displayDuration;
        movedDistance = 0f;

        if (player != null)
            lastPlayerPosition = player.position;

        Debug.Log("メッセージ表示: " + message);
    }

    /// <summary>
    /// メッセージを非表示
    /// </summary>
    void HideMessage()
    {
        if (messageText != null)
            messageText.text = "";

        if (textBox != null)
            textBox.SetActive(false);

        isDisplaying = false;
        movedDistance = 0f;
    }

    /// <summary>
    /// 他スクリプト用：表示中か？
    /// </summary>
    public bool IsDisplaying()
    {
        return isDisplaying;
    }
}
