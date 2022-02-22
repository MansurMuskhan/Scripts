using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
namespace Goons.UI
{
    public class PlayersCardsPanel : CardsUiPanel
    {
        protected override void ProcessDrop(CardUI card)
        {
            DeckWindow.RemoveCard(card);
        }
        protected override void ProcessPointerEnter(CardUI card)
        {
            card.Maximize();
        }
    }
}