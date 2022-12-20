using System.Collections;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace BiggerNumber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //choose number of threads
            int numberOfThreads = 2;
            //int numberOfThreads = 3;
            //int numberOfThreads = 4;
            //int numberOfThreads = 6;

            //initial number
            int number = 2;

            //array for intemediate result
            int[] resNumber = new int[numberOfThreads];
            Console.WriteLine("Initial number: ", number);
            Console.WriteLine(number);

            //array where we find bigger number
            int[] arrayOfNumbers = createArray();
            //print 50 numbers from array
            Console.WriteLine();
            Console.WriteLine("50 numbers from array:");
            printArray(arrayOfNumbers);

            int sizeOfArray = arrayOfNumbers.Length;
            int sizeOfArrayPart = (int)(sizeOfArray % numberOfThreads == 0
                ? (sizeOfArray / numberOfThreads)
                : (sizeOfArray / numberOfThreads) + 1);
            var startingPosition = 0;

            //defirne list of Tasks for parallel sorting
            var tasks = new List<Task>();
            

            //parallel finding of bigger number
            for (var i = 0; i < numberOfThreads; i++)
            {
                tasks.Add(FindBubberNumber(number, arrayOfNumbers, startingPosition, sizeOfArrayPart, i, resNumber));
                startingPosition += sizeOfArrayPart;
            }
            Task.WaitAll(tasks.ToArray());            

            //final loop of finding number
            int finalResult = FindBubberNumberSync(number, resNumber);

            Console.WriteLine();
            Console.WriteLine("Result: ", finalResult);
            Console.WriteLine(finalResult);

        }


        // fill array of numbers with random values
        static int[] createArray()
        {

            int[] listOfNumbers = new int[100_000];
            Random randomNumbers = new Random();
            int MinimumValue = 0;
            int MaximumValue = 100_000;


            for (int i = 0; i < 100_000; i++)
            {
                listOfNumbers[i] = randomNumbers.Next(MinimumValue, MaximumValue);
            }
            return listOfNumbers;
        }

        //findBubberNumber async
        static Task FindBubberNumber(int number, int[] listOfNumbers, int startIndex, int sizeOfArrayPart, int resNumberIndex, int[] resNumber )
        {
            return Task.Run(() =>
            {
                int endIndex = 0;
                if (startIndex + sizeOfArrayPart < listOfNumbers.Length)
                {
                    endIndex = startIndex + sizeOfArrayPart;
                }
                else
                {
                    endIndex = listOfNumbers.Length;
                }

                int result = number;
                for (int i = 0; i < listOfNumbers.Length; i++)
                {
                    if (listOfNumbers[i] >= number)
                    {
                        result = listOfNumbers[i];
                        number = result;
                    }
                }

                resNumber[resNumberIndex] = result;
                

            });
        }

        //find number sync
        static int FindBubberNumberSync(int number, int[] listOfNumbers)
        {
            int result = number;
            for (int i = 0; i < listOfNumbers.Length; i++)
            {
                if (listOfNumbers[i] >= number)
                {
                    result = listOfNumbers[i];
                    number = result;
                }                
            }
            return result;
        }

        //print an array of numbers from index 1000 to index 1050
        static void printArray(int[] listOfNumbers)
        {
            for (int i = 1000; i < 1050; i++)
            {
                Console.Write(listOfNumbers[i] + " ");
            }
            Console.WriteLine();
        }
    }
}
