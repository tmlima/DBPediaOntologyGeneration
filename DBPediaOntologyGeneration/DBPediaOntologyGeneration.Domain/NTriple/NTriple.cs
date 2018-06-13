using System;

namespace DBPediaOntologyGeneration.Domain.NTriple
{
    public class NTriple : IEquatable<NTriple>
    {
        public Tuple<string, string, string> Triple { get; private set; }

        public NTriple(string subject, string predicate, string objectValue)
        {
            Triple = new Tuple<string, string, string>( subject, predicate, objectValue );
        }

        public bool Equals( NTriple other )
        {
            return 
                this.Triple.Item1 == other.Triple.Item1 && 
                this.Triple.Item2 == other.Triple.Item2 && 
                this.Triple.Item3 == other.Triple.Item3;
        }
    }
}
