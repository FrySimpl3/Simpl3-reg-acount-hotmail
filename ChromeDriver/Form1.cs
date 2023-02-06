using OpenQA.Selenium.DevTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChromeDriver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String lic =System.Environment.CurrentDirectory+ "\\ChromeDrivers\\LICENSE.chromedriver";
            if (System.IO.File.Exists(lic))
            {
                System.IO.File.Delete(lic);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Behind._str();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UndetectedChromeDriverCustom.UndetectedChromeDriver.KillAllChromeProcesses();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UndetectedChromeDriverCustom.UndetectedChromeDriver.soluong = (int)numericUpDown1.Value;
        }
    }
}
