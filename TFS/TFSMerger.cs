using System;
using CommonLibrary;

using Microsoft.TeamFoundation.Client;
using CodeMergerEntity;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFS
{
    public class TFSMerger : IMerger
    {
        private const string TfsServer = "http://localhost:8080/tfs";
        private const string TfsUserName = "tfsusername";
        private const string TfsPassword = "tfspassword";
        private const string FromBranch = "$/TestSource/Dev";
        private const string ToBranch = "$/TestSource/Main";

        static TfsTeamProjectCollection tfsTeamProjectCollection;
        static TfsTeamProjectCollection GetTeamProjectCollection()
        {
            var tfsTeamProjectCollection = new TfsTeamProjectCollection(new Uri(TfsServer), new System.Net.NetworkCredential(TfsUserName, TfsPassword));
            tfsTeamProjectCollection.EnsureAuthenticated();
            return tfsTeamProjectCollection;
        }

        ConfigEntity configEntity;

        public TFSMerger(ConfigEntity config)
        {
            configEntity = config;
        }

        public void Merge()
        {
            tfsTeamProjectCollection = GetTeamProjectCollection();
            var versionControl = tfsTeamProjectCollection.GetService<VersionControlServer>();
            var mergeCandidates = versionControl.GetMergeCandidates(FromBranch, ToBranch, RecursionType.Full);

        }
    }
}
