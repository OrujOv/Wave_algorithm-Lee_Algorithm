using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication_Maze
{

    /*
    Даны робот, задаваемый интерфейсом IRobot, и лабиринт, представляющий из себя набор клеток (Cell),
    каждая из которых является либо пустой, либо стеной, либо выходом.
    Размеры и форма лабиринта заранее неизвестны.

    Пример возможного лабиринта (# - стена, R - робот, E - выход):

    #######
    #     #
    # # E #
    ## #  #
    # R#  #
    #     #
    #######

    */

    public enum CellType
    {
        Empty,
        Wall,
        Exit
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public static class PathFinderFromMaze
    {
        public static void FindExit_()
        {
            //Input
            //Starting point of Robot
            int currY = 4;
            int currX = 2;
            //The maze
            int[,] maze = { {1,1,1,1,1,1,1}, //Лабиринт на основании вышепоказанного примера
                            {1,0,0,0,0,0,1},
                            {1,0,1,0,2,0,1},
                            {1,1,0,1,0,0,1},
                            {1,0,0,1,0,0,1},
                            {1,0,0,0,0,0,1},
                            {1,1,1,1,1,1,1}};


            // Finding exit and waves to it
            int W = maze.GetLength(1);
            int H = maze.GetLength(0);
            int? exitX = null;
            int? exitY = null;
            int d, x, y;
            bool marked;

            if (maze[currY, currX] != (int)CellType.Exit || maze[currY, currX] == (int)CellType.Wall)
            {

                d = 5;                              //Произвольная метка для выделения волн
                maze[currY, currX] = 5;
                do
                {
                    marked = true;
                    for (y = 0; y < H; ++y)
                        for (x = 0; x < W; ++x)
                            if (maze[y, x] == d)
                            {
                                for (int z = 0; z < 4; z++)
                                {
                                    int adjacentX = x;
                                    int adjacentY = y;
                                    switch (z)
                                    {
                                        case 0:
                                            adjacentX = x - 1;
                                            adjacentY = y;
                                            break;
                                        case 1:
                                            adjacentX = x + 1;
                                            adjacentY = y;
                                            break;
                                        case 2:
                                            adjacentX = x;
                                            adjacentY = y - 1;
                                            break;
                                        case 3:
                                            adjacentX = x;
                                            adjacentY = y + 1;
                                            break;
                                    }
                                    if (adjacentX >= 0 && adjacentX < H && adjacentY >= 0 && adjacentY < W &&
                                       (maze[adjacentY, adjacentX] == (int)CellType.Empty || maze[adjacentY, adjacentX] == (int)CellType.Exit))
                                    {
                                        marked = false;
                                        if (maze[adjacentY, adjacentX] == (int)CellType.Exit)
                                        {
                                            exitX = adjacentX;
                                            exitY = adjacentY;
                                        }
                                        maze[adjacentY, adjacentX] = d + 1;
                                    }
                                }
                            }
                    d++;
                } while (!marked);


                //======//
                //Finding way to exit
                int[] wayX = new int[W * H];
                int[] wayY = new int[H * W];
                int way;

                if (exitX == null && exitY == null) { Console.WriteLine("No way out"); }
                else
                {

                    way = maze[(int)exitY, (int)exitX];
                    x = (int)exitX;
                    y = (int)exitY;
                    d = way;
                    int arrN = way - 5;
                    while (d >= 5)
                    {
                        wayX[arrN] = x;
                        wayY[arrN] = y;
                        d--;
                        arrN--;
                        for (int k = 0; k < 4; ++k)
                        {
                            int adjacentX = x;
                            int adjacentY = y;
                            switch (k)
                            {
                                case 0:
                                    adjacentX = x - 1;
                                    adjacentY = y;
                                    break;
                                case 1:
                                    adjacentX = x + 1;
                                    adjacentY = y;
                                    break;
                                case 2:
                                    adjacentX = x;
                                    adjacentY = y - 1;
                                    break;
                                case 3:
                                    adjacentX = x;
                                    adjacentY = y + 1;
                                    break;
                            }
                            if (adjacentX >= 0 && adjacentX < H && adjacentY >= 0 && adjacentY < W &&
                                 maze[adjacentY, adjacentX] == d)
                            {
                                x = adjacentX;
                                y = adjacentY;
                                break;
                            }
                        }
                    }
                }

                for (int step = 0; step < wayX.Length; step++)
                {
                    int difY = wayY[step] - wayY[step + 1];
                    int difX = wayX[step] - wayX[step + 1];

                    if (difY == -1 && difX == 0) Console.WriteLine("Move " + Direction.Down+"(From "+ wayY[step] +":"+ wayX[step] + " to "+wayY[step+1] + ":" + wayX[step+1]+")");
                    else if (difY == 1 && difX == 0) Console.WriteLine("Move " + Direction.Up + "(From " + wayY[step] + ":" + wayX[step] + " to " + wayY[step + 1] + ":" + wayX[step + 1] + ")");
                    else if (difY == 0 && difX == -1) Console.WriteLine("Move " + Direction.Right + "(From " + wayY[step] + ":" + wayX[step] + " to " + wayY[step + 1] + ":" + wayX[step + 1] + ")");
                    else if (difY == 0 && difX == 1) Console.WriteLine("Move " + Direction.Left + "(From " + wayY[step] + ":" + wayX[step] + " to " + wayY[step + 1] + ":" + wayX[step + 1] + ")");
                    else if (difX == 0 && difY == 0) break;
                }
            }
            else Console.WriteLine("Starting point is unacceptable");
        }
    }
}