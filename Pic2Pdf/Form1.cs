using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace Pic2Pdf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select a picture";
            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            openFileDialog1.InitialDirectory = @"C:\Picture\";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Add(openFileDialog1.FileName.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var folderBrowserDiaglog = new FolderBrowserDialog();
            if (folderBrowserDiaglog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDiaglog.SelectedPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                listBox1.Items.RemoveAt(this.listBox1.SelectedIndex);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Generate document
            Document document = new Document();
            var imagePath = Path.Combine(textBox2.Text, "convertPdf.pdf");
            using (var stream = new FileStream(imagePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                PdfWriter.GetInstance(document, stream);

                document.Open();
                foreach (string itemInList in listBox1.Items)
                {
                    using (var imageStream = new FileStream(itemInList, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var image = Image.GetInstance(imageStream);
                        float maxWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                        float maxHeight = document.PageSize.Height - document.TopMargin - document.BottomMargin;
                        if (image.Height > maxHeight || image.Width > maxWidth)
                            image.ScaleToFit(maxWidth, maxHeight);
                        document.Add(image);
                    }
                }
                document.Close();
            }

            MessageBox.Show("Generate pdf success.");
            Application.Exit();
        }


    }
}
