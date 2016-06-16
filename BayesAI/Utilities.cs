using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BayesAI
{
    public class Utilities
    {
        public static List<AnalisisResult> readFolderFiles(string PathURL)
        {
            List<AnalisisResult> AnalisesList = new List<AnalisisResult>();
            string uploadsDirectory = System.Web.HttpContext.Current.Server.MapPath("/Doc_Uploads ");
            string[] txt = Directory.GetFiles(@PathURL, "*.txt", SearchOption.AllDirectories);
            StreamReader sr;
            foreach (string name in txt)
            {
                sr = new StreamReader(name);
                AnalisesList.Add(analiseTxt(sr.ReadToEnd()));
                sr.Close();
            }

            string[] doc = Directory.GetFiles(@PathURL, "*.doc", SearchOption.AllDirectories);
            foreach (string name in doc)
            {
                Document document = new Document();
                document.LoadFromFile(name);
                document.SaveToFile(uploadsDirectory + "\\" + "ToText.txt", FileFormat.Txt);

                sr = new StreamReader(uploadsDirectory + "\\" + "ToText.txt");
                AnalisesList.Add(analiseTxt(sr.ReadToEnd()));
                sr.Close();
            }

            string[] htmls = Directory.GetFiles(@PathURL, "*.html", SearchOption.AllDirectories);
            string withoutHtml = "";
            foreach(string name in htmls)
            {
                sr = new StreamReader(name);
                withoutHtml = HtmlRemoval.StripTagsRegexCompiled(sr.ReadToEnd());
                sr.Close();
                AnalisesList.Add(Utilities.analiseTxt(withoutHtml));

            }
            return AnalisesList;
        }
        public static AnalisisResult analiseTxt(String FileContent)
        {
            var con = ConfigurationManager.ConnectionStrings["BayesAIDBConnectionString"].ToString();
            AnalisisResult result = new AnalisisResult();
            string[] words = Regex.Split(FileContent, @"\W+"); // "@\W" split on every non-word char
            List<string> nonUsedWords = new List<string>();
            SqlConnection myConnection = new SqlConnection(con);
            string queryString = "Select * from languageWord where fk_word = @pWord";
            SqlCommand queryCmd = new SqlCommand(queryString, myConnection);
            SqlDataReader queryReader = null;
            queryCmd.Parameters.Add("@pWord", System.Data.SqlDbType.VarChar);
            bool wordUsed = false;
            myConnection.Open();

            //Languague Classifier
            foreach (string word in words)
            {
                queryCmd.Parameters["@pWord"].Value = word;
                queryReader = queryCmd.ExecuteReader();
                while (queryReader.Read())
                {
                    wordUsed = true;
                    if (result.LangsWords.Count == 0)
                    {
                        trickNode nTuple = new trickNode(queryReader["fk_lang"].ToString(), 1, 0.0);
                        result.LangsWords.Add(nTuple);
                    }
                    else
                    {
                        for (int i = 0; i < result.LangsWords.Count; i++)
                        {
                            trickNode currTuple = (trickNode)result.LangsWords[i];
                            if (currTuple.val == queryReader["fk_lang"].ToString())
                            {
                                result.LangsWords.ElementAt(i).cant++;
                                break;
                            }
                            else
                            {
                                if (i == result.LangsWords.Count - 1)
                                {
                                    trickNode nTuple = new trickNode(queryReader["fk_lang"].ToString(), 1, 0.0);
                                    result.LangsWords.Add(nTuple);
                                    break;
                                }
                            }
                        }
                    }
                }
                if (!wordUsed)
                {
                    nonUsedWords.Add(word);
                }
                wordUsed = false;
                queryReader.Close();
            }

            //Category Classifier
            queryString = "Select * from categoryWord where fk_word = @pWord";
            queryCmd.CommandText = queryString;

            foreach (string word in words)
            {
                queryCmd.Parameters["@pWord"].Value = word;
                queryReader = queryCmd.ExecuteReader();
                while (queryReader.Read())
                {
                    wordUsed = true;
                    if (result.categsWords.Count == 0)
                    {
                        trickNode nTuple = new trickNode(queryReader["fk_categ"].ToString(), 1, 0.0);
                        result.categsWords.Add(nTuple);
                    }
                    else
                    {
                        for (int i = 0; i < result.categsWords.Count; i++)
                        {
                            trickNode currTuple = (trickNode)result.categsWords[i];
                            if (currTuple.val == queryReader["fk_categ"].ToString())
                            {
                                result.categsWords.ElementAt(i).cant++;
                                break;
                            }
                            else
                            {
                                if (i == result.categsWords.Count - 1)
                                {
                                    trickNode nTuple = new trickNode(queryReader["fk_categ"].ToString(), 1, 0.0);
                                    result.categsWords.Add(nTuple);
                                    break;
                                }
                            }
                        }
                    }
                }
                if (!wordUsed)
                {
                    if (!nonUsedWords.Contains(word))
                    {
                        nonUsedWords.Add(word);
                    }
                }
                wordUsed = false;
                queryReader.Close();
            }

            //Language Analisis
            int totalLangWords = 0;
            foreach (trickNode node in result.LangsWords)
            {
                totalLangWords += node.cant;
            }

            string resLang = "";
            double currProb = 0.0;
            foreach (trickNode node in result.LangsWords)
            {
                node.prob = (double)node.cant / totalLangWords;
                if (currProb < node.prob)
                {
                    currProb = node.prob;
                    resLang = node.val;
                }
            }
            result.langResult = resLang;

            //Bayesian Category Analisis
            //Previous Probability
            List<int> dbProbs = new List<int>();
            queryString = "select count(*) cnt from (select fk_word w from categoryWord where fk_categ = @pWord) as alias";
            queryCmd.CommandText = queryString;
            foreach (trickNode node in result.categsWords)
            {
                queryCmd.Parameters["@pWord"].Value = node.val;
                queryReader = queryCmd.ExecuteReader();
                queryReader.Read();
                dbProbs.Add((int)queryReader["cnt"]);
                queryReader.Close();
            }

            //Calculate Posteriori Probability
            int count = 0;
            int totalPreviousWords = dbProbs.Sum();
            string resCateg = "";
            currProb = 0.0;
            foreach (trickNode node in result.categsWords)
            {
                node.prob = ((double)dbProbs[count] / totalPreviousWords) * ((double)node.cant / dbProbs[count]);
                if (currProb < node.prob)
                {
                    currProb = node.prob;
                    resCateg = node.val;
                }
                count++;
            }
            result.categResult = resCateg;

            //Send Learning Words
            insertLearningWords(nonUsedWords, result.categResult, result.langResult);
            myConnection.Close();
            return result;
        }
        public static void insertLearningWords(List<string> wordsList, string categ, string lang)
        {
            var con = ConfigurationManager.ConnectionStrings["BayesAIDBConnectionString"].ToString();
            SqlConnection myConnection = new SqlConnection(con);
            string queryStringLang = "Exec processLearningLangWords @pWord, @plang";
            string queryStringCateg = "Exec processLearningCategWords @pWord, @pCateg";
            SqlCommand queryCmdLang = new SqlCommand(queryStringLang, myConnection);
            SqlCommand queryCmdCateg = new SqlCommand(queryStringCateg, myConnection);
            queryCmdLang.Parameters.Add("@pWord", System.Data.SqlDbType.VarChar);
            queryCmdLang.Parameters.Add("@plang", System.Data.SqlDbType.VarChar);
            queryCmdCateg.Parameters.Add("@pWord", System.Data.SqlDbType.VarChar);
            queryCmdCateg.Parameters.Add("@pCateg", System.Data.SqlDbType.VarChar);
            myConnection.Open();

            foreach (string word in wordsList)
            {
                if (!word.Any(char.IsDigit) && word.Length > 1)
                {
                    queryCmdLang.Parameters["@pWord"].Value = word;
                    queryCmdLang.Parameters["@plang"].Value = lang;
                    queryCmdLang.ExecuteNonQuery();
                    if (word.Length > 3)
                    {
                        queryCmdCateg.Parameters["@pWord"].Value = word;
                        queryCmdCateg.Parameters["@pCateg"].Value = categ;
                        queryCmdCateg.ExecuteNonQuery();
                    }
                }
            }
            myConnection.Close();
        }
        public static void learn()
        {
            var con = ConfigurationManager.ConnectionStrings["BayesAIDBConnectionString"].ToString();
            SqlConnection myConnection = new SqlConnection(con);
            string queryStringLang = "Exec LearnLangWords";
            string queryStringCateg = "Exec LearnCategWords";
            SqlCommand queryCmdLang = new SqlCommand(queryStringLang, myConnection);
            SqlCommand queryCmdCateg = new SqlCommand(queryStringCateg, myConnection);
            myConnection.Open();
            queryCmdCateg.ExecuteNonQuery();
            queryCmdLang.ExecuteNonQuery();
        }
    }
}