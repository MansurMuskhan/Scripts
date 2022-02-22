using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Goons.Network;
using Zenject;
public class ConnectionUI : MonoBehaviour
{
    [Inject]
    private IMatchMaking _matchMaking;
    [Inject]
    private IMatch _match;
     

    public async void ButtonConnectClicked()
    {
        var room = await _matchMaking.FindRoom();
        print($"found room {room.room_id} : {room.ticket}");

        await _match.JoinRoom(room);
        print("room joinded");
    }
    public void ButtonDisconnectClicked()
    {
        _match.LeaveRoom();
    }
}
