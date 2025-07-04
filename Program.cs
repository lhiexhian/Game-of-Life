using System;
using System.Threading;

public class GameOFLife
{
    private readonly int _width;
    private readonly int _height;
    private bool[,] _currentGen;
    private bool[,] _nextGen;

    public GameOFLife(int height, int width)
    {
        _height = height;
        _width = width;
        _currentGen = new bool[height, width];
        _nextGen = new bool[height, width];
    }
    public void Generate()
    {
        Random random = new Random();
        for (int i = 0; i < _height-1; i++)
        {
            for (int j = 0; j < _width-1; j++)
            {
                _currentGen[i, j] = random.Next(0, 10) < 1;
            }
        }
    }

    public void Refresh()
    {
        Console.SetCursorPosition(0, 2);
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                if (j < 2)
                {
                    Console.Write(" ");
                    continue;
                }
                else
                {
                    Console.Write(_currentGen[i, j] ? "#" : " ");
                }
            }
            Console.WriteLine();
        }
    }

    public void NewGen()
    {
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                int liveNeighbors = CountLiveNeighbors(i, j);
                bool currentCell = _currentGen[i, j];

                if (currentCell)
                {
                    _nextGen[i, j] = liveNeighbors == 2 || liveNeighbors == 3;
                }
                else
                {
                    _nextGen[i, j] = liveNeighbors == 3;
                }
            }
        }
        Array.Copy(_nextGen, _currentGen, _currentGen.Length);
    }

    public int CountLiveNeighbors(int x, int y)
    {
        int count = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue; // Skip the cell itself
                int newX = x + i;
                int newY = y + j;
                if (newX >= 0 && newX < _height && newY >= 0 && newY < _width)
                {
                    if (_currentGen[newX, newY]) count++;
                }
            }
        }
        return count;
    }
}
class Program
{
    

    static void Main(string[] args)
    {
        Console.WriteLine("The Game of Life");
        Console.CursorVisible = false;
        Console.SetWindowSize(Math.Min(60 + 2, 100), Math.Min(30 + 4, 100));
        Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        Console.CursorSize = 1;
        var game = new GameOFLife(30, 60);
        game.Generate();

        while (true)
        {
            game.Refresh();
            game.NewGen();
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Escape:
                    case ConsoleKey.Q:
                        return;

                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                        game.Generate();
                        break;
                }
            }
            Console.WriteLine("Press [Space] to reset, [ESC] to quit...");
            Thread.Sleep(10);
        }
    }
}
