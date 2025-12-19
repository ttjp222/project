using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DoorPuzzleComplete : MonoBehaviour
{
    [Header("UI References")]
    public GameObject puzzlePanel;      // パネル全体
    public Image puzzleImage;           // 問題画像
    public InputField answerInput;      // 入力フィールド
    public Button submitButton;         // 送信ボタン
    
    [Header("Puzzle Settings")]
    public Sprite questionSprite;       // 問題の画像
    public string correctAnswer = "こがねい";
    public float detectionRange = 2f;
    
    private Transform player;
    private bool isNearby = false;
    private bool isPuzzleSolved = false;
    private bool wasInputFieldClicked = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // UI を最初は非表示
        puzzlePanel.SetActive(false);
        
        // 画像を設定
        if (questionSprite != null)
        {
            puzzleImage.sprite = questionSprite;
        }
        
        // ボタンにクリックイベントを追加
        submitButton.onClick.AddListener(CheckAnswer);
        
        // InputField の設定
        answerInput.contentType = InputField.ContentType.Standard;
    }

    void Update()
    {
        if (player == null || isPuzzleSolved) return;

        float distance = Vector2.Distance(transform.position, player.position);
        
        // 範囲内に入ったらUIを表示
        if (distance <= detectionRange && !isNearby)
        {
            isNearby = true;
            puzzlePanel.SetActive(true);
            answerInput.text = "";
            PlayerMovement.canMove = true; // 最初は移動可能
            Debug.Log("扉に近づきました");
        }
        // 範囲外に出たらUIを非表示
        else if (distance > detectionRange && isNearby)
        {
            isNearby = false;
            puzzlePanel.SetActive(false);
            answerInput.text = "";
            wasInputFieldClicked = false;
            
            // プレイヤー移動を再開
            PlayerMovement.canMove = true;
            Debug.Log("範囲外 - プレイヤー移動再開");
        }
        
        // パネルが表示されている間、InputFieldのフォーカス状態をチェック
        if (isNearby && puzzlePanel.activeSelf)
        {
            // InputFieldがフォーカスされているかチェック
            bool isFocused = answerInput.isFocused;
            
            if (isFocused)
            {
                // フォーカス中は移動停止
                if (!wasInputFieldClicked)
                {
                    wasInputFieldClicked = true;
                    PlayerMovement.canMove = false;
                    Debug.Log("入力フィールドフォーカス - プレイヤー移動停止");
                }
            }
            else
            {
                // フォーカスが外れたら移動再開
                if (wasInputFieldClicked)
                {
                    wasInputFieldClicked = false;
                    PlayerMovement.canMove = true;
                    Debug.Log("入力フィールド解除 - プレイヤー移動再開");
                }
            }
        }
    }

    void CheckAnswer()
    {
        string playerAnswer = answerInput.text.Trim();
        
        Debug.Log("入力された答え: " + playerAnswer);
        
        if (playerAnswer == correctAnswer)
        {
            Debug.Log("正解！扉が開きます");
            isPuzzleSolved = true;
            puzzlePanel.SetActive(false);
            
            // プレイヤー移動を再開
            PlayerMovement.canMove = true;
            
            // GameManagerに記録
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RegisterDestroyed(gameObject.name);
            }
            
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("不正解");
            answerInput.text = "";
            answerInput.ActivateInputField();
        }
    }
}