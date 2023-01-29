using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zachet
{
    public partial class connecting : Form
    {
        public string Login
        {
            get { return textBox1.Text; }

        }
        public string Password
        {
            get { return textBox2.Text; }
        }

        public connecting()
        {
            InitializeComponent();
        }
    }
}
