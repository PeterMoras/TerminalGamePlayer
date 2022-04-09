namespace GamePlayer.Games.Connect4
{
    public interface IC4Model
    {
        C4PieceType[,] Data { get; }

        public long Rows { get; }
        public long Cols { get; }

        event Action<C4ModelEventArgs>? OnDataChange;
        event Action<C4WinArgs>? OnWinningPiecePlaced;

        bool TryAddPiece(int col, C4PieceType piece, IC4Player player);
        void Clear();
    }

    public struct Point
    {
        public int X;
        public int Y;
        public Point(int x, int y) { X = x; Y = y; }
    }



    public class C4Model : IC4Model
    {

        public static C4PieceType[,] DefaultBoard => FromEmpty(6, 7);
        private C4PieceType[,] _data = DefaultBoard;

        public C4PieceType[,] Data => _data;

        public long Rows => Data.GetLongLength(1);
        public long Cols => Data.GetLongLength(0);

        public event Action<C4ModelEventArgs>? OnDataChange;
        public event Action<C4WinArgs>? OnWinningPiecePlaced;

        public C4Model()
        {
            OnDataChange += CheckForWin;
        }

        private void CheckForWin(C4ModelEventArgs e)
        {
            var point = e.AddedPieceLoc;
            List<Point> pieces;

            //check various win conditions
            //player null means the board placed the piece (probably clearing the board or something)
            if (e.Player != null && (pieces = GetHorizontalLine(point, e.PieceType)).Count >= 4
                || (pieces = GetVerticalLine(point, e.PieceType)).Count >= 4
                || (pieces = GetFSlashLine(point, e.PieceType)).Count >= 4
                || (pieces = GetBSlashLine(point, e.PieceType)).Count >= 4)
            {
                OnWinningPiecePlaced?.Invoke(new C4WinArgs(e.PieceType, point, e.Player!));
            }


        }

        private List<Point> GetHorizontalLine(Point point, C4PieceType pieceType)
            => lineCheck(point, pieceType, (p) => new Point(p.X + 1, p.Y), (p) => new Point(p.X - 1, p.Y));

        private List<Point> GetVerticalLine(Point point, C4PieceType pieceType)
            => lineCheck(point, pieceType, (p) => new Point(p.X, p.Y + 1), (p) => new Point(p.X, p.Y - 1));
        private List<Point> GetFSlashLine(Point point, C4PieceType pieceType)
            => lineCheck(point, pieceType, (p) => new Point(p.X + 1, p.Y + 1), (p) => new Point(p.X - 1, p.Y - 1));
        private List<Point> GetBSlashLine(Point point, C4PieceType pieceType)
            => lineCheck(point, pieceType, (p) => new Point(p.X - 1, p.Y + 1), (p) => new Point(p.X + 1, p.Y - 1));
        List<Point> lineCheck(Point point, C4PieceType pieceType, Func<Point, Point> nextPoint, Func<Point, Point> previousPoint)
        {
            List<Point> points = new();
            //increasing
            var modPoint = point;
            while (IsSameType(modPoint, pieceType))
            {
                points.Add(modPoint);
                modPoint = nextPoint(modPoint);
            }
            //dont check original point a second time
            modPoint = previousPoint(point);
            while (IsSameType(modPoint, pieceType))
            {
                points.Add(modPoint);
                modPoint = previousPoint(modPoint);
            }
            return points;

        }
        bool IsSameType(Point point, C4PieceType type)
        {
            //out of range?
            if (point.X >= Cols || point.X < 0 || point.Y >= Rows || point.Y < 0) return false;
            return _data[point.X, point.Y] == type;
        }

        public void Clear()
        {
            _data = DefaultBoard;
            OnDataChange?.Invoke(new C4ModelEventArgs(_data, new Point(-1, -1), C4PieceType.None, null));
        }

        public bool TryAddPiece(int col, C4PieceType piece, IC4Player player)
        {
            for (int r = 0; r < Rows; r++)
            {
                if (_data[col, r] == C4PieceType.None)
                {
                    _data[col, r] = piece;
                    OnDataChange?.Invoke(new C4ModelEventArgs(_data, new Point(col, r), piece, player));
                    return true;
                }
            }
            return false;
        }



        public static C4PieceType[,] FromEmpty(int cols, int rows)
        {
            C4PieceType[,] data = new C4PieceType[cols, rows];
            var length = data.Length;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    data[c, r] = C4PieceType.None;
                }
            }
            return data;
        }

    }
    public record C4ModelEventArgs(C4PieceType[,] Data, Point AddedPieceLoc, C4PieceType PieceType, IC4Player? Player);

    public record C4WinArgs(C4PieceType PieceType, Point WinningLoc, IC4Player WinningPlayer);
}