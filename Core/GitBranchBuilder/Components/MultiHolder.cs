using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using GitBranchBuilder.Providers;

namespace GitBranchBuilder.Components
{
    public class MultiHolder<T> : Lazy<IEnumerable<T>>, IEnumerable<T>, IComponent
    {
        public IEnumerator<T> GetEnumerator() => Value.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public MultiHolder(IEnumerable<IProvider<T>> providers) :
            base(valueFactory: () => providers.Select(x => x.GetValue()),
                 mode: LazyThreadSafetyMode.PublicationOnly)
        {

        }

        public static implicit operator T(MultiHolder<T> multiHolder)
            => multiHolder.FirstOrDefault();
    }
}
