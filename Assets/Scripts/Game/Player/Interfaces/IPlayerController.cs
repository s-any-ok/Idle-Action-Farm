#nullable enable
using Game.Player.Views;

namespace Game.Player.Interfaces
{
    public interface IPlayerController
    {
        PlayerView View { get; }
    }
}