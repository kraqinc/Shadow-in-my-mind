using UnityEngine;
using UnityEngine.SceneManagement;
using ShadowInMyMind.Sanity;
using ShadowInMyMind.Shadow;
using ShadowInMyMind.Player;
using ShadowInMyMind.Audio;
using ShadowInMyMind.UI;

namespace ShadowInMyMind.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        public GameState CurrentState = GameState.Playing;

        [Header("References")]
        public SanitySystem SanitySystem;
        public ShadowController ShadowController;
        public PlayerController Player;

        public event System.Action<GameState> OnGameStateChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetState(GameState.Playing);
            AudioManager.Instance?.PlayAmbience();
        }

        public void SetState(GameState newState)
        {
            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);

            switch (newState)
            {
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.MentalBreakdown:
                    ShadowController?.TriggerBreakdown();
                    break;
                case GameState.GameOver:
                    Time.timeScale = 0f;
                    UIManager.Instance?.ShowDeathScreen();
                    break;
            }
        }

        public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        public void LoadMainMenu() => SceneManager.LoadScene("MainMenu");
    }

    public enum GameState { Playing, Paused, MentalBreakdown, GameOver }
}
