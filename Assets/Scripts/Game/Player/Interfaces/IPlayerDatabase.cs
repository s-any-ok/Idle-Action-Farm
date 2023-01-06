#nullable enable
namespace Game.Player.Interfaces
{
    public interface IPlayerDatabase
    {
        float MoveSpeed { get; }
        float TurnSpeed { get; }
        float SellTime { get; }
        float CoinTime { get; }
        int WheatValue { get; }
        int MaxStacked { get; }
    }
}