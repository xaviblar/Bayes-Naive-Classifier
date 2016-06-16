using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Text.RegularExpressions;
using Spire.Doc;

namespace BayesAI
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            URLtxtBox.Attributes["onclick"] = "this.value=''";
        }

        protected void FacebookFileBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("AnalisisResult.aspx");
        }

        protected void analiseBtn_Click(object sender, EventArgs e)
        {
            if(FileUpload1.HasFile)
            {
                Utilities.learn();
                StreamReader sr;
                string ext = System.IO.Path.GetExtension(this.FileUpload1.PostedFile.FileName);
                if(ext == ".txt")
                {
                    sr = new StreamReader(FileUpload1.PostedFile.InputStream);
                    AnalisisResult res = Utilities.analiseTxt(sr.ReadToEnd());
                    sr.Close();
                }
                else if(ext == ".doc" || ext==".docx")
                {
                    string uploadsDirectory = System.Web.HttpContext.Current.Server.MapPath("/Doc_Uploads ");
                    string docPath = System.Web.HttpContext.Current.Server.MapPath(FileUpload1.PostedFile.FileName);
                    FileUpload1.PostedFile.SaveAs(docPath);
                    Document document = new Document();
                    document.LoadFromFile(docPath);
                    document.SaveToFile(uploadsDirectory + "\\" + "ToText.txt", FileFormat.Txt);

                    sr = new StreamReader(uploadsDirectory + "\\" + "ToText.txt");
                    AnalisisResult res= Utilities.analiseTxt(sr.ReadToEnd());
                    sr.Close();
                }

                else if(ext == ".html")
                {
                    sr = new StreamReader(FileUpload1.PostedFile.InputStream);
                    string withoutHtml = HtmlRemoval.StripTagsRegexCompiled(sr.ReadToEnd());
                    sr.Close();
                    AnalisisResult res = Utilities.analiseTxt(withoutHtml);
                }
            }
        }

        protected void GoURLBtn_Click(object sender, EventArgs e)
        {
            Utilities.learn();
            List<AnalisisResult> list =  Utilities.readFolderFiles(URLtxtBox.Text);
        }
    }
}