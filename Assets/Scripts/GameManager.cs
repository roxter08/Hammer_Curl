using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [SerializeField] GridLayoutGroup gridRoot;
    [SerializeField] int rows;
    [SerializeField] int columns;

    public List<Sprite> cardImages; // List of card images
    public Card cardPrefab; // Prefab for the card

    private Card firstSelectedCard;
    private Card secondSelectedCard;

    private Queue<CardPair> cardPairsQueue;
    private Coroutine cardPairCheckCoroutine;

    private void Awake()
    {
        cardPairsQueue = new Queue<CardPair>();
        cardPairCheckCoroutine = null;
    }

    private void Start()
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
            GameObject cardObject = Instantiate(cardPrefab.gameObject, gridRoot.transform, false);
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

    private void Update()
    {
        CheckForMatch();
    }

    private void ResetSelectedCards()
    {
        firstSelectedCard = null;
        secondSelectedCard = null;
    }

    private async void CheckForMatch()
    {
        for (int i = cardPairsQueue.Count - 1; i >= 0; i--)
        {
            CardPair cardPair = cardPairsQueue.Dequeue();
            await cardPair.CheckForMatch();
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

    public CardPair(Card firstCard, Card secondCard)
    {
        this.firstCard = firstCard;
        this.secondCard = secondCard;
    }

    public async Task CheckForMatch()
    {
        await Task.Delay(500);
        if (FirstCard.cardImage == SecondCard.cardImage)
        {
            FirstCard.MatchFound();
            SecondCard.MatchFound();
        }
        else
        {
            FirstCard.HideCard();
            SecondCard.HideCard();
        }
    }
}
