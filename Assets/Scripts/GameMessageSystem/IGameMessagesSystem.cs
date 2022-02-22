using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameMessagesSystem
{
    event Action<string> GameMessageReceived;

    void SendGameMessage(string message);
}

public class GameMessagesSystem : IGameMessagesSystem
{
    public event Action<string> GameMessageReceived;

    public void SendGameMessage(string message)
    {
        GameMessageReceived?.Invoke(message);
    }
}
