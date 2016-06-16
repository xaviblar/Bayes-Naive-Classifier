using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BayesAI
{
    public class AnalisisResult
    {
        public string langResult;
        public string categResult;
        public List<trickNode> categsWords;
        public List<trickNode> LangsWords;

        public AnalisisResult()
        {
            this.categsWords = new List<trickNode>();
            this.LangsWords = new List<trickNode>();
            this.langResult = "";
            this.categResult = "";
        }
    }
}