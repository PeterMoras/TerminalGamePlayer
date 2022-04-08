namespace GamePlayer
{
    public interface IPlayerFactory
    {
        IPlayer CreatePlayer(PlayerType type);
    }

    public enum PlayerType
    {
        Human,
        EasyAI,
        MediumAI,
        HardAI
    }
}