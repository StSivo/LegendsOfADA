using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        instance = this;
    }

    public TMP_Text playerManaText, enemyManaText, scoreMultiplierText;

    public TMP_Text challengeNameText, challengeFeatureText, enemyScoreText, playerScoreText;

    public GameObject manaWarning;
    public float manaWarningTime;
    private float manaWarningCounter;

    public int playerTotalScore, enemyTotalScore;

    private int currentEnemyScore, currentPlayerScore;
    private float currentEnemyMultiplier, currentPlayerMultiplier;

    public GameObject endTurnButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (manaWarningCounter > 0)
        {
            manaWarningCounter -= Time.deltaTime;

            if (manaWarningCounter <= 0)
            {
                manaWarning.SetActive(false);
            }
        }
    }

    public void UpdateEnemyScoreText(int enemyScore, float enemyMultiplier)
    {
        currentEnemyScore += enemyScore;
        currentEnemyMultiplier += enemyMultiplier;
        enemyTotalScore = (int)(currentEnemyScore * currentEnemyMultiplier);

        enemyScoreText.text = "Enemy: " + enemyTotalScore.ToString();
    }

    public void UpdatePlayerScoreText(int playerScore, float playerMultiplier)
    {
        currentPlayerScore += playerScore;
        currentPlayerMultiplier += playerMultiplier;
        playerTotalScore = (int)(currentPlayerScore * currentPlayerMultiplier);

        UpdateScoreMupltiplierText(currentPlayerMultiplier);

        Debug.Log("cs: " + currentPlayerScore + " cm: " + currentPlayerMultiplier);
        Debug.Log("ts: " + playerScore + " tm: " + playerMultiplier);
        Debug.Log("s: " + currentPlayerScore + " m: " + currentPlayerMultiplier + " tot: " + playerTotalScore);

        playerScoreText.text = "You: " + playerTotalScore.ToString();
    }

    public void SetChallengeNameText(string challengeName)
    {
        challengeNameText.text = challengeName;
    }

    public void SetChallengeFeatureText(string challengeFeature)
    {
        challengeFeatureText.text = challengeFeature;
    }

    public void SetPlayerScoreText(int playerScore)
    {
        currentPlayerScore = playerTotalScore = playerScore;
        currentPlayerMultiplier = 1f;
        playerScoreText.text = "You: " + playerTotalScore.ToString();
    }

    public void SetEnemyScoreText(int enemyScore)
    {
        currentEnemyScore = enemyTotalScore = enemyScore;
        currentEnemyMultiplier = 1f;
        enemyScoreText.text = "Enemy: " + enemyTotalScore.ToString();
    }

    public void SetPlayerManaText(int manaAmount)
    {
        playerManaText.text = "Mana: " + manaAmount;
    }

    public void SetEnemyManaText(int manaAmount)
    {
        enemyManaText.text = "Enemy Mana: " + manaAmount;
    }

    public void UpdateScoreMupltiplierText(float scoreMultiplier)
    {
        scoreMultiplierText.text = "Multiplier: " + scoreMultiplier + "x";
    }

    public void ShowManaWarning()
    {
        manaWarning.SetActive(true);
        manaWarningCounter = manaWarningTime;
    }

    public void EndPlayerTurn()
    {
        BattleController.instance.EndPlayerTurn();
    }

    internal void SetChallengeNameText(int v)
    {
        throw new NotImplementedException();
    }
}
