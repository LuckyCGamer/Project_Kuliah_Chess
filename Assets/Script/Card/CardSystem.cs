using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] public HandView player1;
    [SerializeField] public HandView player2;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;
    [SerializeField] private ChessBoardController chessBoardController;
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private SwitchCamera switchCamera;
    public readonly List<Card> drawPile = new();
    private readonly List<Card> discardPile = new();
    public List<Card> hand_player1 = new();
    public List<Card> hand_player2 = new();

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

    public void ShuffleCard()
    {
        // Debug.Log("shuffle card");
		int count = drawPile.Count;
		int last = count - 1;
		for (int i = 0; i < last; ++i) {
			int r = UnityEngine.Random.Range(i, count);
			Card tmp = drawPile[i];
			drawPile[i] = drawPile[r];
			drawPile[r] = tmp;
		}
    }

    // Performers
    private IEnumerator DrawCardsPerformer(DrawCardGA drawCardsGA)
    {
        int actualAmount = Mathf.Min(drawCardsGA.Amount, drawPile.Count);
        int notDrawnAmount = drawCardsGA.Amount - actualAmount;
        if(drawCardsGA.Player == "both")
        {
            for (int i = 0; i < actualAmount; i++)
            {
                if(drawPile.Count == 0)
                {
                    // Debug.Log("cannot draw card");
                    break;
                }
                yield return DrawCard("player1");

                if(drawPile.Count == 0)
                {
                    // Debug.Log("cannot draw card");
                    break;
                }
                yield return DrawCard("player2");
            }
        }
        else if (drawCardsGA.Player == "player1")
        {
            for (int i = 0; i < actualAmount; i++)
            {
                Debug.Log(drawPile.Count);
                if(drawPile.Count == 0)
                {
                    // Debug.Log("cannot draw card");
                    break;
                }
                yield return DrawCard("player1");
            }
        }
        else
        {
            for (int i = 0; i < actualAmount; i++)
            {
                Debug.Log(drawPile.Count);
                if(drawPile.Count == 0)
                {
                    // Debug.Log("cannot draw card");
                    break;
                }
                yield return DrawCard("player2");
            }           
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
            if(playCardGA.targetGrid != Vector3Int.zero)
            {
                PerformEffectGA performEffectGA = new(effect, playCardGA.targetGrid);
                ActionSystem.Instance.AddReaction(performEffectGA);
            }
            else if (playCardGA.SelectedPiece != null)
            {
                PerformEffectGA performEffectGA = new(effect, playCardGA.SelectedPiece);
                ActionSystem.Instance.AddReaction(performEffectGA);
            }
            else
            {
                PerformEffectGA performEffectGA = new(effect);
                ActionSystem.Instance.AddReaction(performEffectGA);               
            }

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

        List<PawnOfWarCardGA> pawnOfWars = new(chessBoardController.pawnOfWar.Keys);
        foreach(PawnOfWarCardGA pawnOfWarCard in pawnOfWars)
        {
            chessBoardController.pawnOfWar[pawnOfWarCard]--;
            if (chessBoardController.pawnOfWar[pawnOfWarCard] <= 0)
            {
                foreach (Piece piece in ChessBoardController.Instance.piecesOnBoard)
                {
                    if (piece.IsWhite() != ChessBoardController.Instance.isWhiteTurn && piece is Pawn)
                    {
                        piece.additionalPotentialMove.Clear();
                    }
                }
                
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
    }

    public IEnumerator DiscardCard(CardView cardView)
    {
        cardView.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }

    public IEnumerator PutCardBackInDeck(CardView cardView)
    {
        cardView.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOMove(drawPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }

}
