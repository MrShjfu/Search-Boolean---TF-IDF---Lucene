using System;
using System.Collections.Generic;
using System.Linq;

namespace IF_IDF
{
    public class SearchAON
    {
        public QueryAON Query;
        private IEnumerable<List<string>> _boolean;

        public List<Tuple<string, int, string>> lst;

        public SearchAON(QueryAON qr, IEnumerable<List<string>> bl)
        {
            Query = qr;
            _boolean = bl;
        }

        public List<Tuple<string, string>> resultFile()
        {
            var db = _boolean.ToList();
            var rb = new Dictionary<string, List<string>>();
            foreach (var list in db)
                rb.Add(list[0], list.GetRange(1, list.Count - 1).ToList());
            var word = new Dictionary<string,Tuple<int,string>>();

            var qr = Query.lst;
            foreach (var item in qr)
            {
                if (rb.ContainsKey(item.Item1))
                {
                    var str = string.Join("", rb[item.Item1].GetRange(1, rb[item.Item1].Count - 1).ToList());
                    word.Add(item.Item1, new Tuple<int, string>(item.Item2, str));
                }
                else
                {
                    word.Add(item.Item1, new Tuple<int, string>(item.Item2, null));
                }
            }
            var listword = new List<Tuple<string,int,string>>();
            foreach (var item in word)
            {

            }
            //tu, string
            return null;
        }
    }
}