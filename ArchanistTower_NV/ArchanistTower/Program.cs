using System;

namespace ArchanistTower
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ArchanistTower game = new ArchanistTower())
            {
                game.Run();
            }
        }
    }
}

