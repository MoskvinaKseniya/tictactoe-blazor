namespace tictactoe_blazor.Components.GameLogic
{
    public class TicTacToe
    {
        public int N { get; }  // размер поля
        public int M { get; }  // количество символов подряд для победы
        public char[,] Board { get; }
        public bool IsPlayerX { get; private set; } = true;
        public bool GameEnded { get; private set; } = false;
        public string? Winner { get; private set; }  // кто победитель

        public TicTacToe(int n, int m)
        {
            N = n;
            M = m;
            Board = new char[n, n];
        }

        public bool MakeMove(int cellIndex)  // ход игрока по индексу клетки
        {
            if (GameEnded) return false;

            int row = cellIndex / N;
            int col = cellIndex % N;

            if (Board[row, col] != '\0') return false;

            Board[row, col] = IsPlayerX ? 'X' : 'O';

            if (IsWinningMove(row, col))
            {
                GameEnded = true;
                Winner = IsPlayerX ? "победил грок X!" : "победил игрок O!";
            }
            else if (IsBoardFull())
            {
                GameEnded = true;
                Winner = "ничья";
            }
            else
            {
                IsPlayerX = !IsPlayerX;
            }

            return true;
        }

        private bool IsWinningMove(int row, int col)  // проверка привёл ли последний ход к победе
        {
            char symbol = Board[row, col];
            if (symbol == '\0') return false;

            (int dx, int dy)[] dirs =
            {
                (0, 1),   // горизонталь
                (1, 0),   // вертикаль
                (1, 1),   // диагональ
                (1, -1)   // диагональ
            };

            foreach (var (dx, dy) in dirs)
            {
                int count = 1;
                count += Count(row, col, dx, dy, symbol);
                count += Count(row, col, -dx, -dy, symbol);
                if (count >= M) return true;
            }
            return false;
        }

        private int Count(int row, int col, int dx, int dy, char symbol)  // количество подряд идущих символов в указанном направлении
        {
            int n = 0;
            int r = row + dx, c = col + dy;

            while (r >= 0 && r < N && c >= 0 && c < N && Board[r, c] == symbol)
            {
                n++;
                r += dx;
                c += dy;
            }
            return n;
        }

        private bool IsBoardFull()  // заполнено ли поле полностью
        {
            for (int r = 0; r < N; r++)
                for (int c = 0; c < N; c++)
                    if (Board[r, c] == '\0') return false;
            return true;
        }

        public void EndGameEarly()  // завершить игру досрочно
        {
            if (!GameEnded)
            {
                GameEnded = true;
                Winner = "игра завершена досрочно";
            }
        }
    }
}

