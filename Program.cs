using System;

namespace Client
{
    /// <summary>Class <c>Program</c> is the main class for simulating a repository</summary>
    ///
    class Program
    {
        static void Main(string[] args)
        {
            Simulator simulator = new Simulator();
            simulator.Init();

            Console.WriteLine("Simulator initialised, simulation started");

            simulator.Simulate();

            Console.WriteLine("Simulation finished, repository succesfully created");
        }
    }
}
