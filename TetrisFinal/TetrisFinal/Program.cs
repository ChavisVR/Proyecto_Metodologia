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

            if (Score >= 100 )
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