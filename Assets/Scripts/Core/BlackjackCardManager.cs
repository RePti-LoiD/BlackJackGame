using TMPro;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;

public class BlackjackCardManager : MonoBehaviour
{
    [SerializeField] private BlackjackPlayer player, bot;

    [Header("---------------")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] public List<Card> cards = new List<Card>();
    [SerializeField] private List<BlackjackCard> spawnedCards = new List<BlackjackCard>();
    [SerializeField] private PlayerBet playerBet; 
    [SerializeField] private UserDataLoader userDataLoader;


    [Header("UI")]
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI nickNameText;
    [SerializeField] private TextMeshProUGUI playerBetText;
    [SerializeField] private TextMeshProUGUI botBetText;

    [Header("Game end UI")]
    [SerializeField] private GameObject gameEndPanel;
    [SerializeField] private TextMeshProUGUI botScoreText;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button returnToMenuButton;

    [SerializeField] private string onPlayerWinMessage;
    [SerializeField] private string onBotWinMessage;
    [SerializeField] private string onDrawMessage;

    [Header("Other")]
    [SerializeField] private float botTimeToThink = 2f;

    public int MaxBlackjackScore = 21;

    private int currentCard = 0;

    private int playerBetAmount, botBetAmount;

    private BlackjackPlayer currentPlayer;
    private User userData;

    private void Start()
    {
        if (PlayerPrefs.GetString(PlayerPrefsKeys.IsGuest) == IsGuest.Guest.ToString())
            nickNameText.text = "Guest";

        playAgainButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(3);
        });

        returnToMenuButton.onClick.AddListener(() =>
        {
            NextSceneData nextSceneData = NextSceneData.Init();
            nextSceneData.SceneName = "MainMenu";
            nextSceneData.SceneIndex = 2;

            SceneManager.LoadScene(1);
        });

        playerBet.OnPlayerBet += (count, userData) =>
        {
            playerBetAmount = botBetAmount = count;

            playerBetText.text = count.ToString();
            botBetText.text = count.ToString();
            this.userData = userData;

            StartGame();
        };

        userDataLoader.OnDataLoad += (data) =>
        {
            nickNameText.text = data.NickName;
        };
    }

    private void StartGame()
    {
        cards = cards.ShuffleList();

        foreach (Card card in cards)
        {
            GameObject spawnedCard = Instantiate(cardPrefab, transform);
            spawnedCard.GetComponent<BlackjackCard>().SetCardStruct(card);

            spawnedCards.Add(spawnedCard.GetComponent<BlackjackCard>());
        }

        // это какой-то пиздец и надо будет убрать
        StartCoroutine(Duration(.0f, () =>
        {
            player.SetCard(PutCard());

            StartCoroutine(Duration(.5f, () =>
            {
                bot.SetCard(PutCard());

                StartCoroutine(Duration(.5f, () =>
                {
                    player.SetCard(PutCard());

                    StartCoroutine(Duration(.5f, () =>
                    {
                        bot.SetCard(PutCard());

                        SetCurrentPlayer(player);
                    }));
                }));
            }));
        }));
    }

    public void NextTurn()
    {        
        if (bot.TurnState == TurnState.Stand && player.TurnState == TurnState.Stand)
        {
            turnText.text = "Game ended";

            StartCoroutine(Duration(1f, () => 
            {
                gameEndPanel.SetActive(true);

                int playerScore = player.CalculateScore(), botScore = bot.CalculateScore();

                playerScoreText.text = playerScore.ToString();

                botScoreText.text = botScore.ToString();

                if (playerScore <= MaxBlackjackScore && botScore <= MaxBlackjackScore)
                {
                    if (playerScore > botScore)
                        PlayerWin();
                    else if (playerScore < botScore)
                        BotWin();
                }
                else if (playerScore > MaxBlackjackScore && botScore > MaxBlackjackScore)
                {
                    if (playerScore < botScore)
                        PlayerWin();
                    else if (playerScore > botScore)
                        BotWin();
                }
                else if (playerScore > MaxBlackjackScore && botScore <= MaxBlackjackScore)
                {
                    BotWin();
                }
                else if (botScore > MaxBlackjackScore && playerScore <= MaxBlackjackScore)
                {
                    PlayerWin();
                }
                else
                {
                    Draw();
                }
            }));

            return;
        }

        if (currentPlayer == bot)
        {
            SetCurrentPlayer(player);
        }

        else if (currentPlayer == player)
        {
            SetCurrentPlayer(bot);

            StartCoroutine(Duration(botTimeToThink, () => EndTurn()));
        }
    }

    void PlayerWin()
    {
        resultText.text = onPlayerWinMessage;
        moneyText.text = $"<color=yellow><s>{userData.PlayerBalance}</s></color> -> <color=green>{userData.PlayerBalance + botBetAmount}</color>";
        userData.PlayerBalance += botBetAmount;

        if (PlayerPrefs.GetString(PlayerPrefsKeys.IsGuest) == IsGuest.Guest.ToString())
            PlayerPrefs.SetInt(PlayerPrefsKeys.UserScore, userData.PlayerBalance);
        else
            userDataLoader.LoadUserBalance(userData.PlayerBalance);
    }

    void BotWin()
    {
        resultText.text = onBotWinMessage;
        moneyText.text = $"<color=yellow><s>{userData.PlayerBalance}</s></color> -> <color=red>{userData.PlayerBalance - playerBetAmount}</color>";
        userData.PlayerBalance -= playerBetAmount;
       
        if (PlayerPrefs.GetString(PlayerPrefsKeys.IsGuest) == IsGuest.Guest.ToString())
            PlayerPrefs.SetInt(PlayerPrefsKeys.UserScore, userData.PlayerBalance);
        else
            userDataLoader.LoadUserBalance(userData.PlayerBalance);
    }

    void Draw()
    {
        resultText.text = onDrawMessage;

        moneyText.text = $"<s>{UserDataLoader.UserData.PlayerBalance}</s> -> {UserDataLoader.UserData.PlayerBalance}";
    }

    public void EndTurn()
    {
        switch(currentPlayer.OnTurnEnd())
        {
            case TurnState.Stand:
                break;
            case TurnState.Hit:
                currentPlayer.SetCard(PutCard());
                break;
        }

        NextTurn();
    }

    private void SetCurrentPlayer(BlackjackPlayer blackjackPlayer)
    {
        if (blackjackPlayer is BlackjackBotPlayer) 
            turnText.text = "Bot turn";
        else 
            turnText.text = "Your turn";

        currentPlayer = blackjackPlayer;
        
        currentPlayer.OnTurn(this);
    }

    public BlackjackCard PutCard() 
    {
        if (currentCard + 1 > spawnedCards.Count - 1) return null;

        BlackjackCard blackjackCard = spawnedCards[currentCard];

        currentCard++;

        return blackjackCard;
    }

    IEnumerator Duration(float seconds, Action action) 
    {
        yield return new WaitForSeconds(seconds);

        action?.Invoke();
    }
}