using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceEffectOnBoardSystem : MonoBehaviour
{
    [SerializeField] ChessBoardController chessBoardController;

    public void OnEnable()
    {
        ActionSystem.AttachPerformer<AddEffectOnBoardGA>(AddEffectOnBoardPerformer);
        ActionSystem.AttachPerformer<GravityCardGA>(ApplyGravityEffectPerformer);
        ActionSystem.AttachPerformer<ReverseTimeGA>(ApplyReverseTimeEffectPerformer);
        ActionSystem.AttachPerformer<PawnOfWarCardGA>(ApplyPawnOfWarEffectPerformer);
        ActionSystem.AttachPerformer<SelfDestructCardGA>(ApplySelfDestructEffectPerformer);
        ActionSystem.AttachPerformer<RedrawCardGA>(ApplyRedrawEffectPerformer);
        ActionSystem.AttachPerformer<BlockTurnCardGA>(AppluBlockTurnEffectPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AddEffectOnBoardGA>();
        ActionSystem.DetachPerformer<GravityCardGA>();
        ActionSystem.DetachPerformer<ReverseTimeGA>();
        ActionSystem.DetachPerformer<PawnOfWarCardGA>();
        ActionSystem.DetachPerformer<SelfDestructCardGA>();
        ActionSystem.DetachPerformer<RedrawCardGA>();
        ActionSystem.DetachPerformer<BlockTurnCardGA>();
    }    

    private IEnumerator AddEffectOnBoardPerformer(AddEffectOnBoardGA addEffectOnBoardGA)
    {
        chessBoardController.boardStatusEffect[addEffectOnBoardGA] = addEffectOnBoardGA.EffectDuration;
        chessBoardController.UpdateAllChessPieceStatusEffect();
        yield return null;
    }

    private IEnumerator ApplyGravityEffectPerformer(GravityCardGA gravityCardGA)
    {
        List<Piece> piecesToMove = new List<Piece>();
        foreach (var piece in ChessBoardController.Instance.piecesOnBoard)
        {
            if (piece.IsWhite() != ChessBoardController.Instance.isWhiteTurn)
            {
                // piecesToMove.Add(piece);
                // Debug.Log($"GravityCardGA performer: Adding {piece.pieceType} of {piece.pieceColor} at {piece.currentPosition} to piecesToMove");
            
                piecesToMove.Add(piece);
            }
        }


        if (!ChessBoardController.Instance.isWhiteTurn)
        {
            //sort from lowest to highest (A to H) value (black pieces move down)
            piecesToMove.Sort((a, b) => b.currentPosition[0].CompareTo(a.currentPosition[0]));
        }
        else
        {
            //sort from highest to lowest (H to A) value (white pieces move up)
            piecesToMove.Sort((a, b) => a.currentPosition[0].CompareTo(b.currentPosition[0]));
        }


        foreach (var piece in piecesToMove)
        {
            // move the piece down by gravityCardGA.moveAmount
            int direction = piece.IsWhite() ? 1 : -1;
            string newPosition = ChessBoardController.Instance.GetChessPosition(piece.currentPosition, 0, 1 * direction);
            if (newPosition != null && ChessBoardController.Instance.GetChessPieceAtPosition(newPosition) == null)
            {
                piece.MoveToPosition(newPosition);
            }
        }

        // Debug.Log("GravityCardGA performer: " + piecesToMove.Count + " pieces to move");
        return null;
    }

    private IEnumerator ApplyReverseTimeEffectPerformer(ReverseTimeGA reverseTimeGA)
    {

        Dictionary<string, GameObject> capturedPiecesSnapshot = new Dictionary<string, GameObject>(ChessBoardController.Instance.previousBoardState[0]);

        foreach (var piece in capturedPiecesSnapshot)
        {
            if(piece.Value != null)
            {
                piece.Value.SetActive(true);
                // Debug.Log($"ReverseTimeGA performer: Restoring {piece.Key} : {piece.Value}");
                piece.Value.GetComponent<Piece>().RestorePosition(piece.Key);
            }

        }

        return null;
    }

    private IEnumerator ApplyPawnOfWarEffectPerformer(PawnOfWarCardGA pawnOfWarCardGA)
    {

        List<Piece> piecesToMove = new List<Piece>();
        foreach (Piece piece in ChessBoardController.Instance.piecesOnBoard)
        {
            if (piece.IsWhite() == ChessBoardController.Instance.isWhiteTurn && piece is Pawn)
            {
                piecesToMove.Add(piece);
            }
        }

        foreach (Piece piece in piecesToMove)
        {
            for (int i = 0; i < pawnOfWarCardGA.directionX.Length; i++)
            {
                int dx = pawnOfWarCardGA.directionX[i];
                int dy = pawnOfWarCardGA.directionY[i];
                string nextPosition = chessBoardController.GetChessPosition(piece.currentPosition, dx, dy);

                if (nextPosition != null && ChessBoardController.Instance.GetChessPieceAtPosition(nextPosition) != null)
                {
                    Piece pieceAtNextPosition = ChessBoardController.Instance.GetChessPieceAtPosition(nextPosition).GetComponent<Piece>();
                    if(pieceAtNextPosition.pieceColor != piece.pieceColor)
                    {
                        piece.additionalPotentialMove.Add(nextPosition);
                    }
                }
            }
        }
        chessBoardController.pawnOfWar[pawnOfWarCardGA] = pawnOfWarCardGA.duration;

        return null;
    }

    private IEnumerator ApplySelfDestructEffectPerformer(SelfDestructCardGA selfDestructCardGA)
    {
        Piece pieceSelected = selfDestructCardGA.selectedPiece;

        int[] directionsX = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] directionsY = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < directionsX.Length; i++)
        {
            int dx = directionsX[i];
            int dy = directionsY[i];
            string nextPosition = chessBoardController.GetChessPosition(pieceSelected.currentPosition, dx, dy);

            if (nextPosition != null && ChessBoardController.Instance.GetChessPieceAtPosition(nextPosition) != null)
            {
                Piece pieceAtNextPosition = ChessBoardController.Instance.GetChessPieceAtPosition(nextPosition).GetComponent<Piece>();
                pieceAtNextPosition.gameObject.SetActive(false);
            }
        }

        pieceSelected.gameObject.SetActive(false);

        return null;
    }

    private IEnumerator ApplyRedrawEffectPerformer(RedrawCardGA redrawCardGA)
    {
        foreach (var card in CardSystem.Instance.hand_player1)
        {
            CardSystem.Instance.drawPile.Add(card);
            CardView cardView = CardSystem.Instance.player1.RemoveCard(card);
            yield return CardSystem.Instance.PutCardBackInDeck(cardView);
        }

        foreach (var card in CardSystem.Instance.hand_player2)
        {
            CardSystem.Instance.drawPile.Add(card);
            CardView cardView = CardSystem.Instance.player2.RemoveCard(card);
            yield return CardSystem.Instance.PutCardBackInDeck(cardView);
        }

        CardSystem.Instance.ShuffleCard();

        yield return null;
    }

    private IEnumerator AppluBlockTurnEffectPerformer(BlockTurnCardGA blockTurnCardGA)
    {
        
        ChessBoardController.Instance.skipTurn = true;
        return null;
    }

}