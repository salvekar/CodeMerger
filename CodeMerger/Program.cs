using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary;
using CodeMergerEntity;

using SVN;
using Microsoft.Practices.Unity;

namespace CommonLibrary
{
    class Program
    {
        private static ConfigEntity configEntity;
        private static LogEntity logEntity;

        static void Main(string[] args)
        {
            configEntity = new ConfigEntity();
            configEntity.ConfigTool = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings.Get("ConfigTool"));
            configEntity.userName = System.Configuration.ConfigurationSettings.AppSettings.Get("userName");
            configEntity.password = System.Configuration.ConfigurationSettings.AppSettings.Get("password");
            configEntity.author = System.Configuration.ConfigurationSettings.AppSettings.Get("author");
            configEntity.sourceUrl = System.Configuration.ConfigurationSettings.AppSettings.Get("sourceUrl");
            configEntity.targetUrl = System.Configuration.ConfigurationSettings.AppSettings.Get("targetUrl");
            configEntity.checkoutPath = System.Configuration.ConfigurationSettings.AppSettings.Get("checkoutPath");
            configEntity.logFilePath = System.Configuration.ConfigurationSettings.AppSettings.Get("logFilePath");

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
                case (Int32)VersionControlTools.TFS:
                    break;
                default:
                    break;
            }

            merger.Merge();
        }

        

        
        
    }
}
