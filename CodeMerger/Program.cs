using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary;
using CodeMergerEntity;
using System.Configuration;

using SVN;
using Microsoft.Practices.Unity;
//using TFS;

namespace CommonLibrary
{
    class Program
    {
        private static ConfigEntity configEntity;
        private static LogEntity logEntity;

        static void Main(string[] args)
        {
            configEntity = new ConfigEntity();
            configEntity.ConfigTool = Convert.ToInt32(ConfigurationManager.AppSettings["ConfigTool"]);
            configEntity.userName = ConfigurationManager.AppSettings["userName"];
            configEntity.password = ConfigurationManager.AppSettings["password"];
            configEntity.author = ConfigurationManager.AppSettings["author"];
            configEntity.sourceUrl = ConfigurationManager.AppSettings["sourceUrl"];
            configEntity.targetUrl = ConfigurationManager.AppSettings["targetUrl"];
            configEntity.checkoutPath = ConfigurationManager.AppSettings["checkoutPath"];
            configEntity.logFilePath = ConfigurationManager.AppSettings["logFilePath"];

            //IUnityContainer objContainer = new UnityContainer();
            //objContainer.RegisterType<IMerger, SVNMerger>();
            //Merger m = objContainer.Resolve<Merger>();
            //m.Merge();

            IMerger merger = null;

            switch (configEntity.ConfigTool)
            {
                case (Int32)VersionControlTools.SVN:
                    merger = new Merger(new SVNMerger(configEntity));
                    break;
                case (Int32)VersionControlTools.GIT:
                    break;
                //case (Int32)VersionControlTools.TFS:
                //    merger = new Merger(new TFSMerger(configEntity));
                //    break;
                default:
                    break;
            }

            merger.Merge();
        }

        

        
        
    }
}
