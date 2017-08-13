using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing.Imaging;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Program
    {
        private const int size = 10;
        private const double Epsilon = 0.01;
        private const double LearningDelta = 0.1;
        private const double Threshold = 0.5;

        static void Main(string[] args)
        {
            var weights = new double[size, size];

            //Learning

            var input = GetImage(@"TestData\1.bmp", size);
            double[,] output = Learn(input, weights, size);

            input = GetImage(@"TestData\1_italics.bmp", size);
            output = Learn(input, weights, size);

            input = GetImage(@"TestData\1_bold.bmp", size);
            output = Learn(input, weights, size);

            //Recognition

            input = GetImage(@"TestData\1.bmp", size);
            output = Recognise(input, weights, size);
            Output(input, output);

            Console.WriteLine();

            input = GetImage(@"TestData\1_italics.bmp", size);
            output = Recognise(input, weights, size);
            Output(input, output);

            Console.WriteLine();

            input = GetImage(@"TestData\1_bold.bmp", size);
            output = Recognise(input, weights, size);
            Output(input, output);

            Console.WriteLine();

            input = GetImage(@"TestData\2.bmp", size);
            output = Recognise(input, weights, size);
            Output(input, output);

            Console.WriteLine();

            input = GetImage(@"TestData\1_hand.bmp", size);
            output = Recognise(input, weights, size);
            Output(input, output);

            Console.ReadLine();
        }

        private static double[,] Learn(double[,] input, double[,] weights, int size)
        {
            var output = new double[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var currentOutput = weights[i, j] * input[i, j];
                    var outputValue = currentOutput > Threshold ? 1d : 0d;
                    while (input[i, j] != outputValue)
                    {
                        if (outputValue == 1)
                        {
                            weights[i, j] -= LearningDelta;
                        }
                        else
                        {
                            weights[i, j] += LearningDelta;
                        }

                        currentOutput = weights[i, j] * input[i, j];
                        outputValue = currentOutput > Threshold ? 1d : 0d;
                    }

                    output[i, j] = outputValue;
                }
            }

            return output;
        }

        private static double[,] Recognise(double[,] input, double[,] weights, int size)
        {
            var output = new double[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var currentOutput = weights[i, j] * input[i, j];
                    var outputValue = currentOutput > Threshold ? 1d : 0d;
                    output[i, j] = outputValue;
                }
            }

            return output;
        }

        private static void Output(double[,] input, double[,] output)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(input[i, j] == 0 ? "." : "|");
                }

                Console.Write("  ");

                for (int j = 0; j < size; j++)
                {
                    Console.Write(output[i, j] == 0 ? "." : "|");
                }

                Console.WriteLine();
            }
        }

        private static double[,] GetImage(string path, int size)
        {
            double[,] result = new double[size, size];
            var whiteColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            var fullPath = Path.Combine(Environment.CurrentDirectory, path);
            using (var image = new System.Drawing.Bitmap(fullPath))
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        var color = image.GetPixel(j, i);
                        result[i, j] = color == whiteColor ? 0 : 1;
                    }
                }
            }

            return result;
        }
    }
}
