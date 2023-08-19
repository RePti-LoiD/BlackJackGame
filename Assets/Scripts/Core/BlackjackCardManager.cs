using TMPro;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BlackjackCardManager : MonoBehaviour
{
    [SerializeField] private BlackjackPlayer player, bot;

    [Header("---------------")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] public List<Card> cards = new List<Card>();
    [SerializeField] private List<BlackjackCard> spawnedCards = new List<BlackjackCard>(); 
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI turnText;
    
    [Header("Game end UI")]
    [SerializeField] private GameObject gameEndPanel;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI botScoreText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI continueText;

    [SerializeField] private string onPlayerWinMessage;
    [SerializeField] private string onBotWinMessage;
    [SerializeField] private string onDrawMessage;

    [Header("Other")]
    [SerializeField] private float botTimeToThink = 2f;

    public int MaxBlackjackScore = 21;

    private int currentCard = 0;

    private BlackjackPlayer currentPlayer;

    private void Start()
    {
        cards = cards.ShuffleList();

        string listToStr = string.Empty;
        foreach (Card card in cards)
        {
            GameObject spawnedCard = Instantiate(cardPrefab, transform);
            spawnedCard.GetComponent<BlackjackCard>().SetCardStruct(card);

            spawnedCards.Add(spawnedCard.GetComponent<BlackjackCard>());

            listToStr += card.CardWeight + " ";
        }
        Debug.Log(listToStr);

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
        print($"{currentPlayer.gameObject.name} is player {currentPlayer is BlackjackPlayer}");
        
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
                        resultText.text = onPlayerWinMessage;
                    else if (playerScore == botScore) 
                        resultText.text = onDrawMessage;
                    else if (playerScore < botScore) 
                        resultText.text = onBotWinMessage;
                }
                else
                {
                    if (playerScore > botScore)
                        resultText.text = onPlayerWinMessage;
                    else if (Mathf.Abs(MaxBlackjackScore - playerScore) < Mathf.Abs(MaxBlackjackScore - botScore))
                        resultText.text = onPlayerWinMessage;
                    else    
                        resultText.text = onBotWinMessage;
                }


            }));

            StartCoroutine(WaitToRestart());

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

    IEnumerator WaitToRestart()
    {   
        float timer = 0;

        while (true)
        {
            continueText.color = new Color(continueText.color.r, 
                continueText.color.g, 
                continueText.color.b, 
                Mathf.PingPong(Time.time, 1));

            timer += Time.deltaTime;

            #if UNITY_ANDROID && !UNITY_EDITOR
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                SceneManager.LoadScene(3);
                yield break;
            }
            #endif

            #if UNITY_STANDALONE_WIN || UNITY_EDITOR
            if (Input.anyKey)
            {
                SceneManager.LoadScene(3);
                yield break;
            }
            #endif

            if (timer > 5)
                break;

            yield return null;
        }

        NextSceneData nextSceneData = NextSceneData.Init();
        nextSceneData.SceneName = "MainMenu";
        nextSceneData.SceneIndex = 2;

        SceneManager.LoadScene(1);
    }
}