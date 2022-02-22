using System.Collections.Generic;
using NativeWebSocket;
using System.Threading.Tasks;
using UnityEngine;
using System;
using Goons.Model;

namespace Goons.Network
{
    public class NetworkMatch : IMatch
    {
        [Serializable]
        public class Message
        {
            public string type;
            public string message;
        }
        private WebSocket _websocket;

        private IGameModel _gameModel;

        public NetworkMatch(IGameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public Task LeaveRoom()
        {
            return _websocket.Close();
        }
        public async Task JoinRoom(RoomMessage room)
        {
            var url = $"ws://ws-test.getagoon.com/ws/{room.room_id}/{room.ticket}/";
            Debug.Log($"trying to connect room {url} ");
            var headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Token 7c015dfc3f46713491de5986ba618cdadde93d09");

            _websocket = new WebSocket(url);

            _websocket.OnOpen += () =>
            {
                Debug.Log("Socket opened");

            };

            _websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };

            _websocket.OnClose += (e) =>
            {
                Debug.Log("Connection closed!");
            };

            _websocket.OnMessage += (bytes) =>
            {
                Debug.Log("OnMessage!");
                Debug.Log(bytes);

                // getting the message as a string
                var message = System.Text.Encoding.UTF8.GetString(bytes);
                var messageObj = JsonUtility.FromJson<Message>(message);

                switch (messageObj.type)
                {
                    case "GameState":
                        DeserializeGameState(messageObj.message);
                        break;
                }
            };

            // Keep sending messages at every 0.3s
            //InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

            // waiting for messages
            await _websocket.Connect();
        }

        private void DeserializeGameState(string gameStateJson)
        {
            var gameState = JsonUtility.FromJson<GameState>(gameStateJson);
            _gameModel.GameState.UpdateGameState(gameState);
        }
    }
}