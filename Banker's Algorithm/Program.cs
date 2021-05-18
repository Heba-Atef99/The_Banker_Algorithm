using System;
using System.Collections.Generic;

namespace Banker_s_Algorithm
{
    class Program
    {
        static int Main(string[] args)
        {
            //get processes
            Console.WriteLine("Enter number of Processes: ");
            int n = Convert.ToInt32(Console.ReadLine());
           
            //get resources
            Console.WriteLine("\nEnter number of Resources: ");
            int m = Convert.ToInt32(Console.ReadLine());
            
            //get the rest of the matrices
            int[] Available = new int[m];
            int[,] Max = new int[n, m];
            int[,] Allocation = new int[n, m];

            getMatrixInput(Available, m, "Available");
            getMatrixInput(Allocation, n, m, "Allocation");
            getMatrixInput(Max, n, m, "Max");

            int[,] Need = new int[n, m];

            //calculate need matrix
            Console.WriteLine("\nThe Need Matrix is");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("\nFor P{0}", i);
                for (int j = 0; j < m; j++)
                {
                    Need[i, j] = Max[i, j] - Allocation[i, j];
                    Console.WriteLine("R{0} => {1}", j, Need[i, j]);
                }
            }

            Console.WriteLine("\nDo you want to know the safe state? (y/n) ");
            if (Console.ReadLine() == "y")
            {
                int[] output = new int[n];
                if (Safety(n, m, Available, Allocation, Need, output) == true)
                    printSequence(output, -1);
            }

            while (true)
            {
                Console.WriteLine("\nIs there any process that need resource request? (y/n) ");
                if (Console.ReadLine() == "y")
                {
                    //0 => process ID, 1 => resource ID, 2 => instances of R
                    int[] Resource = new int[m + 1];
                    Console.WriteLine(" ");
                    Console.WriteLine("Enter the ID of that process (EX P0 has ID = 0) ");
                    Resource[0] = Convert.ToInt32(Console.ReadLine());

                    if (Resource[0] >= n)
                    {
                        Console.WriteLine("Invalid Process ID");
                        return 1;
                    }

                    for (int i = 0; i < m; i++)
                    {
                        Console.WriteLine("Enter the instances of R{0}", i);
                        Resource[i + 1] = Convert.ToInt32(Console.ReadLine());
                    }

                    Resource_Request(n, m, Resource, Available, Allocation, Need);
                }
                else
                {
                    break;
                }
            }


            return 0;
        }

        static bool Safety(int n, int m, int[] Available, int[,] Allocation, int[,] Need, int[] output)
        {
            int[] Work = new int[m];
            int[] Finish = new int[n];
            //calculate work matrix
            Available.CopyTo(Work, 0);
            int k = 0;
            short flag = 0;
            for (int i = 0; i < n; i++)
            {
                if(Finish[i] == 0)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (Need[i, j] <= Work[j])
                        {
                            flag++;
                        }
                    }
                    if (flag == m)
                    {
                        for (int j = 0; j < m; j++)
                        {
                            Work[j] += Allocation[i, j];
                        }
                        output[k++] = i;
                        Finish[i] = 1;
                        i = (i == n - 1) ? -1 : i;
                    }
                    flag = 0;
                }
            }

            for (int i = 0; i < n; i++)
            {
                if (Finish[i] == 0)
                {
                    Console.WriteLine("No");
                    return false;
                }
            }

            return true;
        }

        static void Resource_Request(int n, int m, int[] Resource, int[] Available, int[,] Allocation, int[,] Need)
        {
            int[] output = new int[n];

            for (int i = 1; i < m + 1; i++)
            {
                if (Resource[i] <= Need[Resource[0], i - 1])
                {
                    if (Resource[i] <= Available[i - 1])
                    {
                        Available[i - 1] -= Resource[i];
                        Allocation[Resource[0], i - 1] += Resource[i];
                        Need[Resource[0], i - 1] -= Resource[i];
                    }
                    else
                    {
                        Console.WriteLine("\nNo");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Error, process has exceeded its maximum claim");
                    return;
                }

            }

            if (Safety(n, m, Available, Allocation, Need, output) == true)
                printSequence(output, Resource[0]);
        }

        static void printSequence (int[] output, int req)
        {

            if (req != -1)
                Console.Write("Yes request can be granted with safe state , Safe state <");

            else
                Console.Write("Yes, Safe state <");

            for (int i = 0; i < output.Length; i++)
            {

                if (req == output[i]) Console.Write("P{0}req", output[i]);

                else
                    Console.Write("P{0}", output[i]);

                if (i != output.Length - 1)
                    Console.Write(",");

            }
            Console.Write(">\n");
        }

        static void getMatrixInput(int[,] matrix, int p, int r, string name)
        {
            Console.WriteLine("\nEnter " + name + " Matrix: ");
            for (int i = 0; i < p; i++)
            {
                Console.WriteLine("For P{0} ", i);
                for (int j = 0; j < r; j++)
                {
                    Console.WriteLine("R{0} ", j);
                    matrix[i, j] = Convert.ToInt32(Console.ReadLine());
                }
                Console.WriteLine(" ");
            }
        }

        static void getMatrixInput(int[] matrix, int r, string name)
        {
            Console.WriteLine("\nEnter " + name + " Matrix: ");
            for (int i = 0; i < r; i++)
            {
                Console.WriteLine("R{0} ", i);
                matrix[i] = Convert.ToInt32(Console.ReadLine());
            }
        }

    }
}
