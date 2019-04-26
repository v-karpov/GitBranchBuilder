using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GitBranchBuilder.Jobs.Impl
{
    public class CombinedJob<T1, T2, TOut> : Job<(T1, T2), TOut>
    {
        protected virtual JoinBlock<T1, T2> JoinBlock { get; }

        protected virtual TransformBlock<Tuple<T1, T2>, TOut> TransformBlock { get; }

        protected override ITargetBlock<(T1, T2)> TargetBlock => throw new NotImplementedException();

        protected override ISourceBlock<TOut> SourceBlock => throw new NotImplementedException();

        public CombinedJob()
        {
            JoinBlock = new JoinBlock<T1, T2>();
        }
    }
}
