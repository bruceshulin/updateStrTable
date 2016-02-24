using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace strtableUpdate
{
    public partial class UpdateIDView : UserControl
    {
        public UpdateIDView()
        {
            InitializeComponent();
        }
        public UpdateIDView(string sheetname,string id)
        {
            InitializeComponent();
            this.txtSheetName.Text = sheetname;
            this.txtID.Text = id;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.BackColor == Color.Red)
            {
                this.BackColor = Color.White;
            }
            else
            {
                this.BackColor = Color.Red;
            }
        }
    }
}
