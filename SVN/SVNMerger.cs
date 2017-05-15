using System.Collections.Generic;

using SharpSvn;
using System.Net;
using System;
using CommonLibrary;
using CodeMergerEntity;
using System.IO;

namespace SVN
{
    public class SVNMerger : IMerger
    {
        //private static List<string> successfiles = new List<string>();
        private static List<string> conflictfiles = new List<string>();

        ConfigEntity configEntity;
        public SVNMerger(ConfigEntity config)
        {
            configEntity = config;
        }
       
        public void Merge()
        {
            SvnUpdateResult result;

            SvnClient svnClient = new SvnClient();

            string svnDir = @".svn";

            svnClient.Authentication.DefaultCredentials = new NetworkCredential(configEntity.userName, configEntity.password);
            // Note: SvnDepth.Infinity and SvnDepth.Unknown are used for full recursive

            
            SvnTarget mergeFrom = new SvnUriTarget(configEntity.sourceUrl);
            SvnTarget mergeTo = new SvnUriTarget(configEntity.targetUrl);

            // Get latest code for target path - full recursive
            SvnUriTarget branch = new SvnUriTarget(configEntity.targetUrl);
            #region UpdateorCheckout
            if (Directory.Exists(configEntity.checkoutPath + "\\" + svnDir))
            {
                SvnUpdateArgs updateArgs = new SvnUpdateArgs();
                updateArgs.Depth = SvnDepth.Infinity;
                svnClient.Update(configEntity.checkoutPath, updateArgs, out result);
            }
            else
            {
                SvnCheckOutArgs checkoutArgs = new SvnCheckOutArgs();
                checkoutArgs.Depth = SvnDepth.Infinity;
                svnClient.CheckOut(branch, configEntity.checkoutPath, checkoutArgs, out result);
            }
            #endregion

            #region Get eligible revisions from trunk
            // Get revisions from source to be merged in target (Retrieves revisions of source that are available for merging)

            SvnMergesEligibleArgs mergeEligArgs = new SvnMergesEligibleArgs();
            mergeEligArgs.Depth = SvnDepth.Empty;

            var mergeList = new List<SvnMergesEligibleEventArgs>();

            var list = svnClient.ListMergesEligible(mergeTo, mergeFrom, mergeEligArgs,
                delegate (object sender, SvnMergesEligibleEventArgs e)
                {
                    e.Detach();
                    mergeList.Add(e);
                });
            #endregion

            #region excluding authors changes and consider rest - TODO : provide switch to On/Off
            // i want to exclude my changes from list and merge rest
            List<long> tobemerged = new List<long>();

            foreach (SvnMergesEligibleEventArgs argument in mergeList)
            {
                if (configEntity.author == string.Empty && configEntity.author != argument.Author)
                {
                    tobemerged.Add(argument.Revision);
                }
            }
            #endregion

            #region finding first and last revision number
            long start = 0;
            long end = 0;

            if (tobemerged.Count > 1)
            {
                start = tobemerged[0];     // first
                end = tobemerged[tobemerged.Count - 1];      // last
            }
            else
            {
                Logger log = new Logger(configEntity.logFilePath);
                List<string> nothingtoMerge = new List<string>();
                nothingtoMerge.Add("Nothing to merge from " + configEntity.sourceUrl);
                log.WriteLog(nothingtoMerge);
                return;     // nothing to merge, exiting
            }
            #endregion

            //svnClient.CleanUp(targetPath);

            start = start - 1;      // reducing by one, as while merging we observed first revision is not being considered for merging

            SvnRevisionRange mergeRange = new SvnRevisionRange(start, end);
            SvnMergeArgs mergeArgs = new SvnMergeArgs();
            mergeArgs.Depth = SvnDepth.Infinity;
            mergeArgs.Conflict += MergeArgs_Conflict;
            //mergeArgs.Notify += MergeArgs_Notify;

            SvnRevertArgs revertAgs = new SvnRevertArgs();
            revertAgs.Depth = SvnDepth.Infinity;
            try
            {
                bool mergeResult1 = svnClient.Merge(configEntity.checkoutPath, mergeFrom, mergeRange, mergeArgs);

                SvnCommitArgs commitArgs = new SvnCommitArgs();
                commitArgs.Depth = SvnDepth.Infinity;
                commitArgs.LogMessage = "Merged the code from " + configEntity.sourceUrl + ". From revision: " + (start + 1).ToString() + " to " + end.ToString();
                //svnClient.Commit(configEntity.checkoutPath, commitArgs);
            }
            catch (Exception ex)
            {
                svnClient.Revert(configEntity.checkoutPath, revertAgs);
                svnClient.CleanUp(configEntity.checkoutPath);

                //successfiles.Clear();
                Logger log = new Logger(configEntity.logFilePath);
                conflictfiles.Add(ex.Message);
                conflictfiles.Add(Convert.ToString(ex.InnerException));
                log.WriteLog(conflictfiles);
            }            
        }
       

        //private static void MergeArgs_Notify(object sender, SvnNotifyEventArgs e)
        //{
        //    successfiles.Add(e.FullPath);
        //}

        private static void MergeArgs_Conflict(object sender, SvnConflictEventArgs e)
        {
            conflictfiles.Add(e.MergedFile);
        }
    }
}
