namespace Testeo
{
    [TestClass]
    public class UnitTest1
    {
        [TestClass]
        public class TetrisTests
        {
            [TestMethod]
            public void TestRotarFiguraActual()
            {
                // Arrange
                var initialFigure = new bool[,] { { true, true }, { true, false } };
                var expectedRotatedFigure = new bool[,] { { true, true }, { false, true } };

                // Act
                RotarFigura(ref initialFigure);

                // Assert
                CollectionAssert.AreEqual(expectedRotatedFigure, initialFigure);
            }

            [TestMethod]
            public void TestColision()
            {
                // Arrange
                var tetrisField = new bool[21, 10];
                var figure = new bool[,] { { true, true }, { true, false } };

                // Act
                var collisionResult = Colision(figure, 19, 0, tetrisField);

                // Assert
                Assert.IsTrue(collisionResult);
            }

            // Métodos de prueba auxiliares
            private void RotarFigura(ref bool[,] figura)
            {
                var nuevaFigura = new bool[figura.GetLength(1), figura.GetLength(0)];
                for (int fila = 0; fila < figura.GetLength(0); fila++)
                {
                    for (int col = 0; col < figura.GetLength(1); col++)
                    {
                        nuevaFigura[col, figura.GetLength(0) - fila - 1] = figura[fila, col];
                    }
                }
                figura = nuevaFigura;
            }

            private bool Colision(bool[,] figura, int fila, int col, bool[,] tetrisField)
            {
                if (col > tetrisField.GetLength(1) - figura.GetLength(1))
                {
                    return true;
                }

                if (fila + figura.GetLength(0) == tetrisField.GetLength(0))
                {
                    return true;
                }

                for (int i = 0; i < figura.GetLength(0); i++)
                {
                    for (int j = 0; j < figura.GetLength(1); j++)
                    {
                        if (figura[i, j] && tetrisField[fila + i + 1, col + j])
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

        }
    }
}