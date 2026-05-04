using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView player1;
    [SerializeField] private HandView player2;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;
    [SerializeField] private ChessBoardController chessBoardController;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private SwitchCamera switchCamera;
    private readonly List<Card> drawPile = new();
    private readonly List<Card> discardPile = new();
    private readonly List<Card> hand_player1 = new();
    private readonly List<Card> hand_player2 = new();

    void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        ActionSystem.SubscribeReaction<ReduceDurationGA>(DurationReducePreReaction, ReactionTiming.PRE);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
        ActionSystem.UnSubscribeReaction<ReduceDurationGA>(DurationReducePreReaction, ReactionTiming.PRE);
    }

    // Publics
    public void Setup(List<CardData> deckData)
    {
        foreach (var cardData in deckData)
        {
            Card card = new(cardData);
            drawPile.Add(card);
        }
    }

    // Performers
    private IEnumerator DrawCardsPerformer(DrawCardGA drawCardsGA)
    {
        int actualAmount = Mathf.Min(drawCardsGA.Amount, drawPile.Count);
        int notDrawnAmount = drawCardsGA.Amount - actualAmount;
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard(drawCardsGA.Player);
        }
    }

    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        foreach (var card in hand_player1)
        {
            discardPile.Add(card);
            CardView cardView = player1.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        hand_player1.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {

        if (switchCamera.Manager == 1)
        {
            hand_player1.Remove(playCardGA.Card);
            CardView cardView = player1.RemoveCard(playCardGA.Card);
            yield return DiscardCard(cardView);
        }
        else
        {
            hand_player2.Remove(playCardGA.Card);
            CardView cardView = player2.RemoveCard(playCardGA.Card); 
            yield return DiscardCard(cardView);        
        }

        // Debug.Log(playCardGA.targetGrid);
        // Perform effects
        foreach (Effect effect in playCardGA.Card.Effects)
        {
            PerformEffectGA performEffectGA = new(effect, playCardGA.targetGrid);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }

    // Reactions
    private void DurationReducePreReaction(ReduceDurationGA reduceDurationGA)
    {
        // Debug.Log("Reduce duration board effect -1");
        List<AddEffectOnBoardGA> boardEffects = new(chessBoardController.boardStatusEffect.Keys);

        foreach(AddEffectOnBoardGA boardEffect in boardEffects)
        {
            chessBoardController.boardStatusEffect[boardEffect]--;
            if (chessBoardController.boardStatusEffect[boardEffect] <= 0)
            {
                // Debug.Log($"effect on board {boardEffect.GridTarget}");
                placementSystem.RemoveEffectOnBoard(boardEffect.GridTarget);
                chessBoardController.boardStatusEffect.Remove(boardEffect);
            }
        }
    }

    //Helper
    private IEnumerator DrawCard(string player)
    {
        
        if(player == "player1")
        {
            Card card = drawPile.Draw();
            hand_player1.Add(card);
            CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation, 1);
            yield return player1.AddCard(cardView);
        }
        else if(player == "player2")
        {
            Card card = drawPile.Draw();
            hand_player2.Add(card);
            CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation, 2);
            yield return player2.AddCard(cardView);
        }
        else if(player == "both")
        {
            Card card_1 = drawPile.Draw();
            hand_player1.Add(card_1);
            CardView cardView_1 = CardViewCreator.Instance.CreateCardView(card_1, drawPilePoint.position, drawPilePoint.rotation, 1);
            yield return player1.AddCard(cardView_1);

            Card card_2 = drawPile.Draw();
            hand_player2.Add(card_2);
            CardView cardView_player2 = CardViewCreator.Instance.CreateCardView(card_2, drawPilePoint.position, drawPilePoint.rotation, 2);
            yield return player2.AddCard(cardView_player2);
        }
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        cardView.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }

}
