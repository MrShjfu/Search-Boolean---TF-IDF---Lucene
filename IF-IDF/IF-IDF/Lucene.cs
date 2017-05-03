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
    public partial class Lucene : Form
    {
        public List<string> DQuery = new List<string>();
        public DataTable DTable = new DataTable();
        public Lucene()
        {
            InitializeComponent();
            this.openFileDialog1.Filter = @"All files(*.*) | *.*";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = @"My Text Browser";
            Search.Enabled = false;
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
                    var document = new document(content.ToString(), file.ToString(),"");
                    dt.Rows.Add(document.Title, document.Content);
                }
            }

            dt.AcceptChanges();
            dataGridView1.DataSource = dt;
            DTable = dt;
            if (dt.Rows.Count != 0)
            {
                Search.Enabled = true;
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private List<Tuple<string, string>> lstFindSearch;
        private void button3_Click(object sender, EventArgs e)
        {
            var crt = new IndexByLucene(DTable);
            crt.InitIndex(DTable);
            var dt = new DataTable();
            dt.Columns.Add("ID Query");
            dt.Columns.Add("NameFile");
            dt.Columns.Add("Score");
            var i = 1;
            lstFindSearch = new List<Tuple<string, string>>();
            foreach (var query in DQuery)
            {
                var db = crt.Search(query);
                foreach (DataRow row in db.Rows)
                {
                    dt.Rows.Add(i.ToString(), row[0], row[1]);
                    lstFindSearch.Add(new Tuple<string, string>(i.ToString(), row[0].ToString()));
                }
                i++;
            }
            var rstForm = new FormLucene(dt, lstFindSearch);
            rstForm.Show();

            //var crt = new CreateIndex(dTable);
            //crt.InitIndex(dTable);
            //var rstForm = new ResultForm(dt: crt.Search(txtQuery.Text, 1));
            //rstForm.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var dt = new DataTable();
            dt.Columns.Add("ID Query");
            dt.Columns.Add("Query Content");
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
                    var line = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    foreach (var varString in line)
                    {
                        if (varString == "") break;
                        var query = new Query(varString.ToString(), "");
                        dt.Rows.Add(query.Title, query.Content);
                        DQuery.Add(query.Content);
                    }
                }
            }

            dt.AcceptChanges();
            dgv2.DataSource = dt;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            var crt = new IndexByLucene(DTable);
            crt.InitIndex(DTable);
            var dt = new DataTable();
            dt.Columns.Add("ID Query");
            dt.Columns.Add("NameFile");
            dt.Columns.Add("Score");
            var i = 1;
            lstFindSearch = new List<Tuple<string, string>>();
            foreach (var query in DQuery)
            {
                var db = crt.Search(query);
                foreach (DataRow row in db.Rows)
                {
                    dt.Rows.Add(i.ToString(), row[0], row[1]);
                    lstFindSearch.Add(new Tuple<string, string>(i.ToString(), row[0].ToString()));
                }
                i++;
            }
            var rstForm = new FormLucene(dt, lstFindSearch);
            rstForm.Show();
        }

        private void Search_Click_1(object sender, EventArgs e)
        {
            var crt = new IndexByLucene(DTable);
            crt.InitIndex(DTable);
            var rstForm = new FormLucene(crt.Search(txtQuery.Text.ToString()), lstFindSearch);
            rstForm.Show();
        }
    }
}
