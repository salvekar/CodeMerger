using System;
using CommonLibrary;

using Microsoft.TeamFoundation.Client;
using CodeMergerEntity;

namespace TFS
{
    public class TFSMerger : IMerger
    {
        ConfigEntity configEntity;

        public TFSMerger(ConfigEntity config)
        {
            configEntity = config;
        }

        public void Merge()
        {
            throw new NotImplementedException();

            
            
        }
    }
}
