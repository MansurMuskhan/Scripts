using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Goons.Network
{
    public class MatchMaking:IMatchMaking
    {


        public async Task<RoomMessage> FindRoom()
        {
            var url = "https://dev-api.getagoon.com/api/v1/find_room";

            var request = WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("Authorization:Token 8d67f6afb6c66bc600bda215deee82793b2c1b9a");

            using var response = await request.GetResponseAsync();
            using var responseStream = response.GetResponseStream();

            using var streamReader = new StreamReader(responseStream);
            var responseText = await streamReader.ReadToEndAsync();

            var roomMessage = JsonUtility.FromJson<RoomMessage>(responseText);
            return roomMessage;
        }
    }
}