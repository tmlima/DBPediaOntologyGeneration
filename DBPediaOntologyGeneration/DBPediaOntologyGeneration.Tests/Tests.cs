using DBPediaOntologyGeneration.Domain.NTriple;
using DBPediaOntologyGeneration.Domain.Ontology;
using DBPediaOntologyGeneration.Scripts;
using DBPediaOntologyGeneration.Scripts.Ontology;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace DBPediaOntologyGeneration.Tests
{
    [TestClass]
    public class Tests
    {
        private readonly string MockFolderPath = Environment.CurrentDirectory + @"\Mock\";

        [TestMethod]
        public void ReadNTripleFile()
        {
            NTripleReader reader = new NTripleReader();
            NTripleCollection nTriples = reader.ReadFile( MockFolderPath + "skos_categories_en.nt" );
            Assert.AreEqual( 16, nTriples.Triples.Count );
            Assert.AreEqual( "<http://dbpedia.org/resource/Category:Futurama>", nTriples.Triples[ 4 ].Triple.Item1 );
            Assert.AreEqual( "<http://www.w3.org/2004/02/skos/core#broader>", nTriples.Triples[ 4 ].Triple.Item2 );
            Assert.AreEqual( "<http://dbpedia.org/resource/Category:Works_by_Matt_Groening>", nTriples.Triples[ 4 ].Triple.Item3 );
        }

        [TestMethod]
        public void GetNTriplesRecursively()
        {
            NTripleReader reader = new NTripleReader();
            NTripleCollection nTriples = reader.GetTriplesRecursvely( MockFolderPath + "skos_categories_en.nt", "<http://dbpedia.org/resource/Category:Wars>" );
            Assert.AreEqual( 4, nTriples.Triples.Count );
            Assert.IsTrue( nTriples.Triples.Any( x => x.Triple.Item1 == "<http://dbpedia.org/resource/Category:Vietnam_War>" ) );
            Assert.IsTrue( nTriples.Triples.Any( x => x.Triple.Item1 == "<http://dbpedia.org/resource/Category:World_War_II>" ) );
        }
        
        [TestMethod]
        public void GenerateOntology()
        {

            List<Entity> entities = new List<Entity>()
            {
                new Entity("Sailing_ship_elements", "Nautical_terms"),
                new Entity("Nautical_terms", null)
            };

            List<Individual> individuals = new List<Individual>()
            {
                new Individual(new NTriple("<http://dbpedia.org/resource/Anchor>", "<http://purl.org/dc/terms/subject>", "<http://dbpedia.org/resource/Category:Sailing_ship_elements>")),
                new Individual(new NTriple("<http://dbpedia.org/resource/Keel>", "<http://purl.org/dc/terms/subject>", "<http://dbpedia.org/resource/Category:Sailing_ship_elements>")),
                new Individual(new NTriple("<http://dbpedia.org/resource/Hawsehole>", "<http://purl.org/dc/terms/subject>", "<http://dbpedia.org/resource/Category:Sailing_ship_elements>"))
            };
            Generator ontologyGenerator = new Generator();
            XmlDocument owl = ontologyGenerator.Generate( entities, individuals );
            Assert.AreEqual( 2, owl.GetElementsByTagName( "owl:Class" ).Count );
            Assert.AreEqual( 3, owl.GetElementsByTagName( "owl:NamedIndividual" ).Count );
        }

        [TestMethod]
        public void GenerateEntityFromNTriple()
        {
            NTripleReader reader = new NTripleReader();
            NTripleCollection nTriples = reader.ReadFile( MockFolderPath + "skos_categories_en.nt" );
            List<Entity> entities = new List<Entity>();
            foreach (NTriple n in nTriples.Triples)
            {
                entities.Add( new Entity( n ) );
            }
            Assert.AreEqual( 16, entities.Count );
            Assert.IsTrue( entities.Single( x => x.Name == "Wars_involving_the_United_States" ).Parent == "Wars_by_country" );
        }

        [TestMethod]
        public void GenerateIndividualFromNTriple()
        {
            NTripleReader reader = new NTripleReader();
            NTripleCollection nTriples = reader.ReadFile( MockFolderPath + "article_categories_en.nt" );
            List<Individual> individuals = new List<Individual>();
            foreach ( NTriple n in nTriples.Triples )
            {
                individuals.Add( new Individual( n ) );
            }
            Assert.AreEqual( 4, individuals.Count );
            Assert.IsTrue( individuals.Single( x => x.Name == "Operation_Brushwood" ).Category == "World_War_II" );
        }
    }
}
