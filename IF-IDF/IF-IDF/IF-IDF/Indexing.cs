using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IF_IDF
{
    public partial class Indexing : Form
    {
        public Indexing(DataTable dt)
        {
            InitializeComponent();
            dataGridView1.DataSource = dt;

        }
    }
}
