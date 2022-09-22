using GusteruStudio.PuzzleStorm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GusteruStudio.Selection
{
    [CreateAssetMenu(menuName = "PuzzleStorm/WaterSortSelection")]
    public class WordsSelectionMode : SelectionMode
    {
        private Letter _firstSelectedLetter = null;

        public override void Set()
        {
            SelectionManager.SetMode(this);
        }

        public override void Enter()
        {
            TouchInput.onPieceSelected += ProcessSelection;
        }

        public override void Exit()
        {
            TouchInput.onPieceSelected -= ProcessSelection;
        }

        protected override void ProcessSelection(PuzzlePiece pp)
        {
           
        }
    }
}
