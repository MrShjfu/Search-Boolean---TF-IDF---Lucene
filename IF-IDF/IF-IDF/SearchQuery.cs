using System;
using System.Collections;
using System.Collections.Generic;
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


        public IEnumerable<KeyValuePair<string, Tuple<int,int>>> FileResult()
        {
            var dic = _boolean.GetDictionList();
            var lst = Query.Lst;
            //tên từ, Tuple<chỉ số tài liêu, tần số, idf.<List<tuple<tài liệu,tần số,tf,tf*idf>>>
            IDictionary<string, Tuple<int, int, double, List<Tuple<string, int, float, double>>>> listItem = new Dictionary< string, Tuple< int, int, double, List< Tuple < string, int,float,double>>>>();
            Tuple<int, int, double, List<Tuple<string, int, float, double>>> Item;
            lst = lst.Distinct().ToList();
            foreach (var item in lst)
            {
                if (!dic.ContainsKey(item)) continue;
                Item = dic.First(a => a.Key == item).Value;
                listItem.Add(item, Item);
            }


            // tài liệu, từ, tần số trong tài liệu đó
            var lstFile = (from item in listItem from subItem in item.Value.Item4 select new Tuple<string, string, int>(subItem.Item1, item.Key, subItem.Item2)).OrderBy(a => a.Item1).ToList();
            var dicFile = new Dictionary<string, Tuple<int,int>>();
            foreach (var item in lstFile)
            {
                if (dicFile.ContainsKey(item.Item1) == true)
                {
                    var world = dicFile[item.Item1].Item1 + 1;// so tu 
                    var fre = dicFile[item.Item1].Item2 + item.Item3;// tan so
                    dicFile[item.Item1] = new Tuple<int, int>(world, fre);
                }
                else
                {
                    dicFile.Add(item.Item1,new Tuple<int, int>(1,item.Item3));
                }
            }
            var listFileSortByFre= dicFile.ToList().OrderByDescending(a => a.Value.Item2).ThenBy(a=>a.Value.Item1).Where(a=>a.Value.Item1>=3);
            return listFileSortByFre;
        }
    }
}













