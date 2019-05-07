using System;

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

        public object Tag { get; protected set; }

        public BranchInfo SetTag(object tag)
            => new BranchInfo(Name, Id, tag);

        public BranchInfo(string branchName, string id = "", object tag = default)
        {
            Name = branchName;
            Id = id;
            Tag = tag;
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
