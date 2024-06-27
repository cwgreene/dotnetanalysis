using System;

namespace ExampleNamespace
{
    class ExampleClass
    {
        void ExampleMethod(Joe bob)
        {
            Console.WriteLine("Hello, World!");
            AnotherMethod();
            bob.chicken();
        }

        void AnotherMethod()
        {
            Console.WriteLine("Another method");
        }
    }

    abstract class Bob {
        public abstract void chicken();
    }

    abstract class Joe {
        public void chicken() {
        }
    }
}
