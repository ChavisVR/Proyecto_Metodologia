using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Text;
using System.Media;

namespace TetrisFinal
{
    class Program
    {

        // Definición de variables estáticas
        static int TetrisRows = 21;
        static int TetrisCols = 10;
        static int InfoCols = 20;
        static int ConsoleRows = 1 + TetrisRows + 1;
        static int ConsoleCols = 1 + TetrisCols + 1 + InfoCols + 1;
        static List<bool[,]> TetrisFigures = new List<bool[,]>()
        {
            // Definición de las formas de las piezas de Tetris
            new bool [,] { {true, true, true, true } },
            new bool [,] { {true, true  }, {true, true  } },
            new bool [,] { {false, true, false}, {true, true ,true } },
            new bool [,] { {false, true, true}, {true, true, false} },
            new bool[,] { {true, true, false}, {false, true, true} },
            new bool[,] { {false, false, true}, {true, true, true} },
            new bool[,] { {true, false, false}, {true, true, true} }
        };
        static string ScoresFileName = "score";
        static int[] ScorePerLines = { 0, 40, 100, 300, 1200 };
        static int HighScore = 0;
        static int Score = 0;
        static int Frame = 0;
        static int Level = 1;
        static string FigureSymbol = "■";
        static int FrameToMoveFigure = 16;
        static bool[,] CurrentFigure = null;
        static int CurrentFigureRow = 0;
        static int CurrentFigureCol = 0;
        static bool[,] NextFigure = null;
        static int NextFigureRow = 14;
        static int NextFigureCol = TetrisCols + 3;
        static bool[,] TetrisField = new bool[TetrisRows, TetrisCols];
        static Random Random = new Random();
        static bool PauseMode = false;
        static bool PlayGame = true;
        static int SongLevel = 2;

        // Método principal
        static void Main(string[] args)
        {
            // Configuraciones de la consola
            Console.OutputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.Unicode;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.CursorVisible = false;
            Console.WindowHeight = ConsoleRows;
            Console.WindowWidth = ConsoleCols;
            Console.BufferHeight = ConsoleRows;
            Console.BufferWidth = ConsoleCols;

            // Inicializar las figuras actual y próxima
            CurrentFigure = TetrisFigures[Random.Next(0, TetrisFigures.Count)];
            NextFigure = TetrisFigures[Random.Next(0, TetrisFigures.Count)];

            while (true)
            {
                Frame++;
                UpdateLevel();

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();

                    if (key.Key == ConsoleKey.Escape)
                    {
                        return;
                    }

                    // Manejar la entrada del teclado para movimiento y rotación
                    if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                    {
                        if (CurrentFigureCol >= 1)
                        {
                            CurrentFigureCol--;
                        }
                    }

                    if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                    {
                        if ((CurrentFigureCol < TetrisCols - CurrentFigure.GetLength(1)))
                        {
                            CurrentFigureCol++;
                        }
                    }

                    if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                    {
                        RotarFiguraActual();
                    }

                    if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                    {
                        Frame = 1;
                        Score += Level;
                        CurrentFigureRow++;
                    }
                }

                // Mover la figura actual hacia abajo a intervalos regulares
                if (Frame % (FrameToMoveFigure - Level) == 0)
                {
                    CurrentFigureRow++;
                    Frame = 0;
                    Score++;
                }

                // Verificar colisiones y actualizar
                if (Colision(CurrentFigure))
                {
                    AgregarFiguraActual();
                    int lineas = ComprobarLIneasCompletas();

                    Score += ScorePerLines[lineas] * Level;

                    CurrentFigureCol = 0;
                    CurrentFigureRow = 0;

                    // Fin del juego
                    if (Colision(CurrentFigure))
                    {
                        File.AppendAllLines(ScoresFileName, new List<string>
                        {
                            $"[{DateTime.Now.ToString()}] {Environment.UserName} => {Score}"
                        });
                        var scoreAsString = Score.ToString();
                        scoreAsString += new string(' ', 7 - scoreAsString.Length);
                        Write("╔══════════════╗", 5, 5);
                        Write("║ Juego        ║", 6, 5);
                        Write("║    terminado!║", 7, 5);
                        Write("║              ║", 8, 5);
                        Write("╚══════════════╝", 9, 5);
                        Console.ReadKey();
                        PlayGame = false;

                        return;
                    }
                }

                DibujarBorde();
                DibInfo();
                DibujarTetrisCampo();
                DibujarFiguraActual();

                Thread.Sleep(40);
            }
        }

        // Obtener la siguiente figura aleatoria
        private static bool[,] ObtenerSiguienteFigura()
        {
            NextFigure = TetrisFigures[Random.Next(0, TetrisFigures.Count)];
            return NextFigure;
        }

        // Actualizar el nivel según la puntuación alcanzada
        private static void UpdateLevel()
        {
            if (Score <= 0)
            {
                Level = 1;
            }

            if (Score >= 100)
            {
                Level = 2;
            }
            else if (Score == 250)
            {
                Level = 3;
            }
            else if (Score == 400)
            {
                Level = 4;
            }
            else if (Score == 550)
            {
                Level = 5;
            }
            else if (Score == 700)
            {
                Level = 6;
            }
            else if (Score == 850)
            {
                Level = 9;
            }
            else if (Score == 1000)
            {
                Level = 8;
            }
            else if (Score == 1150)
            {
                Level = 9;
            }
            else if (Score == 1300)
            {
                Level = 10;
            }
        }


    }
}