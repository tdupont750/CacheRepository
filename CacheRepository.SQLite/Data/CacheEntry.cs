using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CacheRepository.SQLite.Data
{
    [Table("CacheEntry")]
    public class CacheEntry
    {
        private DateTime _createdUtc;

        private DateTime _modifiedUtc;

        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }

        public DateTime CreatedUtc
        {
            get { return _createdUtc; }
            set { _createdUtc = value.ToUniversalTime(); }
        }

        public DateTime ModifiedUtc
        {
            get { return _modifiedUtc; }
            set { _modifiedUtc = value.ToUniversalTime(); }
        }
    }
}