using UnityEngine;
namespace Goons.View
{
    public class CardSlot : MonoBehaviour
    {
        public bool IsBusy => transform.childCount > 0;

        public CardView Content;

        public Vector3 position => transform.position;
    }
}