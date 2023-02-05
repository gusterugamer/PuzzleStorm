using GusteruStudio.PuzzleStorm;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
            ClearSelection();
        }

        public override void Exit()
        {
            TouchInput.onPieceSelected -= ProcessSelection;
        }

        protected override void ProcessSelection(PuzzlePiece pp)
        {
            Bottle bottle = pp as Bottle;

            Assert.IsNotNull(bottle, "THE SELECTED PIECES IS NOT A <<BOTTLE>> !");

            if (bottle.colors.Length == 0)
            {
                Debug.Log("BOTTLE: " + bottle.name + "IS EMPTY AND CANNOT BE SELECTED!", bottle.gameObject);
                return;
            }

            //Records the first selected bottle
            if (_firstSelected == null)
            {
                Debug.Log("Bottle selected: " + bottle.name, bottle.gameObject);
                _firstSelected = bottle;
                return;
            }

            //Deselects the selected bottle if it's clicked a second time
            if (_firstSelected == bottle)
            {
                Debug.Log("Bottle DEselected: " + bottle.name, bottle.gameObject);
                ClearSelection();
                return;
            }

            //If there is a selected bottle recorded and another bottle selected try to fill the second bottle
            if (_firstSelected != null)
            {
                _firstSelected.AttemptToFill(bottle);
                ClearSelection();
            }
        }

        private void ClearSelection()
        {
            _firstSelected = null;
        }
    }
}