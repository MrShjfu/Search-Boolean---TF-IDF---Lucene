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
        public static string[] StopWords = {};
        public string Rank;


        public document(string title, string content)
        {
            var line = title.Split(new[] { @"\", "." }, StringSplitOptions.None);
            Title = line[line.Length - 2];
            Content = content;
            StopWords=new []{""};
            IStemmer stemmer = new EnglishStemmer();
            const string regex = @"[A-Za-z\-]+";
            var valueEnumerable = Regex.Matches(content, regex);
            ListWorld = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList().Except(StopWords).OrderBy(a => a);
            ListWorld = ListWorld.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
        }

        public document(string title, string content,Stopword st)
        {
            var line = title.Split(new[] { @"\", "." }, StringSplitOptions.None);
            Title = line[line.Length - 2];
            Content = content;
            IStemmer stemmer = new EnglishStemmer();
            const string regex = @"[A-Za-z\-]+";
            var valueEnumerable = Regex.Matches(content, regex);
            StopWords = st.Lst.ToArray();
            ListWorld = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList().Except(StopWords).OrderBy(a => a);
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