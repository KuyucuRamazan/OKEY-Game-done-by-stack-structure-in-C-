using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Homework
{
    internal class Program
    {
        static Random random = new Random();

        public static void Main(string[] args)
        {
            Tile[] tiles = new Tile[106];
            for (int i = 0; i < tiles.Length - 2; i++)
            {
                byte number = (byte)(i % 13 + 1);
                TileColor color = (TileColor)(i / 13 % 4);
                tiles[i] = new Tile(color, number);
            }

            // Last two tiles are fake jokers
            tiles[104] = new Tile(TileColor.Black, Tile.FAKE_OKEY);
            tiles[105] = new Tile(TileColor.Black, Tile.FAKE_OKEY);

            // Shuffle the tiles two times
            Shuffle(tiles);
            Shuffle(tiles);

            Player[] players = {
                new Player(new FixedLengthStack<Tile>(27)),
                new Player(new FixedLengthStack<Tile>(27)),
                new Player(new FixedLengthStack<Tile>(26)),
                new Player(new FixedLengthStack<Tile>(26))
            };

            // Distribute tiles
            for (int i = 0; i < tiles.Length; i++)
            {
                if (i < 20)
                {
                    // beşer beşer herkese
                    players[i / 5].Tiles.Push(tiles[i]);
                }
                else if (i < 36)
                {
                    // dörder dörder herkese
                    players[(i - 20) / 4].Tiles.Push(tiles[i]);
                }
                else
                {
                    // birer birer herkese ve son iki taş ilk iki insana
                    players[i % 4].Tiles.Push(tiles[i]);
                }
            }

            // Show tiles of players
            foreach (Player player in players)
            {
                foreach (Tile tile in player.Tiles)
                {
                    Console.WriteLine("Numara: {0}, Renk = {1}", tile.Number, tile.Color);
                }

                Console.WriteLine();
            }

            // Calculate scores of players
            foreach (Player player in players) // tiles = 0, 1, 2, 3, 4
            {
                Tile prev = player.Tiles.Pop(); // 0
                Tile current = prev; // 0
                Tile next = player.Tiles.Pop(); // 1

                for (int i = 0; i < player.Tiles.Count - 2; i++)
                {
                    prev = current; // 0, 1, 2
                    current = next; // 1, 2, 3
                    next = player.Tiles.Pop(); // 2, 3, 4

                    if (prev.Number == Tile.FAKE_OKEY)
                    {
                        if (current.Color == next.Color && Math.Abs(next.Number - current.Number) == 1)
                        {
                            player.Score++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("prev = {0}, current = {1}, next = {2}",
                                prev.Number + " " + prev.Color, current.Number + " " + current.Color, next.Number + " " + next.Color);
                            Console.ResetColor();
                        }
                        else if (current.Color != next.Color && current.Number == next.Number)
                        {
                            player.Score++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("prev = {0}, current = {1}, next = {2}",
                                prev.Number + " " + prev.Color, current.Number + " " + current.Color, next.Number + " " + next.Color);
                            Console.ResetColor();
                        }
                        else if (current.Number == Tile.FAKE_OKEY || next.Number == Tile.FAKE_OKEY)
                        {
                            player.Score++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("prev = {0}, current = {1}, next = {2}",
                                prev.Number + " " + prev.Color, current.Number + " " + current.Color, next.Number + " " + next.Color);
                            Console.ResetColor();
                        }
                    }
                    else if (current.Number == Tile.FAKE_OKEY)
                    {
                        if (prev.Color == next.Color && Math.Abs(next.Number - prev.Number) == 2)
                        {
                            player.Score++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("prev = {0}, current = {1}, next = {2}",
                                prev.Number + " " + prev.Color, current.Number + " " + current.Color, next.Number + " " + next.Color);
                            Console.ResetColor();
                        }
                        else if (prev.Color != next.Color && prev.Number == next.Number)
                        {
                            player.Score++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("prev = {0}, current = {1}, next = {2}",
                                prev.Number + " " + prev.Color, current.Number + " " + current.Color, next.Number + " " + next.Color);
                            Console.ResetColor();
                        }
                        else if (prev.Number == Tile.FAKE_OKEY || next.Number == Tile.FAKE_OKEY)
                        {
                            player.Score++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("prev = {0}, current = {1}, next = {2}",
                                prev.Number + " " + prev.Color, current.Number + " " + current.Color, next.Number + " " + next.Color);
                            Console.ResetColor();
                        }
                    }
                    else if (next.Number == Tile.FAKE_OKEY)
                    {
                        if (prev.Color == current.Color && Math.Abs(current.Number - prev.Number) == 1)
                        {
                            player.Score++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("prev = {0}, current = {1}, next = {2}",
                                prev.Number + " " + prev.Color, current.Number + " " + current.Color, next.Number + " " + next.Color);
                            Console.ResetColor();
                        }
                        else if (prev.Color != current.Color && prev.Number == current.Number)
                        {
                            player.Score++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("prev = {0}, current = {1}, next = {2}",
                                prev.Number + " " + prev.Color, current.Number + " " + current.Color, next.Number + " " + next.Color);
                            Console.ResetColor();
                        }
                        else if (prev.Number == Tile.FAKE_OKEY || current.Number == Tile.FAKE_OKEY)
                        {
                            player.Score++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("prev = {0}, current = {1}, next = {2}",
                                prev.Number + " " + prev.Color, current.Number + " " + current.Color, next.Number + " " + next.Color);
                            Console.ResetColor();
                        }
                    }
                    else if (current.Color == prev.Color && current.Color == next.Color)
                    {
                        // Same color
                        if ((prev.Number == current.Number - 1 && current.Number == next.Number - 1)
                            || (prev.Number == current.Number + 1 && current.Number == next.Number + 1))
                        {
                            // Consecutive numbers
                            player.Score++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("prev = {0}, current = {1}, next = {2}",
                                prev.Number + " " + prev.Color, current.Number + " " + current.Color, next.Number + " " + next.Color);
                            Console.ResetColor();
                        }
                        else if (current.Number == 13 && ((prev.Number == 12 && next.Number == 1)
                            || (prev.Number == 1 && next.Number == 12)))
                        {
                            // 12-13-1      1-13-12
                            player.Score++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("prev = {0}, current = {1}, next = {2}",
                                prev.Number + " " + prev.Color, current.Number + " " + current.Color, next.Number + " " + next.Color);
                            Console.ResetColor();
                        }
                    }
                    else if (current.Color != prev.Color && current.Color != next.Color && prev.Color != next.Color)
                    {
                        // Different colors
                        if (prev.Number == current.Number && current.Number == next.Number)
                        {
                            // Same numbers
                            player.Score++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("prev = {0}, current = {1}, next = {2}",
                                prev.Number + " " + prev.Color, current.Number + " " + current.Color, next.Number + " " + next.Color);
                            Console.ResetColor();
                        }
                    }
                }
            }

            int min = int.MaxValue;
            int max = int.MinValue;

            for (int i = 0; i < players.Length; i++)
            {
                int score = players[i].Score;

                if (score > max)
                {
                    max = score;
                }
                if (score < min)
                {
                    min = score;
                }

                Console.WriteLine("Player{0} score: {1}", i+1, score);
            }

            Console.WriteLine("\nMaximum Score = {0}\nMinimum Score = {1}", max, min);
            Console.WriteLine();
        }

        public static void Shuffle(object[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                int randIndex = random.Next(i, arr.Length);
                object tile = arr[i];

                // Exchange tiles
                arr[i] = arr[randIndex];
                arr[randIndex] = tile;
                Thread.Sleep(100);
            }
        }
    }

    internal class Tile
    {
        internal const int FAKE_OKEY = 0;

        internal TileColor Color { get; private set; }
        internal byte Number { get; private set; }

        internal Tile(TileColor color, byte number)
        {
            if (number > 13)
                throw new ArgumentOutOfRangeException(nameof(number), "Number must be between 1 and 13");
            Color = color;
            Number = number;
        }
    }

    internal enum TileColor
    {
        Red,
        Yellow,
        Black,
        Blue
    }

    internal class FixedLengthStack<T> : IEnumerable<T>
    {
        T[] _datas;
        int _currentIndex;
        internal int Count { get; private set; }

        internal FixedLengthStack(int length)
        {
            _datas = new T[length];
            _currentIndex = length;
            Count = length;
        }

        internal void Push(T data)
        {
            if (_currentIndex == 0)
                throw new Exception("Yığın dolu");

            _datas[--_currentIndex] = data;
        }

        internal T Pop()
        {
            if (_currentIndex == _datas.Length)
                throw new Exception("Yığın boş");

            T data = _datas[_currentIndex];
            _datas[_currentIndex++] = default(T);

            return data;
        }

        internal T Peek()
        {
            if (_currentIndex == _datas.Length)
                throw new Exception("Yığın boş");

            return _datas[_currentIndex];
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _currentIndex; i < _datas.Length; i++)
            {
                yield return _datas[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class Player
    {
        internal FixedLengthStack<Tile> Tiles { get; private set; }
        internal int Score
        {
            get => _score;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Score negatif olamaz!");

                _score = value;
            }
        }
        int _score;

        internal Player(FixedLengthStack<Tile> tiles)
        {
            Tiles = tiles;
            Score = 0;
        }
    }
}