using System;
using System.Collections.Generic;
using System.Linq;

namespace IF_IDF
{
    public class Stopword
    {
        public List<string> Lst;
        public Stopword(string s)
        {
            var line = s.Split(new[] { Environment.NewLine}, StringSplitOptions.None);
            Lst = line.ToList();
        }
    }
}