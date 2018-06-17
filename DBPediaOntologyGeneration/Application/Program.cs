using DBPediaOntologyGeneration.Domain.NTriple;
using DBPediaOntologyGeneration.Domain.Ontology;
using DBPediaOntologyGeneration.Scripts;
using DBPediaOntologyGeneration.Scripts.Ontology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Application
{
    class Program
    {
        static void Main( string[] args )
        {
            try
            {
                string path = args[ 0 ] + @"\";

                Console.WriteLine( "Starting..." );

                const string RootCategory = "http://dbpedia.org/resource/Category:Nautical_terms";
                NTripleReader reader = new NTripleReader();

                Console.WriteLine( "Reading N-triples..." );
                NTripleCollection skolCategories = reader.GetTriplesRecursvely( path + "skos_categories_en.nt", RootCategory );
                List<string> categories = skolCategories.Subjects;
                categories.Add( RootCategory );
                NTripleCollection articleCategories = reader.GetTriples( path + "article_categories_en.nt", categories, NTripleReader.NtripleSearchType.Object );
                NTripleCollection wikipediLinks = reader.GetTriples( path + "wikipedia_links_en.nt", articleCategories.Subjects, NTripleReader.NtripleSearchType.Object );
                NTripleCollection shortAbstracts = reader.GetTriples( path + "short_abstracts_en.nt", articleCategories.Subjects, NTripleReader.NtripleSearchType.Subject );
                Console.WriteLine( "Reading N-triples... - Done" );

                List<Entity> entities = skolCategories.Triples.Select( x => new Entity( x ) ).ToList();
                List<Individual> individuals = articleCategories.Triples.Select( x => new Individual( x ) ).ToList();

                Console.WriteLine( "Adding Wikipedia links and short abstracts..." );
                OntologyGenerator ontologyGenerator = new OntologyGenerator();
                individuals = ontologyGenerator.AddWikipediaLinks( individuals, wikipediLinks );
                individuals = ontologyGenerator.AddShortAbstracts( individuals, shortAbstracts );
                Console.WriteLine( "Adding Wikipedia links and short abstracts... - Done" );

                Console.WriteLine( "Generating ontology..." );
                XmlDocument owl = new OwlGenerator().Generate( entities, individuals );
                Console.WriteLine( "Generating ontology... - Done" );

                string fileName = "generatedOntology_" + DateTime.Now.ToString( "yyyyMMdd_HHmmss" ) + ".owl";
                owl.Save( fileName );
                Console.WriteLine( "Ontology file [" + fileName + "]" );

                Console.WriteLine( "Done" );

            }
            catch ( Exception e )
            {
                Console.WriteLine( "Error: " + e.Message );
                Console.Write( e.StackTrace );
            }
            Console.ReadKey();
        }
    }
}
