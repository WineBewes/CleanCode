using System;
using System.Windows.Forms;
using CleanCode.Business;

namespace CleanCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToggleVisibility(sender);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ToggleVisibility(sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ToggleVisibility(sender);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ToggleVisibility(sender);
        }
        
        private void button5_Click(object sender, EventArgs e)
        {
            var helloWorldInserter = new HelloWorldInserter(new UserNameGetter());

            MessageBox.Show(helloWorldInserter.InsertHelloWorld());
        }

        private void ToggleVisibility(object sender)
        {
            var button = (Button) sender;

            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            panel1.Visible = false;
            button5.Visible = false;

            if (button.Name == "button1")
            {
                button2.Visible = true;
            }
            if (button.Name == "button2")
            {
                button3.Visible = true;
            }
            if (button.Name == "button3")
            {
                panel1.Visible = true;
            }
            if (button.Name == "button4")
            {
                button5.Visible = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;

            button4.Enabled = !(string.IsNullOrEmpty(textBox.Text.Trim()));
        }
    }
}
