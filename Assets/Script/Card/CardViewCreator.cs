using DG.Tweening;
using UnityEngine;

public class CardViewCreator : Singleton<CardViewCreator>
{

    [SerializeField] private CardView cardViewPrefab;
    
    public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation)
    {
        float cardScale = 0.05f;
        CardView cardView = Instantiate(cardViewPrefab, position, rotation);
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(new Vector3(cardScale, cardScale, cardScale), 0.15f);
        cardView.Setup(card);
        return cardView;
    }

}
