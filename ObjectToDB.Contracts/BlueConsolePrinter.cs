using System;

namespace ObjectToDB.Contracts
{
    [Serializable]
    public class BlueConsolePrinter : IConsolePrinter
    {
        public UserDetail UserDetail { get; set; }

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
