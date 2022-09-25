using GusteruStudio.PuzzleStorm;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GusteruStudio.Selection
{
    [CreateAssetMenu(menuName = "PuzzleStorm/WordsSortSelection")]
    public class WordsSelectionMode : SelectionMode
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
            ClearLetter();
        }

        public override void Exit()
        {
            TouchInput.onPieceSelected -= ProcessSelection;
        }

        protected override void ProcessSelection(PuzzlePiece pp)
        {
            Letter letter = pp as Letter;

            if (_firstLetter == letter || _secondLetter == letter)
                return;

            if (_firstLetter == null)
            {
                _firstLetter = letter;
                return;
            }

            if (_firstLetter != letter && _secondLetter != letter)
            {
                _secondLetter = letter;
            }

            Debug.Log("Letter1: " + _firstLetter.name + "Letter2: " + _secondLetter.name);

        }

        private void BuildWord()
        {
            _wm.BuildWord(_firstLetter, _secondLetter);
        }

        private void ClearLetter()
        {
            _firstLetter = null;
            _secondLetter = null;
        }
    }
}
