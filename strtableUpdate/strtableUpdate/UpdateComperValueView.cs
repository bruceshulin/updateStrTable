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
    public partial class UpdateComperValueView : UserControl
    {
        public UpdateComperValueView()
        {
            InitializeComponent();
        }
        public UpdateComperValueView(string sheetname,string id,string contry,string strtable1value,string strtable2value)
        {
            InitializeComponent();
            this.txtSheetName.Text = sheetname;
            this.txtID.Text = id;
            this.txtContry.Text = contry;
            this.txtTab1Value.Text = strtable1value;
            this.txtTab2Value.Text = strtable2value;
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
