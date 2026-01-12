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
            isPuzzleSolved = true;
            puzzlePanel.SetActive(false);
            PlayerMovement.canMove = true;

            if (GameManager.Instance != null)
                GameManager.Instance.RegisterDestroyed(gameObject.name);

            Destroy(gameObject);
        }
        else
        {
            answerInput.text = "";
            answerInput.ActivateInputField();
        }
    }
}
