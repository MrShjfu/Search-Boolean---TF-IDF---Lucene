using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IF_IDF
{
    public partial class EvaluateQuery : Form
    {
        public EvaluateQuery()
        {
            InitializeComponent();
            openFileDialog1.Multiselect = true;
            ResulTable = new DataTable();
            lstFileResult = new List<Tuple<string, string>>();
        }

        public EvaluateQuery(DataTable dt)
        {
            InitializeComponent();
            openFileDialog1.Multiselect = true;
            ResulTable = new DataTable();
            dataGridView2.DataSource = dt;
            ResulTable = dt;
            lstFileResult = new List<Tuple<string, string>>();
            foreach (DataRow row in dt.Rows)
            {
                lstFileSearch.Add(new Tuple<string, string>(row[0].ToString(), row[1].ToString()));
            }
        }

        public DataTable ResulTable;
        public List<Tuple<string, string>> lstFileResult; //Tai lieu ma may trả ve
        List<Tuple<string, string>> lstFileSearch = new List<Tuple<string, string>>();// tai lieu minh tra ve
        // query ,tai lieu giong, tong so may tra ve, minh tra ve
        List<Tuple<string,int,int,int>> lstEvaluate = new List<Tuple<string,int, int, int>>();
        List<Tuple<string, int, int, int,double,double>> listTuple = new List<Tuple<string, int, int, int,double,double>>();

        public List<string> ListFile;
        private void button1_Click(object sender, EventArgs e)
        {
            var dt = new DataTable();
            dt.Columns.Add("Query ID");
            dt.Columns.Add("File Name");
            dt.Columns.Add("Rank");
            var dr = this.openFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {

                foreach (var file in openFileDialog1.FileNames)
                {
                    var content = "";
                    using (var reader = new StreamReader(file, Encoding.UTF8))
                    {
                        content = reader.ReadToEnd();
                    }
                    var line = content.Split(new string[] {"\r\n", "\n"}, StringSplitOptions.None);
                    foreach (var varString in line)
                    {
                        if (varString == "") continue;
                        var query = new Query(varString.ToString(), 1);
                        if (query.Rank.Trim() == "-1") continue;
                        dt.Rows.Add(query.IdQuery, query.Title, query.Rank);
                        lstFileResult.Add(new Tuple<string, string>(query.IdQuery, query.Title));
                    }
                }
            }
            dt.AcceptChanges();
            dataGridView1.DataSource = dt;
            ResulTable = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dt = new DataTable();
            dt.Columns.Add("ID Query");
            dt.Columns.Add("NameFile");
            lstFileSearch = new List<Tuple<string, string>>();

            var dr = this.openFileDialog2.ShowDialog();
            if (dr == DialogResult.OK)
            {
                foreach (var file in openFileDialog2.FileNames)
                {
                    var content = "";
                    using (var reader = new StreamReader(file, Encoding.UTF8))
                    {
                        content = reader.ReadToEnd();
                    }
                    var line = content.Split(new string[] {"\r\n", "\r"}, StringSplitOptions.None);
                    foreach (var varString in line)
                    {
                        if (varString == "") break;
                        var query = new Query(varString);
                        dt.Rows.Add(query.IdQuery, query.Title);
                        lstFileSearch.Add(new Tuple<string, string>(query.IdQuery, query.Title));
                    }
                }
            }
            dataGridView2.DataSource = dt;
        }

        public void asd()
        {
            List<string> listItem  = new List<string>();
            foreach (var row in lstFileResult)
            {
                var has = listItem.Any(x => x == row.Item1);
                if (has == false)
                    listItem.Add(row.Item1);
            }

            foreach (var item in listItem)
            {
                var fileSystem = lstFileResult.Where(y => y.Item1 == item).ToList();
                var fileResult = lstFileSearch.Where(y => y.Item1 == item).ToList();
                var a = fileSystem.Intersect(fileResult).Count(); // so tai lieu giong nhau cho moi query
                Tuple < string,int,int,int> tup = new Tuple<string, int, int, int>(item,a,fileSystem.Count,fileResult.Count);
                lstEvaluate.Add(tup);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> listItem = new List<string>();
            foreach (var row in lstFileResult)
            {
                var has = listItem.Any(x => x == row.Item1);
                if (has == false)
                    listItem.Add(row.Item1);
            }

            foreach (var item in listItem)
            {
                var FileSystem = lstFileResult.Where(y => y.Item1 == item).ToList();
                var FileResult = lstFileSearch.Where(y => y.Item1 == item).ToList();
                var a = FileSystem.Intersect(FileResult).Count(); // so tai lieu giong nhau cho moi query
                Tuple<string, int, int, int> tup = new Tuple<string, int, int, int>(item, a, FileSystem.Count, FileResult.Count);
                lstEvaluate.Add(tup);
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("Query");
            dt.Columns.Add("Tai Lieu Giong");
            dt.Columns.Add("number of Res");
            dt.Columns.Add("number system");
            dt.Columns.Add("Recall");
            dt.Columns.Add("Pre");


            foreach (var item in lstEvaluate)
            {
                listTuple.Add(new Tuple<string, int, int, int, double, double>(item.Item1,item.Item2,item.Item3,item.Item4,(double)item.Item2/item.Item3,(double)item.Item2/item.Item4));
                dt.Rows.Add(item.Item1, item.Item2, item.Item3, item.Item4, (double)item.Item2 / item.Item3, (double)item.Item2 / item.Item4);
            }
            dataGridView3.DataSource = dt;

            double recall = listTuple.Sum(a => a.Item5 / listTuple.Count);
            double pre = listTuple.Sum(a => a.Item6 / listTuple.Count);

            txt1.Text = @"Recall trung bình: " + recall.ToString();
            txt2.Text = @"Pre trung bình: " + pre.ToString();

        }
    }
}
