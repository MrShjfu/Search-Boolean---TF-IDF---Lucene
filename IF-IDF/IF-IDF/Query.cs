using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Iveonik.Stemmers;

namespace IF_IDF
{
    public class Query
    {
        public string IdQuery;
        public string Title;
        public string Rank;

        public IEnumerable<string> Lst;
        public Query(string s, int a)
        {
            var b = s.Split(new string[] { " ", "\t" }, StringSplitOptions.None);
            if (b.Length != 3) return;
            IdQuery = b[0];
            Title = b[1];
            Rank = b[2];
        }
        public Query(string s)
        {
            var b = s.Split(new string[] { " ", "\t" }, StringSplitOptions.None);
            if (b.Length != 2) return;
            IdQuery = b[0];
            Title = b[1];
        }
        public Query(string s,Stopword sw, int e)
        {
            var b = s.Split(new string[] { "\t", "\t" }, StringSplitOptions.None);
            if (b.Length != 2) return;
            IdQuery = b[0];
            Title = b[1];

            const string regex = @"[A-Za-z\-]+";
            IStemmer stemmer = new EnglishStemmer();
            var valueEnumerable = Regex.Matches(Title = b[1], regex);
            try
            {
                Lst = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList().Except(sw.Lst.ToArray()).OrderBy(a => a);
                Lst = Lst.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
            }
            catch (Exception)
            {
                Lst = Lst.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
            }
        }

        public Query(string s, Stopword sw)
        {
            const string regex = @"[A-Za-z\-]+";
            IStemmer stemmer = new EnglishStemmer();
            var valueEnumerable = Regex.Matches(s, regex);
            try
            {
                Lst = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList().Except(sw.Lst.ToArray()).OrderBy(a => a);
                Lst = Lst.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
            }
            catch(Exception )
            {
                Lst = Lst.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
            }
        }

        public string Id;
        public string Content;

        public Query(string s,string a)
        {
            var line = s.Split(new string[] { "\t" }, StringSplitOptions.None);
            Id = line[0];
            Content = line[1];
        }

        public Query(string s,float a)
        {
            var b = s.Split(new string[] { " ", "\t" }, StringSplitOptions.None);
            if (b.Length != 3) return;
            Id = b[0];
            Title = b[1];
            Rank = b[2];
        }
    }
}