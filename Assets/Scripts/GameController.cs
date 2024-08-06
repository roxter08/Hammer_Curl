using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using Random = UnityEngine.Random;

public class GameController
{
    private GridLayoutGroup gridRoot;
    private int rows;
    private int columns;

    public List<Sprite> cardImages; // List of card images
    public Card cardPrefab; // Prefab for the card

    private Card firstSelectedCard;
    private Card secondSelectedCard;
    private Queue<CardPair> cardPairsQueue;
    private GameCallbacks gameCallbacks;

    public GameController(GridLayoutGroup gridLayout, int rows, int columns, List<Sprite> cardImages, Card cardPrefab, GameCallbacks gameCallbacks) 
    {
        this.rows = rows;
        this.columns = columns;
        this.gridRoot = gridLayout;
        this.cardImages = cardImages;
        this.cardPrefab = cardPrefab;
        this.gameCallbacks = gameCallbacks;

        cardPairsQueue = new Queue<CardPair>();
    }

    public void Initialize()
    {
        RectTransform gridRectTransform = gridRoot.transform as RectTransform;
        if (rows > columns)
        {
            gridRoot.cellSize = new Vector3(gridRectTransform.rect.width / columns, gridRectTransform.rect.height / rows, 0);
            gridRoot.constraintCount = columns;
        }
        else
        {
            gridRoot.cellSize = new Vector3(gridRectTransform.rect.width / rows, gridRectTransform.rect.height / columns, 0);
            gridRoot.constraintCount = rows;
        }

        InitializeCards();
    }

    private void InitializeCards()
    {
        int maxCount = rows * columns;
        List<Sprite> images = PickRandom(cardImages, maxCount/2);
        images.AddRange(images); // Duplicate images for pairs
        images = Shuffle(images); // Shuffle images
        List<Card> cards = new();
        for (int i = 0; i < maxCount; i++)
        {
            GameObject cardObject = GameObject.Instantiate(cardPrefab.gameObject, gridRoot.transform, false);
            Card card = cardObject.GetComponent<Card>();
            card.Initialize(images[i], this);
        }
    }

    public void CardSelected(Card selectedCard)
    {
        if (firstSelectedCard == null)
        {
            firstSelectedCard = selectedCard;
        }
        else if (secondSelectedCard == null && selectedCard != firstSelectedCard)
        {
            secondSelectedCard = selectedCard;
            cardPairsQueue.Enqueue(new CardPair(firstSelectedCard, secondSelectedCard));
            ResetSelectedCards();
        }
    }

    private void ResetSelectedCards()
    {
        firstSelectedCard = null;
        secondSelectedCard = null;
    }

    public async void CheckForMatch()
    {
        for (int i = cardPairsQueue.Count - 1; i >= 0; i--)
        {
            CardPair cardPair = cardPairsQueue.Dequeue();
            await cardPair.CheckForMatch((value)=>
            {
                gameCallbacks.RaiseMatchFoundEvent(value.HasMatched);
            });
            
            gameCallbacks.RaiseTurnUpdateEvent();
        }
    }

    List<Sprite> Shuffle(List<Sprite> list)
    {
        for (int i = list.Count - 1; i > 0; i--)//
        {
            int randomIndex = Random.Range(0, i + 1);
            Sprite temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    List<Sprite> PickRandom(List<Sprite> list, int maxCount)
    {
        List<Sprite> result = new();
        for (int i = 0; i<maxCount; i++)//
        {
            int randomIndex = Random.Range(0, list.Count);
            result.Add(list[randomIndex]);
        }
        return result;
    }
}

public struct CardPair
{
    private Card firstCard;
    private Card secondCard;
    public Card FirstCard => firstCard;
    public Card SecondCard => secondCard;

    public bool HasMatched { get; private set; }

    public CardPair(Card firstCard, Card secondCard)
    {
        this.firstCard = firstCard;
        this.secondCard = secondCard;
        HasMatched = false;
    }

    public async Task CheckForMatch(Action<CardPair> OnComplete)
    {
        await Task.Delay(500);
        if (FirstCard.cardImage == SecondCard.cardImage)
        {
            FirstCard.MatchFound();
            SecondCard.MatchFound();
            HasMatched = true;
        }
        else
        {
            FirstCard.HideCard();
            SecondCard.HideCard();
            HasMatched = false;
        }

        if(OnComplete != null)
        {
            OnComplete.Invoke(this);
        }
    }
}
