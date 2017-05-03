﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IF_IDF
{
    public partial class Form1 : Form
    {
        public DataTable DTable;
        public List<document> ListDocuments = new List<document>();
        public Stopword Stopword;
        public Form1()
        {
            InitializeComponent();
            DTable = new DataTable();
            openFileDialog1.Multiselect = true;
            dataGridView1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var dt = new DataTable();
            dt.Columns.Add("Title");
            dt.Columns.Add("Content");
            var dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                foreach (var file in openFileDialog1.FileNames)
                {
                    string content;
                    using (var reader = new StreamReader(file, Encoding.UTF8))
                    {
                        content = reader.ReadToEnd();
                    }
                    switch (_sword)
                    {
                        case false:
                        {
                            var document = new document(file, content);
                            ListDocuments.Add(document);
                            dt.Rows.Add(document.Title, document.Content);
                        }
                            break;
                        case true:
                        {
                            var document = new document(file, content,Stopword);
                            ListDocuments.Add(document);
                            dt.Rows.Add(document.Title, document.Content);
                        }
                            break;
                    }

                }
            }

            dt.AcceptChanges();
            dataGridView1.DataSource = dt;
            DTable = dt;
        }

        public BoardBoolean bbl;
        private void button2_Click(object sender, EventArgs e)
        {
            bbl = new BoardBoolean(ListDocuments);
            bbl.CreateBoardVobu();
            var lstBoard = bbl.Vobu;
            foreach (var tuple in lstBoard)
            {
                Console.WriteLine(tuple.Item1 + @"	" + tuple.Item2 + @"	" + tuple.Item3);
            }


            bbl.CreateBoardDictionary();
            foreach (var tuple in bbl.GetDictionList())
            {
                Console.WriteLine(tuple.Key + @"	" + tuple.Value.Item1 + @"	" + tuple.Value.Item2);
                foreach (var variable in tuple.Value.Item4)
                {
                    Console.WriteLine(@"	" + variable.Item1 + @"	" + variable.Item2);
                }
            }
        }


        //Gom từ khóa có cùng chỉ số tài liệu và thêm tần số
        private void btnIndexing_Click(object sender, EventArgs e)
        {
            bbl = new BoardBoolean(ListDocuments);
            bbl.CreateBoardVobu();
            var lstBoard = bbl.Vobu;
            var dt = new DataTable();
            dt.Columns.Add("Từ Khóa");
            dt.Columns.Add("Chỉ số tài liệu");
            dt.Columns.Add("Tần số");
            foreach (var doc in lstBoard)
                dt.Rows.Add(doc.Item1, doc.Item2.Title, doc.Item3);
            var index = new Indexing(dt);
            index.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            bbl = new BoardBoolean(ListDocuments);
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
                    dt.Rows.Add("", variable.Item1, variable.Item2,variable.Item3.ToString(CultureInfo.InvariantCulture),"",variable.Item4.ToString(CultureInfo.InvariantCulture));
                }
            }
            var dicAndPosting = new DicAndPosting(dt);
            dicAndPosting.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var evlt= new EvaluateQuery();
            evlt.Show();
        }

        private bool _sword;
        private void button8_Click(object sender, EventArgs e)
        {
            var dr = openFileDialog2.ShowDialog();
            if (dr != DialogResult.OK) return;
            foreach (var unused in openFileDialog1.FileNames)
            {
                string content;
                using (var reader = new StreamReader(openFileDialog2.FileName, Encoding.UTF8))
                {
                    content = reader.ReadToEnd();
                }
                Stopword = new Stopword(content);
                if (content!="")
                {
                    lbTxt.Text = @"đã có stopword";
                    _sword = true;
                }
                else
                {
                    MessageBox.Show(@"File rổng chọn lại", @"Select File Stopword", MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    _sword = false;
                    lbTxt.Text = @"chưa có stopword";

                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            var query = new Query(txtSearch.Text, Stopword);
            var searchQuery = new SearchQuery(query,bbl);
            var result = searchQuery.FileResult();
            var dt = new DataTable();
            dt.Columns.Add("Query ID");
            dt.Columns.Add("File Result");
            dt.Columns.Add("World");
            dt.Columns.Add("Frequency");
            foreach (var item in result)
            {
                dt.Rows.Add(txtSearch.Text, item.Key, item.Value.Item1, item.Value.Item2);
            }
            dataGridView2.DataSource = dt;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            var dt = new DataTable();
            dt.Columns.Add("ID Query");
            dt.Columns.Add("File Result");
            var dr = this.openFileDialog3.ShowDialog();
            if (dr == DialogResult.OK)
            {
                foreach (var file in openFileDialog3.FileNames)
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
                        var query = new Query(varString.ToString(), Stopword,1);
                        var search = new SearchQuery(query,bbl);
                        var result = search.FileResult();
                        foreach (var item in result)
                        {
                            dt.Rows.Add(query.IdQuery, item.Key);
                        }
                    }
                }
            }
            dataGridView2.DataSource = dt;
        }

        private void button7_Click(object sender, EventArgs e)
        {

            var bs = new BindingSource {DataSource = dataGridView2.DataSource};
            var dt = (DataTable)(bs.DataSource);
            var text = "";


            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FilterIndex = 2;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    text = dt.Rows.Cast<DataRow>().Aggregate(text, (current, row) => current + row[0].ToString() + "\t" + row[1].ToString() + "\n");
                    File.WriteAllText(sfd.FileName, text);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var bs = new BindingSource { DataSource = dataGridView2.DataSource };
            var dt = (DataTable)(bs.DataSource);
            var evlt = new EvaluateQuery(dt);
            evlt.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var lucen = new Lucene();
            lucen.Show();
        }
    }
}

