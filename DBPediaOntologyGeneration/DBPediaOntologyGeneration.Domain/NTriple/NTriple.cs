using System;
using System.Collections.Generic;
using System.Text;

namespace DBPediaOntologyGeneration.Domain.NTriple
{
    public class NTriple
    {
        public Tuple<string, string, string> Triple { get; private set; }

        public NTriple(string subject, string predicate, string objectValue)
        {
            Triple = new Tuple<string, string, string>( subject, predicate, objectValue );
        }
    }
}
