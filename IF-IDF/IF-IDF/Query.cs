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
        // tu , tf
        public List<Tuple<string,double>> Lstq;
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
        public Query(string s,Stopword sw, int e,string rg)
        {
            Lstq = new List<Tuple<string, double>>();
            var b = s.Split(new string[] { "\t", "\t" }, StringSplitOptions.None);
            if (b.Length != 2) return;
            IdQuery = b[0];
            Title = b[1];
            var regex = "";
            if (rg == null)
                regex = @"[A-Za-z\-]+";
            else
                regex = rg;
            IStemmer stemmer = new EnglishStemmer();
            var valueEnumerable = Regex.Matches(Title = b[1], regex);
            IEnumerable<string> Lst=null;
            try
            {
                Lst = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList().Except(sw.Lst.ToArray()).OrderBy(a => a);
                Lst = Lst.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
            }
            catch (Exception)
            {
                Lst = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList().OrderBy(a => a);
                Lst = Lst.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
            }
            Dictionary<string, int> counts = Lst.GroupBy(x => x)
                .ToDictionary(g => g.Key,
                    g => g.Count());
            var ls = Lst.Distinct().ToList();
            foreach (var item in counts)
            {
                Lstq.Add(new Tuple<string, double>(item.Key,(double)item.Value/(double)ls.Count));
            }
        }

        public Query(string s, Stopword sw,string rg)
        {
            var regex = "";
            if (rg == null)
                regex = @"[A-Za-z\-]+";
            else
                regex = rg;
            IStemmer stemmer = new EnglishStemmer();
            s=s.ToLower();
            var valueEnumerable = Regex.Matches(s, regex);
            IEnumerable<string> Lst = null;
            Lstq = new List<Tuple<string, double>>();
            if (sw!=null)
            {
                Lst = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList().Except(sw.Lst.ToArray()).OrderBy(a => a);
                Lst = Lst.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
            }
            else
            {
                Lst = valueEnumerable.Cast<Match>().Select(match => match.Value).OrderBy(a => a).ToList();
                Lst = Lst.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
            }
            var ls = Lst.Distinct().ToList();
            foreach (var item in ls)
            {
                var l = Lst.Count(a=>a==item);
                Lstq.Add(new Tuple<string, double>(item, (double)l / Lst.ToList().Count));
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