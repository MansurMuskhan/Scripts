using System.Threading.Tasks;

namespace Goons.Network
{
    public interface IMatchMaking
    {
        Task<RoomMessage> FindRoom();
    }
}