using System;
using System.Collections.Generic;

public class BoatMovements
{
   private class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }

    public static bool CanTravelTo(bool[,] gameMatrix, int fromRow, int fromColumn, int toRow, int toColumn)
    {
        var rows = gameMatrix.GetLength(0);
        var columns = gameMatrix.GetLength(1);

        if (fromRow < 0 || fromRow >= rows || fromColumn < 0 || fromColumn >= columns ||
            toRow < 0 || toRow >= rows || toColumn < 0 || toColumn >= columns)
        {
            return false;
        }

        if (!gameMatrix[fromRow, fromColumn] || !gameMatrix[toRow, toColumn])
        {
            return false;
        }

        if (fromRow == toRow && fromColumn == toColumn)
        {
            return true;
        }

        var queue = new Queue<Position>(); 
        var visited = new bool[rows, columns];

        queue.Enqueue(new Position(fromRow, fromColumn));
        visited[fromRow, fromColumn] = true;

        int[] rowMoves = { 0, 0, 1, -1 };
        int[] colMoves = { 1, -1, 0, 0 };

        
        while (queue.Count > 0) 
        {
            var current = queue.Dequeue();

            for (var i = 0; i < 4; i++)
            {
                var nextRow = current.Row + rowMoves[i];
                var nextColumn = current.Column + colMoves[i];

                var isInBounds = nextRow >= 0 && nextRow < rows && 
                                 nextColumn >= 0 && nextColumn < columns;

                if (!isInBounds) continue;
                if (nextRow == toRow && nextColumn == toColumn)
                {
                    return true;
                }

                if (!visited[nextRow, nextColumn] && gameMatrix[nextRow, nextColumn])
                {
                    visited[nextRow, nextColumn] = true;
                    queue.Enqueue(new Position(nextRow, nextColumn));
                }
            }
        }
        return false;
    }

    public static void Main()
    {
        bool[,] gameMatrix = 
        {
            {false, true,  true,  false, false, false},
            {true,  true,  true,  false, false, false},
            {true,  true,  true,  true,  true,  true},
            {false, true,  true,  false, true,  true},
            {false, true,  true,  true,  false, true},
            {false, false, false, false, false, false},
        };

        Console.WriteLine(CanTravelTo(gameMatrix, 3, 2, 2, 2)); 
        Console.WriteLine(CanTravelTo(gameMatrix, 3, 2, 3, 4)); 
        Console.WriteLine(CanTravelTo(gameMatrix, 3, 2, 6, 2)); 
    }
}