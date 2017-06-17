using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace IF_IDF
{
    public class SearchQuery
    {
        public Query Query;
        private BoardBoolean _boolean;

        public List<Tuple<string, List<string>>> LstResult;
        public SearchQuery(Query query, BoardBoolean boolean)
        {
            Query = query;
            _boolean = boolean;
        }


        public List<KeyValuePair<string, double>> FileResult()
        {
            var dic = _boolean.GetDictionList();
            var lst = Query.Lstq;
            //tên từ, Tuple<chỉ số tài liêu, tần số, idf.<List<tuple<tài liệu,tần số,tf,tf*idf>>>
            IDictionary<string, Tuple<int, int, double, List<Tuple<string, int, float, double>>>> listItem = new Dictionary< string, Tuple< int, int, double, List< Tuple < string, int,float,double>>>>();
            Tuple<int, int, double, List<Tuple<string, int, float, double>>> Item;
            var lstQuery = new List<Tuple<string,double>>();
            //query: tu, tf
            lst = lst.Distinct().ToList();
            foreach (var item in lst)
            {
                if (!dic.ContainsKey(item.Item1)) continue;
                Item = dic.First(a => a.Key == item.Item1).Value;
                //tu, tf*idf
                lstQuery.Add(new Tuple<string,double>(item.Item1, item.Item2* dic[item.Item1].Item3));// tu ,tf
                listItem.Add(item.Item1, Item);
            }
            //tai lieu, tu, tf*idf
            var lstFile = (from item in listItem from subitem in item.Value.Item4
                           select new Tuple<string, string, double>(subitem.Item1, item.Key, subitem.Item4))
                           .OrderBy(a=>a.Item1).ToList();
            // gop lai 
            var dicFile  = new Dictionary<string,List<Tuple<string,double>>>();
            foreach (var item in lstFile)
            {
                if (dicFile.ContainsKey(item.Item1) == true)
                {
                    dicFile[item.Item1].Add(new Tuple<string, double>(item.Item2,item.Item3));
                }
                else
                {
                    var tup = new Tuple<string,double>(item.Item2,item.Item3);
                    dicFile.Add(item.Item1 ,new List<Tuple<string, double>>(new Tuple<string, double>[] {tup}));
                }
            }
            var docTfidf = new Dictionary<string, double>();
            foreach (var item in dicFile)
            {
                var doDaiTaiLieu = Math.Sqrt(item.Value.Sum(a => Math.Pow(a.Item2, 2)));
                var file = item.Value.ToList();
                var d = 0.0;
                var x = item.Value.Sum(subitem => lstQuery.FirstOrDefault(a => a.Item1 == subitem.Item1).Item2 * subitem.Item2);
                var doDaiQuery = Math.Sqrt(lstQuery.Sum(e =>Math.Pow(e.Item2,2)));
                var c = (float)((float)x / ((float)doDaiTaiLieu * (float)doDaiQuery));
                docTfidf.Add(item.Key,(double)c);
            }   
            var listFileSortByFre = docTfidf.ToList();
            return listFileSortByFre;

            //tai lieu,tu,tf*idf
            //new Dictionary<string,List<Tuple<string,double>>>();
            //var listFileSortByFre = dicFile.ToList().OrderByDescending(a => a.Value.Item2).ThenBy(a => a.Value.Item1).Where(a => a.Value.Item1 >= 3);
            //return listFileSortByFre;
        }
    }
}













