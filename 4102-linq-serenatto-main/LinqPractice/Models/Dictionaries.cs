using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqPractice.Models
{
    public static class Dictionaries
    {
        public static Dictionary<string, string> GetWords()
        {
            var words = new Dictionary<string, string>();
            words.Add("policeman", "bad_data");
            words.Add("calculation", " ");
            words.Add("thread", "bad_data");
            words.Add("despair", "bad_data");
            words.Add("reflection", "bad_data");
            words.Add("penetrate", "bad_data");
            words.Add("conference", "bad_data");
            words.Add("worry", "bad_data");
            words.Add("divide", "bad_data");
            words.Add("organ", "bad_data");
            words.Add("limited", "bad_data");
            words.Add("smile", "bad_data");
            words.Add("strain", "bad_data");
            words.Add("expect", "bad_data");
            words.Add("alcohol", "bad_data");
            words.Add("moment", "");
            return words;
        }

        public class DuplicatesDTO
        {
            public string Word { get; set; }
            public int? Count { get; set; }
        }

    }
}

