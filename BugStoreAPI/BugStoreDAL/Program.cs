using System;
using BugStoreDAL.EF.Initializers;

namespace BugStoreDAL
{
    class Program
    {
        static void Main(string[] args)
        {
            Initializer.InitializeData();
        }
    }
}
