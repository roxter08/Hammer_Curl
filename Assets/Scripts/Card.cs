using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image cardImageComponent;
    [SerializeField] private Sprite cardBackImage;
    [SerializeField] private AudioClip cardFlipAudio;

    [HideInInspector]
    public Sprite cardImage;

    private GameController gameManager;
    private bool isRevealed = false;

    public void Initialize(Sprite image, GameController manager)
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
        SoundManager.GetInstance().PlayAudio(cardFlipAudio);
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
