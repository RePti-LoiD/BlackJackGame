using TMPro;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlackjackCardManager : MonoBehaviour
{
    [SerializeField] private BlackjackPlayer player, bot;

    [Header("---------------")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] public List<Card> cards = new List<Card>();
    [SerializeField] private List<BlackjackCard> spawnedCards = new List<BlackjackCard>();
    [SerializeField] private PlayerBet playerBet; 
    [SerializeField] private User user;


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
        playAgainButton.onClick.AddListener(() =>
        {
            ReloadScene();
        });

        returnToMenuButton.onClick.AddListener(() =>
        {
            BackToMenu();
        });

        playerBet.OnPlayerBet += (count, userData) =>
        {
            playerBetAmount = botBetAmount = count;

            playerBetText.text = count.ToString();
            botBetText.text = count.ToString();
            this.userData = userData;

            StartGame();
        };

        nickNameText.text = UserDataWrapper.UserData.NickName;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(3);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(2);
    }

    private void StartGame()
    {
        cards = cards.ShuffleList();

        int renderIndex = 0;
        foreach (Card card in cards)
        {
            renderIndex++;
            GameObject spawnedCard = Instantiate(cardPrefab, transform);
            spawnedCard.GetComponent<BlackjackCard>().SetCardStruct(card, renderIndex);

            spawnedCards.Add(spawnedCard.GetComponent<BlackjackCard>());
        }

        // это какой-то пиздец. надо будет убрать
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
        moneyText.text = $"<color=yellow><s>{userData.UserWallet.Balance}</s></color> -> <color=green>{userData.UserWallet.Balance + botBetAmount}</color>";
        userData.UserWallet.AddMoney(botBetAmount * 2);

        print("Player win " + userData.UserWallet.Balance);
    }

    void BotWin()
    {
        resultText.text = onBotWinMessage;
        moneyText.text = $"<color=yellow><s>{userData.UserWallet.Balance}</s></color> -> <color=red>{userData.UserWallet.Balance - playerBetAmount}</color>";
        /*userData.UserWallet.AddMoney(-playerBetAmount);*/

        print("Bot win " + userData.UserWallet.Balance);
    }

    void Draw()
    {
        resultText.text = onDrawMessage;

        moneyText.text = $"<s>{userData.UserWallet.Balance}</s> -> {userData.UserWallet.Balance}";
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