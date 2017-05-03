﻿using System;
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
        public Query(string s,Stopword sw, int e)
        {
            Lstq = new List<Tuple<string, double>>();
            var b = s.Split(new string[] { "\t", "\t" }, StringSplitOptions.None);
            if (b.Length != 2) return;
            IdQuery = b[0];
            Title = b[1];

            const string regex = @"[A-Za-z\-]+";
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

        public Query(string s, Stopword sw)
        {
            const string regex = @"[A-Za-z\-]+";
            IStemmer stemmer = new EnglishStemmer();
            var valueEnumerable = Regex.Matches(s, regex);
            IEnumerable<string> Lst = null;
            try
            {
                Lst = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList().Except(sw.Lst.ToArray()).OrderBy(a => a);
                Lst = Lst.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
            }
            catch (Exception)
            {
                Lst = Lst.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
            }
            var ls = Lst.Distinct().ToList();
            foreach (var item in ls)
            {
                var l = Lst.Select(a => a == item).Count();
                Lstq.Add(new Tuple<string, double>(item, (double)l / ls.Count));
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