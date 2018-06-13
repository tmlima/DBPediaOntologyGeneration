using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DBPediaOntologyGeneration.Domain.NTriple
{
    public class NTripleCollection
    {
        public List<NTriple> Triples { get; private set; }

        public NTripleCollection()
        {
            this.Triples = new List<NTriple>();
        }

        public void AddTriples(List<NTriple> triples)
        {
            foreach (NTriple triple in triples)
            {
                if ( !this.Triples.Any( x => x.Equals( triple ) ) )
                    this.Triples.Add( triple );
            }
        }

        public List<string> Subjects
        {
            get
            {
                return Triples.Select( x => x.Triple.Item1 ).ToList();
            }
        }
    }
}
