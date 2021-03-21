using System;
using System.Collections.Generic;
using System.Text;

namespace LineageServer.Model
{
    public class StringTokenizer
    {
        readonly string[] tokens;

        int currentIndex;

        public StringTokenizer(string str)
        {
            try
            {
                this.tokens = str.Split(new string[] { "\t", "\n", "\r", "\f" }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string nextToken()
        {
            if (hasMoreToken())
            {
                return this.tokens[this.currentIndex++];
            }
            else
            {
                return string.Empty;
            }
        }

        public bool hasMoreTokens()
        {
            return this.tokens != null &&
                (this.currentIndex < this.tokens.Length);
        }
    }
}
