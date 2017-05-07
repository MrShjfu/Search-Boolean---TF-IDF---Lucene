using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Iveonik.Stemmers;

namespace IF_IDF
{
    public class QueryAON
    {
        public string s;
        public List<Tuple<string,int>> lst;
        public List<string> Lst;
        public List<string> Lst1;


        public QueryAON(string s, Stopword sw)
        {
            lst = new List<Tuple<string, int>>();
            const string regex = @"[A-Za-z\-()]+";
            IStemmer stemmer = new EnglishStemmer();
            var valueEnumerable = Regex.Matches(s, regex);
            Lst = new List<string>();
            Lst1=new List<string>();
            var Lstq = new List<Tuple<string, double>>();
            if (sw != null)
            {
                var dt = sw.Lst.Where(a => a != "AND" && a != "OR" && a != "NOT").ToList();
                Lst = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList().Except(dt.ToArray()).ToList();
                Lst = Lst.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
            }
            else
            {
                Lst = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList();
                Lst = Lst.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
            }

            var i = 0;
            for (var j = 0; j < Lst.Count; j++)
            {
                {
                    if (Lst[j] == "and" || Lst[j] == "or" || Lst[j] == "not" || Lst[j] == "(" || Lst[j] == ")") continue;
                    lst.Add(new Tuple<string, int>(Lst[j], i));
                    Lst1.Add(Lst[j]);
                    Lst[j] = i.ToString();
                    i++;
                }
            }

        }
    }
}