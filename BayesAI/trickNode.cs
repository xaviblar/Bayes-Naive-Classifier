using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BayesAI
{
    public class trickNode
    {
        public string val;
        public int cant;
        public double prob;

        public trickNode(string valp, int cantp, double probp)
        {
            this.cant = cantp;
            this.val = valp;
            this.prob = probp;
        }
    }
}