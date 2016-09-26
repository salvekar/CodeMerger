using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public class Merger : IMerger
    {
        IMerger actualMerger = null;

        public Merger(IMerger mrgr)
        {
            this.actualMerger = mrgr;
        }

        public Merger()
        {

        }

        public virtual void Merge()
        {
            actualMerger.Merge();
        }
    }
}
