using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMergerEntity
{
    public enum VersionControlTools
    {
        SVN = 1,
        GIT = 2,
        TFS = 3
    }
    public class ConfigEntity
    {
        /// <summary>
        /// which configuration tool is being used SVN, GIT, TFS
        /// </summary>
        public int ConfigTool { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        /// <summary>
        /// Do not merge check in for this author.
        /// if author is mentioned then revisions checked in by author in source will be merged into target.
        /// generally, Developer check in the code from branch to trunk, so code is always available in branch, so no point in getting same code from trunk to branch and increase revisions in branch
        /// if multiple developers are check in the code from branch to trunk then this needs to be kept blank/null/""/string.empty
        /// </summary>
        public string author { get; set; }
        public string sourceUrl { get; set; }
        public string targetUrl { get; set; }
        public string checkoutPath { get; set; }
        public string logFilePath { get; set; }
    }
}
