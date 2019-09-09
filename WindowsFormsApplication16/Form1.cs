using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hooks;

namespace WindowsFormsApplication16
{
    public partial class Form1 : Form
    {
        Hook hHook = new Hook();
        public Form1()
        {
            InitializeComponent();
            hHook.GetKeysCode = new GetKeysCodeEventHandler(getKeysCodes);
        }

        private void getKeysCodes(int code)
        {
            if (listBox1.Items.Count >= 20)
            {
                listBox1.Items.Clear();
            }
            listBox1.Items.Add((Keys)code);
        }

        private void button1_Click(object sender, EventArgs e)
        {
      
            hHook.Hook_Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hHook.Hook_Clear();
        }
    }
}
