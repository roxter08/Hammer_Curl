using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image cardImageComponent;
    public Sprite cardBackImage;

    [HideInInspector]
    public Sprite cardImage;

    private GameManager gameManager;
    private bool isRevealed = false;

    public void Initialize(Sprite image, GameManager manager)
    {
        cardImage = image;
        cardImageComponent.sprite = cardBackImage;
        gameManager = manager;
    }

    public void OnCardClicked()
    {
        if (isRevealed) return;

        RevealCard();
        gameManager.CardSelected(this);
    }

    public void RevealCard()
    {
        cardImageComponent.sprite = cardImage;
        isRevealed = true;
    }

    public void HideCard()
    {
        cardImageComponent.sprite = cardBackImage;
        isRevealed = false;
    }

    public void MatchFound()
    {
        // Add logic for matched cards (e.g., disable further interaction)
        GetComponent<Button>().interactable = false;
    }
}
