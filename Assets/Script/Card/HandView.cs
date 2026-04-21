using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{

    [SerializeField] private SplineContainer splineContainer;
    private readonly List<CardView> cards = new();

    public IEnumerator AddCard(CardView cardView)
    {
        cards.Add(cardView);
        yield return UpdateCardPositions(0.15f);
    }

    public CardView RemoveCard(Card card)
    {
        CardView cardView = GetCardView(card);
        if(cardView == null) return null;
        cards.Remove(cardView);
        StartCoroutine(UpdateCardPositions(0.15f));
        return cardView;
    }

    private CardView GetCardView(Card card)
    {
        return cards.Where(cardView => cardView.Card == card).FirstOrDefault();
    }

    private IEnumerator UpdateCardPositions(float duration)
    {
        if (cards.Count == 0) yield break;

        float cardSpacing = 1f / 10f;
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;

        for (int i = 0; i < cards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;

            Vector3 splineLocalPos = spline.EvaluatePosition(p);
            Vector3 worldPosition = transform.TransformPoint(splineLocalPos);

            // Use spline tangent for fan rotation, not camera direction
            Vector3 forward = transform.TransformDirection(spline.EvaluateTangent(p));
            Vector3 up = transform.TransformDirection(spline.EvaluateUpVector(p));
            Quaternion rotation = Quaternion.LookRotation(forward, up) * Quaternion.Euler(0, -90, 0);

            // Linear z-offset so no card pops forward
            Vector3 finalPosition = worldPosition + 0.01f * i * Vector3.back;

            cards[i].transform.DOMove(finalPosition, duration);
            cards[i].transform.DORotateQuaternion(rotation, duration);
        }

        yield return new WaitForSeconds(duration);
    }
}
