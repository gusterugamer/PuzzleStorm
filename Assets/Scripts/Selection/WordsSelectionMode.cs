using GusteruStudio.Extensions;
using GusteruStudio.PuzzleStorm;
using GusteruStudio.ReactiveVariables;
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
        [BoxGroup("Event")][SerializeField] private LetterSelectionEvent _onLettersSelected;
        private Letter _firstLetter = null;
        private Letter _secondLetter = null;

        public override void Set()
        {
            SelectionManager.SetMode(this);
        }

        public override void Enter()
        {
            TouchInput.onPieceSelected += MakeSelection;
            TouchInput.onPointerDown += ClearLetter;
            TouchInput.onPointerUp += ProcessSelection;
            ClearLetter();
        }

        public override void Exit()
        {
            TouchInput.onPieceSelected -= MakeSelection;
            TouchInput.onPointerDown -= ClearLetter;
            TouchInput.onPointerUp -= ProcessSelection;
        }

        protected override void MakeSelection(PuzzlePiece pp)
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
                _secondLetter = ValidateSecondLetter(_firstLetter, letter);
                Debug.Log("SELECTED" + "<color=" + Color.green.ToRGBHex() + ">" + " END " + "</color>" + "LETTER << " + _secondLetter.name + " >>", _secondLetter.gameObject);

                Debug.DrawLine(_firstLetter.transform.position, _secondLetter.transform.position, Color.black, 0.5f);
            }
        }

        private void ClearLetter(PointerEventData eventData = null)
        {
            _firstLetter = null;
            _secondLetter = null;

            Debug.Log("LETTER SELECTION IS CLEAR");
        }

        private void ProcessSelection(PointerEventData eventData = null)
        {
            if (_firstLetter == null || _secondLetter == null) return; //happens when player doesn't drag the pointer and lifts the finger

            LetterSelection letterSelection = new LetterSelection();
            letterSelection.firstLetter = _firstLetter;
            letterSelection.secondLetter = _secondLetter;

            _onLettersSelected.Invoke(letterSelection);
        }

        private Letter ValidateSecondLetter(Letter firstLetter, Letter secondLetter)
        {
            Vector2Int firstLetterGridPos = firstLetter.GridPosition;
            Vector2Int secondLetterGridPos = secondLetter.GridPosition;

            List<List<Letter>> grid = firstLetter.Grid;

            if (firstLetterGridPos.x != secondLetterGridPos.x &&
                firstLetterGridPos.y != secondLetterGridPos.y)
            {
                /*To find the closest letter on the diagonal from first letter with respect to 
                the player's pointer at a moment in time we calculate horizontal and
                vertical distances between the first letter and the current letter
                the pointer is hovering and take the minmum of that so we don't go
                out of bounds if the grid is NOT square.
                Then we add the offset to first letter position in grid to move on diagonal
                and get to a valid letter*/

                int height = Mathf.Abs(firstLetterGridPos.y - secondLetterGridPos.y);
                int width = Mathf.Abs(firstLetterGridPos.x - secondLetterGridPos.x);

                //Cache the sign in case the selection on diagonal is in reverse
                //The axis doesn't matter since is diagonal
                int sign = firstLetterGridPos.y - secondLetterGridPos.y > 0 ? -1 : 1;

                int offSetToSecondLetter = Mathf.Min(height, width) * sign;

                Vector2Int diagonalLetter = new Vector2Int(firstLetterGridPos.x + offSetToSecondLetter, firstLetterGridPos.y + offSetToSecondLetter);
                Vector2Int verticalLetter = new Vector2Int(firstLetterGridPos.x, secondLetterGridPos.y);
                Vector2Int horizontalLetter = new Vector2Int(secondLetterGridPos.x, firstLetterGridPos.y);

                float verticalDistance = Vector2Int.Distance(secondLetterGridPos, verticalLetter);
                float horizontalDistance = Vector2Int.Distance(secondLetterGridPos, horizontalLetter);
                float diagonalDistance = Vector2Int.Distance(secondLetterGridPos, diagonalLetter);

                float min = Mathf.Min(horizontalDistance, diagonalDistance, verticalDistance);

                if (min == verticalDistance) return grid[verticalLetter.x][verticalLetter.y];
                if (min == horizontalDistance) return grid[horizontalLetter.x][horizontalLetter.y];
                if (min == diagonalDistance) return grid[diagonalLetter.x][diagonalLetter.y];
            }

            return secondLetter;
        }
    }

    public struct LetterSelection
    {
        public Letter firstLetter;
        public Letter secondLetter;
    }
}
