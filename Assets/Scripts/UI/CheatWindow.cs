using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Goons.Debug;

namespace Goons.UI
{
    public class CheatWindow : BaseWindow
    {
        [Inject]
        private ICheatSystem _cheatSystem;

        [Inject]
        private IUiSystem _uiSystem;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (IsViwible)
                    Hide();
                else
                    Show();
            }
        }

        public void DeckBuilderButtonClicked()
        {
            _uiSystem.ShowDeckBuilder();
        }

        public void TakePlayerCard()
        {
            _cheatSystem.DrawCardByPlayer();
        }
        public void TakeOpponentCard()
        {
            _cheatSystem.DrawCardByOpponent();
        }
        public void PlaceCardByOpponent()
        {
            _cheatSystem.PlaceCardByOpponent();
        }
        public void MoveRandomPlayerCardToGraveyard()
        {
            _cheatSystem.MoveRandomPlayerCardToGraveyard();
        }
        public void MoveRandomEnemyCardToGraveyard()
        {
            _cheatSystem.MoveRandomOpponentCardToGraveyard();
        }

        public void FromGraveyardToPlayerHand()
        {
            _cheatSystem.MoveRandomCardFromGraveyardToPlayerHand();
        }
        public void FromGraveyardToOpponentHand()
        {
            _cheatSystem.MoveRandomCardFromGraveyardToOpponentHand();
        }

        public void FromGraveyardToPlayerDesk()
        {
            _cheatSystem.MoveRandomCardFromGraveyardToPlayerDesk();
        }
        public void FromGraveyardToOpponentDesk()
        {
            _cheatSystem.MoveRandomCardFromGraveyardToOpponentDesk();
        }
    }
}