using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IF_IDF
{
    public partial class FormLucene : Form
    {
        public DataTable ResulTable;
        public DataTable getFileTable;
        public FormLucene(DataTable dt, List<Tuple<string, string>> lst)
        {

            InitializeComponent();
            rstData.DataSource = dt;
            ResulTable = new DataTable();
            getFileTable = new DataTable();
            lstFileResult = new List<Tuple<string, string>>();
            listSearch = new List<Tuple<string, string>>();
            listSearch = lst;
            getFileTable = dt;
            openFileDialog1.Multiselect = true;
        }

        private void btnResultFile_Click(object sender, EventArgs e)
        {
           
        }

        public List<Tuple<string, string>> lstFileResult; //Tai lieu ma may trả ve

        public List<Tuple<string, string>> listSearch;
        public List<Tuple<string, int, int>> lstcount; //ten tai lieu, giong, tren tong so tai lieu ta tinh duoc
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnResultFile_Click_1(object sender, EventArgs e)
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
                    var line = content.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                    foreach (var varString in line)
                    {
                        if (varString == "") continue;
                        var document = new Query(varString.ToString());
                        if (document.Rank.Trim() == "-1") continue;
                        dt.Rows.Add(document.Id, document.Title, document.Rank);
                        lstFileResult.Add(new Tuple<string, string>(document.Id, document.Title));
                    }
                }
            }
            dt.AcceptChanges();
            dataGridView1.DataSource = dt;
            ResulTable = dt;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var lstItem = new List<string>(); // danh sach cac tai lieu 
            var lstPre = new List<float>();
            foreach (var row in listSearch)
            {
                var has = lstItem.Any(x => x == row.Item1);
                if (has == false)
                    lstItem.Add(row.Item1);
            }

            //danh sach Tong tat ca tai lieu minh tra ve 
            var lstCount = lstItem.Select(item => listSearch.Where(x => x.Item1 == item).ToList().Count).ToList();

            // tim tong so tai lieu giong
            var lstItemSample = lstItem.Select(item => lstFileResult.Where(y => listSearch.Any(z => z.Item1 == y.Item1 && z.Item2 == y.Item2 && z.Item1 == item))
                    .ToList()
                    .Count)
                .ToList();
            for (var index = 0; index < lstItem.Count; index++)
            {
                lstPre.Add((float)lstItemSample[index] / lstCount[index]);
            }

            var b = lstPre.Sum(x => Convert.ToSingle(x));
            txt.Text = "" + ((float)b / lstPre.Count).ToString(CultureInfo.InvariantCulture);
        }
    }
}
