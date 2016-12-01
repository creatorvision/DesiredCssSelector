using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesiredCssSelector
{
    public partial class Form1 : Form
    {
        HashSet<string> desiredElements = new HashSet<string>();
        string maincssContent;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            desiredElements.Add("jumbotron");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            desiredElements.Add("pre");
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            desiredElements.Add("img");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText("C:\\Program Files\\Code_Learn\\baseTemplate\\Finaltemplate.css", String.Empty);
            processFormRequest pfr = new processFormRequest();
            maincssContent = pfr.addBaseCss() + System.Environment.NewLine + System.Environment.NewLine + System.Environment.NewLine; // Adding Base CSS 
            if (desiredElements.Count!=0)
            {
                for (int i = 0; i < desiredElements.Count; i++)
                {
                    maincssContent = maincssContent + pfr.fetchElementContent(desiredElements.ElementAt(i), ref desiredElements) + System.Environment.NewLine + System.Environment.NewLine + System.Environment.NewLine; // Adding css of the desired element
                    System.IO.File.WriteAllText("C:\\Program Files\\Code_Learn\\baseTemplate\\Finaltemplate.css", maincssContent);
                }
                desiredElements.Clear();
            }
            else
            {
                System.IO.File.WriteAllText("C:\\Program Files\\Code_Learn\\baseTemplate\\Finaltemplate.css", maincssContent); // when nothing is selected and only base css is needed to be loaded 
            }
            desiredElements.Clear();
        }
    }

    public class processFormRequest
    {
        public string addBaseCss()
        {
            string basecss="";
            try
            {
                using (StreamReader sr = new StreamReader("C:\\Program Files\\Code_Learn\\baseTemplate\\base.css"))
                {
                    basecss= sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {

                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return basecss;
        }
        public string fetchElementContent(string inpElement, ref HashSet<string> desiredElements)
        {
            string elementContent = "";
            try
            {
                using (StreamReader sr  = new StreamReader("C:\\Program Files\\Code_Learn\\baseTemplate\\template.css"))
                {
                    string line;
                    while ((line = sr.ReadLine().TrimEnd('\r', '\n',' ')) != null)
                    {
                        if (line.Equals("/*")) // Start of finding element
                        {
                            string searchelement = sr.ReadLine().Replace("<", "").Replace(">", "").TrimEnd('\r', '\n', ' ');
                            if (searchelement == inpElement)
                            {
                                string nextLine;
                                while ((nextLine = sr.ReadLine().TrimEnd('\r', '\n', ' ')) != null && !nextLine.Equals("*/"))
                                {
                                    if (nextLine.StartsWith("+"))
                                    {
                                        desiredElements.Add(nextLine.Replace("<", "").Replace(">", "").Replace("+", "").TrimEnd('\r', '\n', ' '));
                                    }
                                }
                                while ((nextLine = sr.ReadLine().TrimEnd('\r', '\n', ' ')) != null && !nextLine.Equals("/*<end " + inpElement + ">*/"))
                                {
                                    elementContent = elementContent + nextLine;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return elementContent;
        }
    }
}
