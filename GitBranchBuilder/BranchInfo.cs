using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBranchBuilder
{
    public class BranchInfo : IEquatable<BranchInfo>
    {
        public static BranchInfo Empty { get; } =
            new BranchInfo(string.Empty);

        public static BranchInfo Develop { get; } =
            new BranchInfo("develop");

        public static BranchInfo Master { get; } =
            new BranchInfo("master");

        public string Name { get; }

        public string Id { get; }

        public BranchInfo(string branchName, string id = "")
        {
            Name = branchName;
            Id = id;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Name.GetHashCode() * Id.GetHashCode();
            }
        }

        public bool Equals(BranchInfo other)
            => Id == other.Id && Name == other.Name;
    }
}
