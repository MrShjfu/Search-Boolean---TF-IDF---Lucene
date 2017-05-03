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
    public partial class DicAndPosting : Form
    {
        public DicAndPosting(DataTable dt)
        {
            InitializeComponent();
            dataGridView1.DataSource = dt;
        }
    }
}
