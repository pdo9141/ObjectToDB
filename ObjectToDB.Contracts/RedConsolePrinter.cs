using System;

namespace ObjectToDB.Contracts
{
    [Serializable]
    public class RedConsolePrinter : IConsolePrinter
    {
        public string Context { get; set; }

        public string Name
        {
            get { return "Red Console"; }
        }

        public void Print()
        {
            Console.WriteLine("Printing from {0}", Name);
        }
    }
}
