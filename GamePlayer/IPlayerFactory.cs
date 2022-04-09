namespace GamePlayer
{
    public interface IPlayerFactory
    {
        IPlayer CreatePlayer(PlayerType type, C4PieceType pieceType);
    }

    public enum PlayerType
    {
        Human,
        EasyAI,
        MediumAI,
        HardAI
    }
}