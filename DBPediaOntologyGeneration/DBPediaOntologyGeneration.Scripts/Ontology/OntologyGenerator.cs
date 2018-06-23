using DBPediaOntologyGeneration.Domain.NTriple;
using DBPediaOntologyGeneration.Domain.Ontology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBPediaOntologyGeneration.Scripts.Ontology
{
    public class OntologyGenerator
    {
        public List<Individual> AddWikipediaLinks( List<Individual> individuals, NTripleCollection wikipediaLinks )
        {
            foreach ( Individual i in individuals )
            {
                NTriple triple = wikipediaLinks.Triples.Where( x => x.Triple.Item3 == "http://dbpedia.org/resource/" + i.Name && x.Triple.Item2 == "http://xmlns.com/foaf/0.1/primaryTopic" ).FirstOrDefault();
                if ( triple != null )
                    i.WikipediaLink = triple.Triple.Item1;
            }
            return individuals;
        }

        public List<Individual> AddShortAbstracts( List<Individual> individuals, NTripleCollection shortAbstracts )
        {
            foreach ( Individual i in individuals )
            {
                NTriple triple = shortAbstracts.Triples.Where( x => x.Triple.Item1 == "http://dbpedia.org/resource/" + i.Name && x.Triple.Item2 == "http://www.w3.org/2000/01/rdf-schema#comment" ).FirstOrDefault();
                if ( triple != null )
                    i.ShortAbstract = triple.Triple.Item3;
            }
            return individuals;
        }

        public List<Entity> GenerateEntityForEachIndividual(List<Individual> individuals)
        {
            List<Entity> newEntities = new List<Entity>();

            foreach (Individual i in individuals)
            {
                Entity entity = new Entity( i.Name, i.Category );
                i.Category = entity.Name;
                newEntities.Add( entity );
            }

            return newEntities;
        }
    }
}
