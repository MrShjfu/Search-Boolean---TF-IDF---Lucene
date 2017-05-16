using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Iveonik.Stemmers;

namespace IF_IDF
{
    public class document
    {
        public string Title;
        public string Content;
        public IEnumerable<string> ListWorld;
        public int count;

        public static string[] StopWords = {};
        public string Rank;


        public document(string title, string content, string rg, int c)
        {
            
            var line = title.Split(new[] { @"\", "." }, StringSplitOptions.None);
            Title = line[line.Length - 2];
            Content = content;
            StopWords=new []{""};
            IStemmer stemmer = new EnglishStemmer();
            var regex = "";
            if (rg == null)
                regex = @"[A-Za-z\-]+";
            else
                regex = rg;
            var valueEnumerable = Regex.Matches(content.ToLower(), regex);
            var lt = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList();
            count = lt.Count;
            ListWorld = valueEnumerable.Cast<Match>().Select(match => match.Value).
                ToList().Except(StopWords).OrderBy(a => a).ToList();
            ListWorld = ListWorld.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
        }
        public document(string title, string content, string rg, int qw,int asa)
        {
            Title = title;
            Content = content;
            StopWords = new[] { "" };
            IStemmer stemmer = new EnglishStemmer();
            var regex = "";
            if (rg == null)
                regex = @"[A-Za-z\-]+";
            else
                regex = rg;
            var valueEnumerable = Regex.Matches(content.ToLower(), regex);
            var lt = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList();
            count = lt.Count;
            ListWorld = valueEnumerable.Cast<Match>().Select(match => match.Value).
                ToList().Except(StopWords).OrderBy(a => a).ToList();
            ListWorld = ListWorld.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
        }
        public document(string title, string content,Stopword st,string rg)
        {
            Content = content.ToLower();
            IStemmer stemmer = new EnglishStemmer();
            var regex = "";
            if (rg == null)
                regex = @"[A-Za-z\-]+";
            else
                regex = rg;
            var valueEnumerable = Regex.Matches(content, regex);
            var lt = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList();
            count = lt.Count;
            StopWords = st.Lst.ToArray();
            ListWorld = valueEnumerable.Cast<Match>().Select(match => match.Value).
                ToList().ConvertAll(a=>a.ToLower()).Except(StopWords).OrderBy(a => a);
            ListWorld = ListWorld.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
        }
        public document(string title, string content, Stopword st, string rg,int s)
        {
            Title = title;

            Content = content.ToLower();
            IStemmer stemmer = new EnglishStemmer();
            var regex = "";
            if (rg == null)
                regex = @"[A-Za-z\-]+";
            else
                regex = rg;
            var valueEnumerable = Regex.Matches(content, regex);
            var lt = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList();
            count = lt.Count;
            StopWords = st.Lst.ToArray();
            ListWorld = valueEnumerable.Cast<Match>().Select(match => match.Value).
                ToList().ConvertAll(a => a.ToLower()).Except(StopWords).OrderBy(a => a);
            ListWorld = ListWorld.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
        }



        public document(string s, string title,string a)
        {
            //var line = s.Split(new string[] {"\r\n", "\n"}, StringSplitOptions.None);
            var line = title.Split(new string[] { @"\", "." }, StringSplitOptions.None);
            Title = line[line.Length - 2];
            Content = s;
        }
    }
}