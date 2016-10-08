using System;

namespace ObjectToDB.Contracts
{
    [Serializable]
    public class BlueConsolePrinter : IConsolePrinter
    {
        public string Context { get; set; }

        public string Name
        {
            get { return "Blue Console"; }
        }

        public void Print()
        {
            Console.WriteLine("Printing from {0}", Name);
        }
    }
}
