using System;
using System.Collections.Generic;
using System.Text;

namespace DBPediaOntologyGeneration.Domain.NTriple
{
    public class NTripleCollection
    {
        public List<NTriple> Triples { get; set; }

        public NTripleCollection()
        {
            this.Triples = new List<NTriple>();
        }
    }
}
