using UnityEngine;

public class GamePause_UI_Element : MonoBehaviour
{
    private GameObject pauseParent;
    private CharacterController2D characterController;

    private bool gameIsPausedByPlayer;

    private void Awake()
    {
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        pauseParent = GameObject.FindGameObjectWithTag("PauseUIParent");
    }

    private void Start()
    {
        pauseParent.SetActive(false);

        characterController.OnPauseButtonPress += OnPauseButton;
    }

    private void OnDestroy()
    {
        characterController.OnPauseButtonPress -= OnPauseButton;
    }

    private void OnPauseButton()
    {
        gameIsPausedByPlayer = !gameIsPausedByPlayer;
        pauseParent.SetActive(gameIsPausedByPlayer);
    }
}
