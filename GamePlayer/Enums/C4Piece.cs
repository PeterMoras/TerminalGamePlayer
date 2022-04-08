namespace GamePlayer
{
    public class C4Piece : Enumeration
    {
        public static C4Piece None = new(0, ' ');
        public static C4Piece X = new(1, 'X');
        public static C4Piece O = new(2, 'O');


        public C4Piece(int id, char icon) : base(id, "" + icon) { }
    }
}