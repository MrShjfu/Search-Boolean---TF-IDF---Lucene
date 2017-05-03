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
        public static string[] stop_words = {"a", "an", "for", "of", "that", "on","in", "at","and","as","by","or","the","to","are","be", "is" ,"was", "were"};

        public document(string title, string content)
        {
            var line = title.Split(new string[] { @"\", "." }, StringSplitOptions.None);
            Title = line[line.Length - 2];
            Content = content;
            IStemmer stemmer = new EnglishStemmer();
            const string regex = @"[A-Za-z\-]+";
            var valueEnumerable = Regex.Matches(content, regex);
            ListWorld = valueEnumerable.Cast<Match>().Select(match => match.Value).ToList().Except(stop_words).OrderBy(a=>a);
            ListWorld = ListWorld.ToList().ConvertAll(d => stemmer.Stem(d.ToLower()));
        }

    }
}