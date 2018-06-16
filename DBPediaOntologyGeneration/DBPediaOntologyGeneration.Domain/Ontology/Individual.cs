using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DBPediaOntologyGeneration.Domain.Ontology
{
    public class Individual
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string ShortAbstract { get; set; }

        public Individual( NTriple.NTriple nTriple )
        {
            Regex regex = new Regex( @"<http://dbpedia.org/resource/(\w+)>" );
            this.Name = regex.Match( nTriple.Triple.Item1 ).Groups[ 1 ].Value;
            if ( nTriple.Triple.Item2.Contains( "http://purl.org/dc/terms/subject" ) )
            {
                regex = new Regex( @"<http://dbpedia.org/resource/Category:(\w+)>" );
                this.Category = regex.Match( nTriple.Triple.Item3 ).Groups[ 1 ].Value;
            }

            this.ShortAbstract = string.Empty;
        }
    }
}
