﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Accord.Imaging;
using Accord.Imaging.Converters;
using Accord.Imaging.Filters;

namespace csgol
{
    class Program
    {
        private const int Height = 50;
        private const int Width = 50;

        private static readonly int[,] Kernel =
        {
            {
                1, 1, 1
            },
            {
                1, 0, 1
            },
            {
                1, 1, 1
            }
        };

        private static readonly Convolution Convolution = new Convolution(Kernel);

        private static readonly MatrixToImage MatrixToImage = new MatrixToImage();

        static void Main(string[] args)
        {
            Console.WriteLine("csgol..");
            var stopWatch = Stopwatch.StartNew();
            var grid = ZeroGrid();
            //PrintGrid(grid);
            grid = LoadComponent(grid, 5, 4, "gosperglidergun.txt");
            for (var i = 0; i < 1000; i++)
            {
                PrintGrid(grid);
                Thread.Sleep(5);
                var _ = GetNextGeneration2dConv(grid);
                grid = GetNextGeneration(grid);
            }

            stopWatch.Stop();
            //PrintGrid(grid);
            Console.WriteLine($"done. Elapsed: {stopWatch.ElapsedMilliseconds}ms");
        }

        private static void PrintGrid(int[,] grid)
        {
            Console.Clear();
            var gridStringBuilder = new StringBuilder();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                var stringBuilder = new StringBuilder();
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    stringBuilder.Append($"{(grid[i, j] == 1 ? 'O' : '.')} ");
                }

                gridStringBuilder.AppendLine(stringBuilder.ToString());
            }

            Console.WriteLine(gridStringBuilder.ToString());
        }

        private static int[,] GetNextGeneration2dConv(int[,] grid)
        {
            var nextGrid = ZeroGrid();
            // MatrixToImage.Format = PixelFormat.Format1bppIndexed;
            // MatrixToImage.Convert(grid, out Bitmap bitmap);
            var bitmap = grid.ToBitmap();
            var neighborCount = Convolution.Apply(bitmap);
            
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (grid[x, y] == 1)
                    {
                    }
                }
            }

            return nextGrid;
        }

        private static int[,] GetNextGeneration(int[,] grid)
        {
            var nextGrid = ZeroGrid();
            for (int y = 0; y < Height; y++)
            {
                var yBefore = y > 0 ? y - 1 : Height - 1;
                var yAfter = y < Height - 1 ? y + 1 : 0;
                for (int x = 0; x < Width; x++)
                {
                    var xBefore = x > 0 ? x - 1 : Width - 1;
                    var xAfter = x < Width - 1 ? x + 1 : 0;
                    var neighborSum = grid[yBefore, xBefore] + grid[yBefore, x] + grid[yBefore, xAfter] +
                                      grid[y, xBefore] + grid[y, xAfter] +
                                      grid[yAfter, xBefore] + grid[yAfter, x] + grid[yAfter, xAfter];
                    if (grid[y, x] == 0)
                    {
                        if (neighborSum == 3)
                        {
                            nextGrid[y, x] = 1;
                        }
                    }
                    else
                    {
                        if (neighborSum == 2 || neighborSum == 3)
                        {
                            nextGrid[y, x] = 1;
                        }
                    }
                }
            }

            return nextGrid;
        }

        static int[,] ZeroGrid()
        {
            return new int[Height, Width];
        }


        static int[,] LoadComponent(int[,] grid, int i, int j, string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            var iStart = i;
            foreach (var line in lines)
            {
                foreach (var c in line)
                {
                    grid[j, i] = c == '.' ? 0 : 1;
                    i++;
                }

                i = iStart;
                j++;
            }

            return grid;
        }
    }
}