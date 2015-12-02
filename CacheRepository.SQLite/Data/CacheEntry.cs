using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CacheRepository.SQLite.Data
{
    [Table("CacheEntry")]
    public class CacheEntry
    {
        /* 
         * CREATE TABLE "main"."CacheEntry" (
         *  "Id" INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL , 
         *  "Key" VARCHAR(255) NOT NULL  UNIQUE , 
         *  "Value" TEXT NOT NULL , 
         *  "SlideSpan" VARCHAR(255) , 
         *  "ExpireUtc" DATETIME)
         */

        private DateTime? _createdUtc;

        public long Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string SlideSpan { get; set; }

        public DateTime? ExpireUtc
        {
            get { return _createdUtc; }

            set
            {
                _createdUtc = value.HasValue 
                    ? value.Value.ToUniversalTime() 
                    : (DateTime?) null;
            }
        }

        public TimeSpan? GetSlideSpan()
        {
            if (string.IsNullOrWhiteSpace(SlideSpan))
                return null;

            TimeSpan timeSpan;
            return TimeSpan.TryParse(SlideSpan, out timeSpan) 
                ? timeSpan
                : (TimeSpan?) null;
        }

        public void SetSlideSpan(TimeSpan? timeSpan)
        {
            SlideSpan = timeSpan.HasValue
                ? timeSpan.ToString()
                : null;
        }
    }
}