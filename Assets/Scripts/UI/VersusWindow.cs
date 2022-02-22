using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Goons.Model;

public class VersusWindow : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _playerName;
    [SerializeField]
    private TMP_Text _opponentName;

    public void Init(PlayerInfo player, PlayerInfo opponent)
    {
        _playerName.text = player.Name;
        _opponentName.text = opponent.Name;
    }
}
