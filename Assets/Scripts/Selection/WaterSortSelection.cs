using GusteruStudio.PuzzleStorm;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GusteruStudio.Selection
{
    [CreateAssetMenu(menuName = "PuzzleStorm/WaterSortSelection")]
    public class WaterSortSelection : SelectionMode
    {
        private Bottle _firstSelected = null;
        public override void Set()
        {
            SelectionManager.SetMode(this);
        }

        public override void Enter()
        {
            TouchInput.onPieceSelected += ProcessSelection;
            CleanSelected();
        }

        public override void Exit()
        {
            TouchInput.onPieceSelected -= ProcessSelection;
        }

        protected override void ProcessSelection(PuzzlePiece pp)
        {
            Bottle bottle = pp as Bottle;

            if (_firstSelected == null)
            {
                _firstSelected = bottle;
                return;
            }

            if (_firstSelected == bottle)
            {
                _firstSelected = null;
                return;
            }

            if (_firstSelected != null)
            {
                int num_first_selected_colors = _firstSelected.colors.Length;
                //first color in the bottle is last color in the array
                Color firstColor = _firstSelected.colors[num_first_selected_colors - 1];
                bottle.Fill(firstColor);
                _firstSelected.Spill();
                CleanSelected();
            }
        }

        private void CleanSelected()
        {
            _firstSelected = null;
        }
    }
}