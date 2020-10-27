using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HashSetByProp
{
    class Program
    {
        static void Main(string[] args)
        {
            DuplicationInHashSet();
            Console.Clear();
            FixingDuplicationInHashSet();
            Console.Clear();
            FirstOrDefaultPerformanceTest();
        }
        private static void DuplicationInHashSet()
        {
            Console.WriteLine("Hashset problem: object with identical properties are all added to the set.");
            Console.WriteLine("Object equality is based on reference");
            var msg1 = new MessageDummy { AlmostGuid = "guid-0000" };
            var msg2 = new MessageDummy { AlmostGuid = "guid-0000" };
            var msg3 = new MessageDummy { AlmostGuid = "guid-0000" };

            var set = new HashSet<MessageDummy>();

            Console.WriteLine("Add first message");
            set.Add(msg1);
            Console.WriteLine($"Set has {set.Count} elements"); // set.Count = 1

            Console.WriteLine("Add second message with identical property");
            set.Add(msg2);
            Console.WriteLine($"Set has {set.Count} elements"); // set.Count = 2

            Console.WriteLine("Add third message with identical property");
            set.Add(msg3);
            Console.WriteLine($"Set has {set.Count} elements"); // set.Count = 3
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
        }
        private static void FixingDuplicationInHashSet()
        {
            Console.WriteLine("Fix for the hashset problem: override equals method");
            var msg1 = new EqualOverrideMessageDummy { AlmostGuid = "guid-0000" };
            var msg2 = new EqualOverrideMessageDummy { AlmostGuid = "guid-0000" };
            var msg3 = new EqualOverrideMessageDummy { AlmostGuid = "guid-0000" };

            var set = new HashSet<EqualOverrideMessageDummy>();

            Console.WriteLine("Add first message");
            set.Add(msg1);
            Console.WriteLine($"Set has {set.Count} elements"); // set.Count = 1

            Console.WriteLine("Add second message with identical property");
            set.Add(msg2);
            Console.WriteLine($"Set has {set.Count} elements"); // set.Count = 1

            Console.WriteLine("Add third message with identical property");
            set.Add(msg3);
            Console.WriteLine($"Set has {set.Count} elements"); // set.Count = 1
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
        }
        private static void FirstOrDefaultPerformanceTest()
        {
            Console.WriteLine($"HashSet FirstOrDefault time complexity: Retrieving an element based on a property from a HashSet");

            Console.WriteLine($"To test whether or not using hashSet.FirstOrDefault with a property-filter affects the runtime,");
            Console.WriteLine($"we compare how long it takes to find an element in a large and in a smaller set.");
            var bigSetCount = (int)Math.Pow(2, 23);
            var smallSetCount = (int)Math.Pow(2, 18); // 32 times as small!!!
            var sw = new Stopwatch();

            Console.WriteLine($"Generating set of {bigSetCount} elements...");
            var bigSet = GenerateHashSetOfNElements(bigSetCount);

            Console.WriteLine($"Generating set of {smallSetCount} elements");
            var smallSet = GenerateHashSetOfNElements(smallSetCount);

            // Finding an element by property in a large hashset like this takes ~150 ms
            Console.WriteLine($"Looking for element [bigSet.FirstOrDefault(x => x.Prop == bigSetCount - 1)] in big set");
            sw.Start();
            Console.WriteLine($"Element found: {{ Prop: {bigSet.FirstOrDefault(x => x.Prop == bigSetCount - 1).Prop} }}");
            sw.Stop();
            var bigSetTime = sw.ElapsedMilliseconds;
            Console.WriteLine($"Operation took {bigSetTime} milliseconds");


            // Finding an element by property in a small hashset takes ~5 ms
            Console.WriteLine($"Looking for element [smallSet.FirstOrDefault(x => x.Prop == smallSetCount - 1)] in big set");
            sw.Restart();
            Console.WriteLine($"Element found: {{ Prop: {smallSet.FirstOrDefault(x => x.Prop == smallSetCount - 1).Prop} }}");
            sw.Stop();
            var smallSetTime = sw.ElapsedMilliseconds;
            Console.WriteLine($"Operation took {smallSetTime} milliseconds");

            Console.WriteLine($"In a set {bigSetCount / smallSetCount} times bigger the search took ~{bigSetTime / smallSetTime} times longer.");
            Console.WriteLine($"Searching time scales linearly with the length.");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey(true);
        }
        private static HashSet<MyClass> GenerateHashSetOfNElements(int n)
        {
            var s = new HashSet<MyClass>();
            for (var i = 0; i < n; i++)
            {
                s.Add(new MyClass { Prop = i });
            }

            return s;
        }
    }

    class MyClass
    {
        public int Prop { get; set; }
    }

    class MessageDummy
    {
        public string AlmostGuid { get; set; }
    }
    class EqualOverrideMessageDummy
    {
        public string AlmostGuid { get; set; }

        public override bool Equals(object o)
        {
            return ((EqualOverrideMessageDummy)o).AlmostGuid == AlmostGuid;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AlmostGuid);
        }
    }
}
