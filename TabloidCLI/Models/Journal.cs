using System;
using System.Collections.Generic;
using System.Text;

namespace TabloidCLI.Models
{
    public class Journal
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDateTime { get; set; }


        public string JournalProperties
        {
            get
            {
                return $"{Title} " +
                       $"{Content}" +
                       $"{CreateDateTime}";
            }
        }

        public override string ToString()
        {
            return JournalProperties;
        }
    }
}
