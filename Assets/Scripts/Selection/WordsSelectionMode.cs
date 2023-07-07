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
            TouchInput.onPointerUp += ClearLetter;
            ClearLetter(null);
        }

        public override void Exit()
        {
            TouchInput.onPieceSelected -= ProcessSelection;
            TouchInput.onPointerUp -= ClearLetter;
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
                _secondLetter = letter;
                Debug.Log("SELECTED" + "<color=" + Color.green.ToRGBHex() + ">" + " END " + "</color>" + "LETTER << " + _secondLetter.name + " >>", _secondLetter.gameObject);
            }

            Debug.Log("Letter1: " + _firstLetter.name + "Letter2: " + _secondLetter.name);

        }

        private void BuildWord()
        {
            _wm.BuildWord(_firstLetter, _secondLetter);
        }

        private void ClearLetter(PointerEventData eventData)
        {
            _firstLetter = null;
            _secondLetter = null;

            Debug.Log("LETTER SELECTION IS CLEAR");
        }
    }
}
