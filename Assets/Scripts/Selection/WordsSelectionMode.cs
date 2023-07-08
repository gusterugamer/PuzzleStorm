using GusteruStudio.Extensions;
using GusteruStudio.PuzzleStorm;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GusteruStudio.Selection
{
    [CreateAssetMenu(menuName = "PuzzleStorm/WordsSortSelection")]
    public sealed class WordsSelectionMode : SelectionMode
    {
        [BoxGroup("WordsMaster")][SerializeField] private WordsMaster _wm;
        private Letter _firstLetter = null;
        private Letter _secondLetter = null;

        public override void Set()
        {
            SelectionManager.SetMode(this);
        }

        public override void Enter()
        {
            TouchInput.onPieceSelected += ProcessSelection;
            TouchInput.onPointerDown += ClearLetter;
            ClearLetter(null);
        }

        public override void Exit()
        {
            TouchInput.onPieceSelected -= ProcessSelection;
            TouchInput.onPointerDown -= ClearLetter;
        }

        protected override void ProcessSelection(PuzzlePiece pp)
        {
            Letter letter = pp as Letter;

            if (_firstLetter == letter || _secondLetter == letter)
                return;

            if (_firstLetter == null)
            {
                _firstLetter = letter;
                Debug.Log("SELECTED" + "<color=" + Color.red.ToRGBHex() + ">" + " START " + "</color>" + "LETTER << " + _firstLetter.name + " >>", _firstLetter.gameObject);
                return;
            }

            if (_firstLetter != letter && _secondLetter != letter)
            {
                _secondLetter = ValidateSecondLetter(_firstLetter,letter);
                Debug.Log("SELECTED" + "<color=" + Color.green.ToRGBHex() + ">" + " END " + "</color>" + "LETTER << " + _secondLetter.name + " >>", _secondLetter.gameObject);
                Debug.DrawLine(_firstLetter.transform.position, _secondLetter.transform.position,Color.black, 0.5f);
            }

            Debug.Log("Letter1: " + _firstLetter.name + "Letter2: " + _secondLetter.name);
        }

        private void ClearLetter(PointerEventData eventData)
        {
            _firstLetter = null;
            _secondLetter = null;

            Debug.Log("LETTER SELECTION IS CLEAR");
        }

        private Letter ValidateSecondLetter(Letter firstLetter, Letter secondLetter)
        {
            Vector2Int firstLetterGridPos = firstLetter.GridPosition;
            Vector2Int secondLetterGridPos = secondLetter.GridPosition;

            if (firstLetterGridPos.x != secondLetterGridPos.x &&
                firstLetterGridPos.y != secondLetterGridPos.y)
            {
                int height = Mathf.Abs(firstLetterGridPos.y - secondLetterGridPos.y);

                Vector2Int verticalLetter = new Vector2Int(firstLetterGridPos.x, secondLetterGridPos.y);
                Vector2Int horizontalLetter = new Vector2Int(secondLetterGridPos.x, firstLetterGridPos.y);
                Vector2Int diagonalLetter = new Vector2Int(firstLetterGridPos.x + height, firstLetterGridPos.y + height);

                float verticalDistance = Vector2Int.Distance(secondLetterGridPos, verticalLetter);
                float horizontalDistance = Vector2Int.Distance(secondLetterGridPos, horizontalLetter);
                float diagonalDistance = Vector2Int.Distance(secondLetterGridPos, diagonalLetter);

                float min = Mathf.Min(horizontalDistance, diagonalDistance, verticalDistance);

                if (min == verticalDistance) return firstLetter.Grid[verticalLetter.x][verticalLetter.y];
                if (min == horizontalDistance) return firstLetter.Grid[horizontalLetter.x][horizontalLetter.y];
                if (min == diagonalDistance) return firstLetter.Grid[diagonalLetter.x][diagonalLetter.y];
            }
            return secondLetter;
        }
    }
}
