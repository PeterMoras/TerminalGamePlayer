namespace GamePlayer
{
    public class C4PieceType : Enumeration
    {
        public static C4PieceType None = new(0, ' ');
        public static C4PieceType X = new(1, 'X');
        public static C4PieceType O = new(2, 'O');


        public C4PieceType(int id, char icon) : base(id, "" + icon) { }
    }
}