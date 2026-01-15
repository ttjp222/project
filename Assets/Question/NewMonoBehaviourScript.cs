using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorPuzzleComplete : MonoBehaviour
{
    [Header("UI References")]
    public GameObject puzzlePanel;
    public Image puzzleImage;
    public TMP_InputField answerInput;
    public Button submitButton;

    [Header("Puzzle Settings")]
    public Sprite questionSprite;
    public string correctAnswer = "こがねい";
    public float detectionRange = 2f;
    
    [Header("Object Settings")]
    public GameObject objectToHide;      // このオブジェクト（消えるもの）
    public GameObject objectToReveal;    // 現れるオブジェクト

    private Transform player;
    private bool isNearby = false;
    private bool isPuzzleSolved = false;
    private bool wasInputFieldClicked = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        puzzlePanel.SetActive(false);

        if (questionSprite != null)
            puzzleImage.sprite = questionSprite;

        submitButton.onClick.AddListener(CheckAnswer);
        
        // objectToHideが設定されていない場合は、このオブジェクト自身を使う
        if (objectToHide == null)
            objectToHide = gameObject;
        
        // 初期状態：現れるオブジェクトは非表示
        if (objectToReveal != null)
            objectToReveal.SetActive(false);
        
        // GameManagerに保存されている場合は、既に解決済み
        if (GameManager.Instance != null && 
            GameManager.Instance.IsDestroyed(gameObject.name))
        {
            // 既に解決済みなら最初から消しておく
            if (objectToHide != null)
                objectToHide.SetActive(false);
            
            if (objectToReveal != null)
                objectToReveal.SetActive(true);
            
            // このスクリプトも無効化
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (player == null || isPuzzleSolved) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange && !isNearby)
        {
            isNearby = true;
            puzzlePanel.SetActive(true);
            answerInput.text = "";
            PlayerMovement.canMove = true;
        }
        else if (distance > detectionRange && isNearby)
        {
            isNearby = false;
            puzzlePanel.SetActive(false);
            answerInput.text = "";
            wasInputFieldClicked = false;
            PlayerMovement.canMove = true;
        }

        if (isNearby && puzzlePanel.activeSelf)
        {
            if (answerInput.isFocused)
            {
                if (!wasInputFieldClicked)
                {
                    wasInputFieldClicked = true;
                    PlayerMovement.canMove = false;
                }
            }
            else
            {
                if (wasInputFieldClicked)
                {
                    wasInputFieldClicked = false;
                    PlayerMovement.canMove = true;
                }
            }
        }
    }

    void CheckAnswer()
    {
        string playerAnswer = answerInput.text.Trim();

        if (playerAnswer == correctAnswer)
        {
            Debug.Log("正解！オブジェクトを切り替えます");
            isPuzzleSolved = true;
            puzzlePanel.SetActive(false);
            PlayerMovement.canMove = true;

            // GameManagerに記録（永続的に消えるように）
            if (GameManager.Instance != null)
                GameManager.Instance.RegisterDestroyed(gameObject.name);

            // 消えるオブジェクトを非表示
            if (objectToHide != null)
                objectToHide.SetActive(false);
            
            // 現れるオブジェクトを表示
            if (objectToReveal != null)
                objectToReveal.SetActive(true);
            
            // このオブジェクト自身を破壊する場合
            if (objectToHide == gameObject)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("不正解");
            answerInput.text = "";
            answerInput.ActivateInputField();
        }
    }
}

/*
==================================================
使い方
==================================================

【セットアップ手順】

1. 扉オブジェクトにこのスクリプトをアタッチ

2. Inspectorで設定：
   【UI References】
   - Puzzle Panel: パズルUIのパネル
   - Puzzle Image: 問題の画像
   - Answer Input: 入力フィールド
   - Submit Button: 送信ボタン
   
   【Puzzle Settings】
   - Question Sprite: 問題の画像
   - Correct Answer: 正解の文字列（例: "1234"）
   - Detection Range: プレイヤーとの検知距離
   
   【Object Settings】
   - Object To Hide: 消えるオブジェクト（空欄なら自分自身）
   - Object To Reveal: 現れるオブジェクト（宝箱、隠し通路など）

3. 現れるオブジェクトの準備：
   - Hierarchyで隠したいオブジェクトを作成
   - 最初は表示されていてもOK（スクリプトが非表示にします）
   - Object To Reveal にドラッグ＆ドロップ

【例】
扉オブジェクト（Door）
└─ Object To Hide: Door（自分自身）
└─ Object To Reveal: HiddenPath（隠し通路）

正解すると：
- Door が消える
- HiddenPath が現れる
- シーンを再読み込みしても消えたまま（GameManager記録）
*/