using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.String;

namespace IF_IDF
{
    public partial class Form1 : Form
    {
        public DataTable DTable = new DataTable();
        public List<document> ListDocuments = new List<document>();
        public Form1()
        {
            InitializeComponent();
            DTable = new DataTable();
            openFileDialog1.Multiselect = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var dt = new DataTable();
            dt.Columns.Add("Title");
            dt.Columns.Add("Content");
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
                    var document = new document(file.ToString(), content.ToString());
                    ListDocuments.Add(document);
                    dt.Rows.Add(document.Title, document.Content);
                }
            }

            dt.AcceptChanges();
            dataGridView1.DataSource = dt;
            DTable = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var bbl = new BoardBoolean(ListDocuments);
            bbl.CreateBoardVobu();
            var lstBoard = bbl.Vobu;
            foreach (var tuple in lstBoard)
            {
                Console.WriteLine(tuple.Item1 + "\t" + tuple.Item2 + "\t" + tuple.Item3);
            }

            Console.WriteLine("-------------------------------------------------------");

            bbl.CreateBoardDictionary();
            foreach (var tuple in bbl.GetDictionList())
            {
                Console.WriteLine(tuple.Key + "\t" + tuple.Value.Item1 + "\t" + tuple.Value.Item2);
                foreach (var variable in tuple.Value.Item4)
                {
                    Console.WriteLine("\t" + variable.Item1 + "\t" + variable.Item2);
                }
            }
        }


        //Gom từ khóa có cùng chỉ số tài liệu và thêm tần số
        private void btnIndexing_Click(object sender, EventArgs e)
        {
            var bbl = new BoardBoolean(ListDocuments);
            bbl.CreateBoardVobu();
            var lstBoard = bbl.Vobu;
            var dt = new DataTable();
            dt.Columns.Add("Từ Khóa");
            dt.Columns.Add("Chỉ số tài liệu");
            dt.Columns.Add("Tần số");
            foreach (var doc in lstBoard)
                dt.Rows.Add(doc.Item1, doc.Item2, doc.Item3);
            var index = new Indexing(dt);
            index.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            var bbl = new BoardBoolean(ListDocuments);
            bbl.CreateBoardVobu();
            bbl.CreateBoardDictionary();

            var dt = new DataTable();
            dt.Columns.Add("Từ Khóa");
            dt.Columns.Add("Chỉ số tài liệu");
            dt.Columns.Add("Tần số");
            dt.Columns.Add("TF");
            dt.Columns.Add("IDF");
            dt.Columns.Add("TF * IDF");
            foreach (var tuple in bbl.GetDictionList())
            {
                dt.Rows.Add(tuple.Key, tuple.Value.Item1, tuple.Value.Item2,"",tuple.Value.Item3.ToString(CultureInfo.InvariantCulture));
                foreach (var variable in tuple.Value.Item4)
                {
                    dt.Rows.Add("", variable.Item1, variable.Item2,"",variable.Item3.ToString(CultureInfo.InvariantCulture),variable.Item4.ToString(CultureInfo.InvariantCulture));
                }
            }
            var dicAndPosting = new DicAndPosting(dt);
            dicAndPosting.Show();
        }
    }
}
