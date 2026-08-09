using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private WaveManager waveManager;

    [Header("Información")]
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text enemyText;

    [Header("Resultado")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button restartButton;

    private void Start()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(
                RestartGame
            );
        }

        UpdateUI();
    }

    private void Update()
    {
        if (waveManager == null)
            return;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (waveManager == null)
            return;

        UpdateStateText();
        UpdateWaveText();
        UpdateEnemyText();
        UpdateResult();
    }

    private void UpdateStateText()
    {
        if (stateText == null)
            return;

        switch (waveManager.CurrentState)
        {
            case GameState.Spawning:
                stateText.text =
                    "Generando enemigos...";
                break;

            case GameState.Playing:
                stateText.text =
                    "¡Combate!";
                break;

            case GameState.Waiting:
                stateText.text =
                    "Preparando siguiente oleada...";
                break;

            case GameState.Win:
                stateText.text =
                    "¡VICTORIA!";
                break;

            case GameState.Lose:
                stateText.text =
                    "¡DERROTA!";
                break;
        }
    }

    private void UpdateWaveText()
    {
        if (waveText == null)
            return;

        waveText.text =
            $"Oleada: {waveManager.CurrentWave} / " +
            $"{waveManager.TotalWaves}";
    }

    private void UpdateEnemyText()
    {
        if (enemyText == null)
            return;

        enemyText.text =
            $"Enemigos: {waveManager.ActiveEnemies}";
    }

    private void UpdateResult()
    {
        if (resultPanel == null)
            return;

        if (waveManager.CurrentState == GameState.Win)
        {
            ShowResult("Victoria!!!! \r\n(YA TERMINARON LOS EXAMENES)");
        }
        else if (
            waveManager.CurrentState ==
            GameState.Lose)
        {
            ShowResult("DERROTA!!! ;-; \r\n(TENDRAS QUE REPETIR EXAMENES)");
        }
    }

    private void ShowResult(string message)
    {
        if (!resultPanel.activeSelf)
        {
            resultPanel.SetActive(true);
        }

        if (resultText != null)
        {
            resultText.text = message;
        }
    }

    private void RestartGame()
    {
        waveManager.RestartGame();
    }
}
