using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

internal sealed class GameController : MonoBehaviour
{
    [Header("Buttons")]
    public Button attackButton;
    public Button mainMenuBack;
    public Button resultButton;
    
    [Header("CanvasGroup")]
    public CanvasGroup buttonPanel;
    public CanvasGroup menuSettingsCanvasGroup;
    public CanvasGroup resultPanel;
    
    [Header("Characters")]
    public Character[] playerCharacter;
    public Character[] enemyCharacter;
    
    [Header("Text")]
    public TextMeshProUGUI resultText;
    
    Character currentTarget;
    bool waitingForInput;
    private Scene n_scene;
    private string nameScene;

    Character FirstAliveCharacter(Character[] characters)
    {
        return characters.FirstOrDefault(character => !character.IsDead());
    }

    void PlayerWon()
    {
        resultText.text = "Player won.";
        Utility.SetCanvasGroupEnabled(resultPanel, true);
    }

    void PlayerLost()
    {
        resultText.text = "Player lost.";
        Utility.SetCanvasGroupEnabled(resultPanel, true);
    }

    bool CheckEndGame()
    {
        if (FirstAliveCharacter(playerCharacter) == null) {
            PlayerLost();
            return true;
        }

        if (FirstAliveCharacter(enemyCharacter) == null) {
            PlayerWon();
            return true;
        }

        return false;
    }

    //[ContextMenu("Player Attack")]
    public void PlayerAttack()
    {
        waitingForInput = false;
    }

    //[ContextMenu("Next Target")]
    public void NextTarget()
    {
        int index = Array.IndexOf(enemyCharacter, currentTarget);
        for (int i = 1; i < enemyCharacter.Length; i++) {
            int next = (index + i) % enemyCharacter.Length;
            if (!enemyCharacter[next].IsDead()) {
                currentTarget.targetIndicator.gameObject.SetActive(false);
                currentTarget = enemyCharacter[next];
                currentTarget.targetIndicator.gameObject.SetActive(true);
                return;
            }
        }
    }

    IEnumerator GameLoop()
    {
        yield return null;
        while (!CheckEndGame()) {
            foreach (var player in playerCharacter)
            {
                if (player.IsDead())
                {
                    continue;
                }
                
                currentTarget = FirstAliveCharacter(enemyCharacter);
                if (currentTarget == null)
                    break;

                currentTarget.targetIndicator.gameObject.SetActive(true);
                Utility.SetCanvasGroupEnabled(buttonPanel, true);

                waitingForInput = true;
                while (waitingForInput)
                    yield return null;

                Utility.SetCanvasGroupEnabled(buttonPanel, false);
                currentTarget.targetIndicator.gameObject.SetActive(false);

                player.target = currentTarget.transform;
                player.AttackEnemy();

                while (!player.IsIdle())
                    yield return null;

                break;
            }

            foreach (var enemy in enemyCharacter)
            {
                if(enemy.IsDead())
                {
                    continue;
                }
                
                Character target = FirstAliveCharacter(playerCharacter);
                if (target == null)
                    break;

                enemy.target = target.transform;
                enemy.AttackEnemy();

                while (!enemy.IsIdle())
                    yield return null;

                break;
            }
        }
    }

    public void LoadSettingsMenu()
    {
        Utility.SetCanvasGroupEnabled(menuSettingsCanvasGroup, true);
        Utility.SetCanvasGroupEnabled(buttonPanel, false);
    }

    public void ContinueButton()
    {
        Utility.SetCanvasGroupEnabled(menuSettingsCanvasGroup, false);
        Utility.SetCanvasGroupEnabled(buttonPanel, true);
    }

    public void RestartLevelButton()
    {
        SceneManager.LoadScene(nameScene);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    void Start()
    {
        n_scene = SceneManager.GetActiveScene();
        nameScene = n_scene.name;
        mainMenuBack.onClick.AddListener(BackToMainMenu);
        attackButton.onClick.AddListener(PlayerAttack);
        resultButton.onClick.AddListener(BackToMainMenu);
        Utility.SetCanvasGroupEnabled(buttonPanel, false);
        Utility.SetCanvasGroupEnabled(menuSettingsCanvasGroup, false);
        Utility.SetCanvasGroupEnabled(resultPanel, false);
        StartCoroutine(GameLoop());
    }
}