using System.Threading.Tasks;

namespace Goons.Network
{
    public interface IMatch
    {
        Task JoinRoom(RoomMessage room);
        Task LeaveRoom();
    }
}