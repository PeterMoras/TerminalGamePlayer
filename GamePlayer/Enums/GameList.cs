namespace GamePlayer
{
    public class GameList : Enumeration
    {
        public static GameList Connect4 = new(0, "Connect4");

        public GameList(int id, string name) : base(id, name)
        {
        }
    }
}