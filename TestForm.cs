using HtmlAgilityPack;
using System;
using System.Collections;
using System.Windows.Forms;

namespace Extractor
{
    public partial class TestForm : Form
    {
        private Button button1;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBox2;
        private TextBox textBox3;
        private Label label4;
        private Label label5;
        private TextBox textBoxH1;
        private TextBox textBox1;
    
        public TestForm()
        {
            InitializeComponent();
        }  

        private void button1_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            var info = WorkerClass.getInfo(url); //WorkerClass.getSourceCode(url);


            this.textBox2.Text = info.Title;
            this.textBox3.Text = info.Description;
            
            this.textBoxH1.Text = showAllListValues(info.tagH1);
        }

        public string showAllListValues(ArrayList list) 
        { 
            string listRetuns = "";
            foreach (string value in list)
                listRetuns = listRetuns + " ; " + value.ToString();

            return listRetuns;
        }

        private void scan_Click(object sender, EventArgs e)
        {
            Uri uri = new Uri(textBox1.Text);

            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument document = htmlWeb.Load(uri.AbsoluteUri);


            this.txtResultado.Text = WorkerClass.getTags(document, "//meta");
            this.txtResultado.Text += WorkerClass.getTags(document, "//h1");
            this.txtResultado.Text += WorkerClass.getTags(document, "//h2");
            this.txtResultado.Text += WorkerClass.getTags(document, "//h3");
            this.txtResultado.Text += WorkerClass.getTags(document, "//title");
            this.txtResultado.Text += WorkerClass.getTags(document, "//a");
        }
    }
}
