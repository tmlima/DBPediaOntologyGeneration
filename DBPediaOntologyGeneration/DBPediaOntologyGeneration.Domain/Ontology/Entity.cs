
using System.Text.RegularExpressions;

namespace DBPediaOntologyGeneration.Domain.Ontology
{
    public class Entity
    {
        public string Name { get; set; }
        public string Parent { get; set; }

        public Entity(string name, string parent )
        {
            this.Name = name;
            this.Parent = parent;
        }

        public Entity( NTriple.NTriple nTriple )
        {
            const int CategoryNameGroupIndex = 1;
            Regex categoriNameRegex = new Regex( @"<http://dbpedia.org/resource/Category:(\w+)>" );
            this.Name = categoriNameRegex.Match( nTriple.Triple.Item1 ).Groups[ CategoryNameGroupIndex ].Value;
            if ( nTriple.Triple.Item2.Contains("http://www.w3.org/2004/02/skos/core#broader"))
            {
                this.Parent = categoriNameRegex.Match( nTriple.Triple.Item3 ).Groups[ CategoryNameGroupIndex ].Value;
            }
        }
    }
}
