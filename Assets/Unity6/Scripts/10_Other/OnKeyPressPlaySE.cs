using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AudioSource))]

// アクションキーを押すと、効果音を鳴らす
public class OnKeyPressPlaySE : MonoBehaviour
{
	//-------------------------------------
    public AudioClip jumpSE;
    public AudioClip attackSE;
	//-------------------------------------

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnJump()
    {
        audioSource.PlayOneShot(jumpSE);
    }

    public void OnAttack()
    {
        audioSource.PlayOneShot(attackSE);
    }
}
