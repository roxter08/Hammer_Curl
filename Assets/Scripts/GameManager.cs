using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GridLayoutGroup gridRoot;
    [SerializeField] int rows;
    [SerializeField] int columns;

    public List<Sprite> cardImages; // List of card images
    public Card cardPrefab; // Prefab for the card
    //public Transform cardParent; // Parent object to hold cards
    private List<Card> cards = new List<Card>(); // List to hold card instances

    private Card firstSelectedCard;
    private Card secondSelectedCard;

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

        //for (int i = 0; i < columns; i++)
        //{
        //    for (int j = 0; j < rows; j++)
        //    {
        //        GameObject cardObjects = Instantiate(card, gridRoot.transform, false);
        //        cardObjects.GetComponent<Image>().color = Random.ColorHSV();
        //    }
        //}

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
            StartCoroutine(CheckForMatch());
        }
    } 

    IEnumerator CheckForMatch()
    {
        yield return new WaitForSeconds(1f);

        if (firstSelectedCard.cardImage == secondSelectedCard.cardImage)
        {
            firstSelectedCard.MatchFound();
            secondSelectedCard.MatchFound();
        }
        else
        {
            firstSelectedCard.HideCard();
            secondSelectedCard.HideCard();
        }

        firstSelectedCard = null;
        secondSelectedCard = null;
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
