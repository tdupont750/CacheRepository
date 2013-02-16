namespace CacheRepository.Tests.Utilities.One
{
    public class Cat
    {
        public string Name { get; set; }
    }

    public class Dog
    {
        public string Name { get; set; }
    }
}

namespace CacheRepository.Tests.Utilities.Two
{
    public class Cat
    {
        public string Name { get; set; }
    }

    public class Dog : One.Dog
    {

    }
}
