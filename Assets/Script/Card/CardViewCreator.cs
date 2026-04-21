using DG.Tweening;
using UnityEngine;

public class CardViewCreator : Singleton<CardViewCreator>
{

    [SerializeField] private CardView cardViewPrefab;
    
    public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation)
    {
        CardView cardView = Instantiate(cardViewPrefab, position, rotation);
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(new Vector3(0.07f, 0.07f, 0.07f), 0.15f);
        cardView.Setup(card);
        return cardView;
    }

}
