namespace GamePlayer.Games.Connect4
{
    public interface IC4View
    {
        public void StartShowing();
        void StopShowing();
    }





    public class C4View : IC4View
    {
        private IC4Model _model;
        bool _visible;

        public C4View(IC4Model model)
        {
            _model = model;
            model.OnDataChange += OnDataChange;
        }

        private void OnDataChange(C4ModelEventArgs e)
        {
            if (_visible)
                showView(e.Data);
        }

        public void StartShowing()
        {
            _visible = true;
            showView(_model.Data);
        }
        public void StopShowing()
        {
            _visible = false;
        }

        // 7 columns, 6 rows
        void showView(C4Piece[,] pieces)
        {
            drawLines(pieces);
            // var p = pieces;
            // Console.WriteLine($"  1 2 3 4 5 6");
            // Console.WriteLine($"7 ${p[0, 6]}|${p[1, 6]}|${p[2, 6]}|${p[3, 6]}|${p[4, 6]}|${p[5, 6]}");
            // //Console.WriteLine("-----------");
            // Console.WriteLine($"7 ${p[0, 6]}|${p[1, 6]}|${p[2, 6]}|${p[3, 6]}|${p[4, 6]}|${p[5, 6]}");
            // // Console.WriteLine("-----------");
            // Console.WriteLine($"5  | | | | | ");
            // // Console.WriteLine("-----------");
            // Console.WriteLine($"4  | | | | | ");
            // // Console.WriteLine("-----------");
            // Console.WriteLine($"3  | | | | | ");
            // // Console.WriteLine("-----------");
            // Console.WriteLine($"2  | | | | | ");
            // // Console.WriteLine("-----------");
            // Console.WriteLine($"1  | | | | | ");
            // Console.WriteLine($"  -----------");
        }
        void drawLines(C4Piece[,] pieces)
        {
            var p = pieces;
            var cols = pieces.GetLength(0);
            var rows = pieces.GetLength(1);

            for (int r = rows - 1; r >= 0; r--)
            {
                //Console.WriteLine($"7 ${p[0, r]}|${p[1, r]}|${p[2, r]}|${p[3, r]}|${p[4, r]}|${p[5, r]}");
                Console.Write($"{r + 1} ");
                for (int c = 0; c < cols; c++)
                {
                    if (c != 0)
                        Console.Write("|");
                    Console.Write($"{p[c, r]}");
                }
                Console.WriteLine();
            }
            Console.Write("  ");
            //write numbers underneath to know which column each is
            for (int c = 0; c < cols; c++)
            {
                if (c != 0)
                    Console.Write(" ");
                Console.Write($"{c}");
            }
            Console.WriteLine();

        }
    }
}