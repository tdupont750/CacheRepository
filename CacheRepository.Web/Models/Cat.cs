using System;

namespace CacheRepository.Web.Models
{
    public class Cat
    {
        public Cat()
        {
            Guid = Guid.NewGuid();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Guid Guid { get; private set; }
    }
}