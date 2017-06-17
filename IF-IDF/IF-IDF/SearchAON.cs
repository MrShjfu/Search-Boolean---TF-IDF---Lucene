using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Lucene.Net.Util;
using org.mariuszgromada.math;
using org.mariuszgromada.math.mxparser;

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



        public List<string> resultFile()
        {
            var db = _boolean.ToList();
            var rb = new Dictionary<string, List<string>>();
            var listPage = db[0].ToList();
            listPage.RemoveAt(0);
            listPage.Reverse();
            int k = 0;
            foreach (var list in db)
            {
                if (list[0] == "") continue;
                rb.Add(list[0], list.GetRange(1, list.Count - 1).ToList());
            }
            var word = new Dictionary<string, Tuple<int, string>>();

            var qr = Query.lst;
            foreach (var item in qr)
            {
                if (rb.ContainsKey(item.Item1))
                {
                    var str = string.Join("", rb[item.Item1].GetRange(0, rb[item.Item1].Count).ToList());
                    word.Add(item.Item1, new Tuple<int, string>(item.Item2, str));
                }
                else
                {
                    word.Add(item.Item1, new Tuple<int, string>(item.Item2, null));
                }
            }


            var lst = Query.lst;
            var Lst = Query.Lst;
            var List = "";
            foreach (var item in lst)
            {
                if (word.ContainsKey(item.Item1))
                {
                    var index = Lst.IndexOf(item.Item2.ToString());
                    if (index != -1)
                    {
                        if (word[item.Item1].Item2 != "")
                            Lst[index] = word[item.Item1].Item2;
                        else
                            Lst[index] = "0";
                    }
                }
            }

            var result1 = "";
            var opera = new List<string>();
            var argu = new List<string>();
            int ww = 0;
            foreach (var s in Lst)
            {
                if (s == "and")
                    opera.Add(" @& ");
                else if (s == "or")
                    opera.Add(" @| ");
                else if (s == "not")
                    opera.Add(" @~");
                else if (s == ")")
                    opera.Add(" )");
                else if (s == "(")
                    opera.Add("( ");
                else
                {
                    opera.Add(ww.ToString());
                    if (s == null)
                        argu.Add("0");
                    else
                        argu.Add(s);
                    ww++;
                }
            }
            int hq = 0;
            var result = "";
            for (int i = 0; i < listPage.Count; i++)
            {
                var v = opera;
                var stringresul = "";
                foreach (var item in opera)
                {
                    if (int.TryParse(item.ToString(), out hq) == false)
                    {
                        stringresul += item;
                    }
                    else
                    {
                        if (argu[hq].Length==1)
                        {
                            stringresul += "0";
                        }else
                        stringresul += argu[hq][i].ToString();
                    }
                }
                Expression ex = new Expression(stringresul);
                string h = ex.calculate().ToString();
                result += Convert.ToString(int.Parse(h), 2).ToString();
            }

            var sub = listPage.Count - result.Length;
            for (int i = 0; i < sub; i++)
            {
                result.Insert(0, "0");
            }
            var resultFile = new List<string>();
            listPage.Reverse();
            for (int i = 0; i < listPage.Count; i++)
            {
                if (result[i].ToString() == "1".ToString())
                {
                    resultFile.Add(listPage[i]);
                }
            }
            if (resultFile == null)
            {
                return null;
            }
            else
                return resultFile;
        }
    }
}