using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace IF_IDF
{
    public class BoardBoolean
    {
        private List<List<string>> _boardBolean;
        private readonly List<document> _listpPages;
        private readonly List<string> _list = new List<string>();
        private readonly List<List<string>> _roateBoard;
        private readonly List<Tuple<string, string, int>> _boardFre;    
        //tên từ, Tuple<chỉ số tài liêu, tần số, idf.<List<tuple<tài liệu,tần số,tf,tf*idf>>>
        private readonly Dictionary<string, Tuple<int, int, double, List<Tuple<string, int,float,double>>>> _dictionList;
        public BoardBoolean(List<document> listpPages)
        {
            _listpPages = listpPages;
            _boardBolean = new List<List<string>>();
            _roateBoard = new List<List<string>>();
            Vobu = new List<Tuple<string, document, int>>();
            _boardFre = new List<Tuple<string, string, int>>();
            _dictionList = new Dictionary<string, Tuple<int, int, double, List<Tuple<string, int, float,double>>>>();
        }

        protected BoardBoolean()
        {
        }

        public Dictionary<string, Tuple<int, int, double, List<Tuple<string, int,float,double>>>> GetDictionList()
        {
            return _dictionList; ;
        }

        public IEnumerable<List<string>> GetBoard()
        {
            return _boardBolean;
        }

        public List<Tuple<string, document, int>> Vobu { get; private set; }

        public List<string> GetList()
        {
            return _list;
        }

        public void CreateBoardBoolean()
        {
            var listOfTitle = new List<string> { "" };
            var leng = _listpPages.Count();
            listOfTitle.AddRange(_listpPages.Select(variable => variable.Title));
            _boardBolean.Add(listOfTitle);
            for (var i = 0; i < leng; i++)
            {
                var lenListOfPage = _listpPages[i].ListWorld;
                foreach (var t in lenListOfPage)
                {
                    var lst = SetValueOfRow(t, i + 1, listOfTitle.Count);
                    _boardBolean.Add(lst);
                }
            }
            _boardBolean = new List<List<string>>(_boardBolean.OrderBy(l => l[0]));
            CompactBoard();
        }

        private static List<string> SetValueOfRow(string word, int index, int length)
        {
            var lst = new List<string> { word };
            for (var i = 1; i < length; i++)
            {
                lst.Add(i == index ? "1" : "0");
            }
            return lst;
        }

        public  void CompactBoard()
        {
            for (var i = 1; i < _boardBolean.Count; i++)// chueyn qua sai paralle, cho nhanh
            {
                for (var j = i + 1; j < _boardBolean.Count && _boardBolean[i][0] == _boardBolean[j][0]; j++)
                {
                    if (_boardBolean[i].SequenceEqual(_boardBolean[j]))
                    {
                        _boardBolean.Remove(_boardBolean[j]);
                        j--;
                    }
                    else
                    {
                        _boardBolean[i][_boardBolean[j].IndexOf("1")] = "1";
                        _boardBolean.Remove(_boardBolean[j]);
                        j--;
                    }
                }
            }
        }

        public  void CreateBoardVobu()
        {
            var vobu = (from variable in _listpPages from iteam in variable.ListWorld select new Tuple<string, document, int>(iteam, variable, 1)).ToList();
            Vobu = SortVoBu(vobu.ToList());
            CreateBoardVoBuByFrequency();
        }

        private List<Tuple<string, document, int>> SortVoBu(IEnumerable<Tuple<string, document, int>> lst)
        {
            return lst.OrderBy(q => q.Item1).ToList();
        }


        public  void CreateBoardVoBuByFrequency()
        {
            for (var i = 0; i < Vobu.Count - 1; i++)
            {
                for (var j = i + 1; j < Vobu.Count && Vobu[i].Item1 == Vobu[j].Item1 && Vobu[i].Item2 == Vobu[j].Item2; j++)
                {
                    Vobu[i] = new Tuple<string, document, int>(Vobu[i].Item1, Vobu[i].Item2, Vobu[i].Item3 + 1);
                    Vobu.RemoveAt(j);
                    j--;
                }
            }
        }
        public  void CreateBoardDictionary()
        {
            var tup = new Dictionary<string, Tuple<int, int, double, List<Tuple<string, int,float,double>>>>();
            for (var i = 0; i < Vobu.Count; i++)
            {
                var lst = new List<Tuple<string, int,float>>();
                lst = (from p in Vobu where p.Item1 == Vobu[i].Item1 select (new Tuple<string, int, float>(p.Item2.Title, p.Item3,float.Parse(p.Item3.ToString())/float.Parse(p.Item2.count.ToString()))))
                    .ToList();
                var count = lst.Sum(p => p.Item2);
                var list = new List<Tuple<string, int, float, double>>();
                // tinh idf = log_10(tong so page/ Tong page chua tu)
                var idf =1.0+ Math.Log((float)_listpPages.Count / (float)count);
                foreach (var item in lst)
                {
                    var value = new Tuple<string, int, float, double>(item.Item1,item.Item2,item.Item3,idf * item.Item3);
                    list.Add(value);
                }
                var tupsub = new Tuple<int, int,double, List<Tuple<string, int, float,double>>>(lst.Count, count,idf, list);
                _dictionList.Add(Vobu[i].Item1, tupsub);
                i = i + tupsub.Item4.Count - 1;
            }
        }
    }
}