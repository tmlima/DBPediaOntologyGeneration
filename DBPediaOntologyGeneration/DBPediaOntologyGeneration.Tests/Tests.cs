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
        private NTripleReader reader;
        private OntologyGenerator generator;

        [ TestMethod]
        public void ReadNTripleFile()
        {
            NTripleCollection nTriples = reader.ReadFile( MockFolderPath + "skos_categories_en.nt" );

            Assert.AreEqual( 16, nTriples.Triples.Count );
            Assert.AreEqual( "http://dbpedia.org/resource/Category:Futurama", nTriples.Triples[ 4 ].Triple.Item1 );
            Assert.AreEqual( "http://www.w3.org/2004/02/skos/core#broader", nTriples.Triples[ 4 ].Triple.Item2 );
            Assert.AreEqual( "http://dbpedia.org/resource/Category:Works_by_Matt_Groening", nTriples.Triples[ 4 ].Triple.Item3 );
        }

        [TestMethod]
        public void GetNTriplesRecursively()
        {
            NTripleCollection nTriples = reader.GetTriplesRecursvely( MockFolderPath + "skos_categories_en.nt", "http://dbpedia.org/resource/Category:Wars" );

            Assert.AreEqual( 4, nTriples.Triples.Count );
            Assert.IsTrue( nTriples.Triples.Any( x => x.Triple.Item1 == "http://dbpedia.org/resource/Category:Vietnam_War" ) );
            Assert.IsTrue( nTriples.Triples.Any( x => x.Triple.Item1 == "http://dbpedia.org/resource/Category:World_War_II" ) );
        }

        [TestInitialize]
        public void Initialize()
        {
            reader = new NTripleReader();
            generator = new OntologyGenerator();
        }

        [TestMethod]
        public void GenerateOntology()
        {
            List<Entity> entities = MockEntities();
            List<Individual> individuals = MockIndividuals();

            XmlDocument owl = new OwlGenerator().Generate( entities, individuals );

            Assert.AreEqual( 2, owl.GetElementsByTagName( "owl:Class" ).Count );
            Assert.AreEqual( 3, owl.GetElementsByTagName( "owl:NamedIndividual" ).Count );
            Assert.AreEqual( 3, owl.GetElementsByTagName( "ShortDescription" ).Count );
            Assert.AreEqual( 3, owl.GetElementsByTagName( "WikipediaUrl" ).Count );
        }

        [TestMethod]
        public void GenerateEntityFromNTriple()
        {
            NTripleCollection nTriples = reader.ReadFile( MockFolderPath + "skos_categories_en.nt" );
            List<Entity> entities = new List<Entity>();

            nTriples.Triples.ForEach( x => entities.Add( new Entity( x ) ) );

            Assert.AreEqual( 16, entities.Count );
            Assert.IsTrue( entities.Single( x => x.Name == "Wars_involving_the_United_States" ).Parent == "Wars_by_country" );
        }

        [TestMethod]
        public void GenerateIndividualFromNTriple()
        {
            NTripleCollection nTriples = reader.ReadFile( MockFolderPath + "article_categories_en.nt" );
            List<Individual> individuals = new List<Individual>();

            nTriples.Triples.ForEach( x => individuals.Add( new Individual( x ) ) );

            Assert.AreEqual( 4, individuals.Count );
            Assert.IsTrue( individuals.Single( x => x.Name == "Operation_Brushwood" ).Category == "World_War_II" );
        }

        [TestMethod]
        public void AddWikipediaLinks()
        {
            List<Individual> individuals = MockIndividuals();
            NTripleCollection wikipediLinks = reader.ReadFile( MockFolderPath + "wikipedia_links_en.nt");

            individuals = generator.AddWikipediaLinks( individuals, wikipediLinks );

            Assert.AreEqual( "http://en.wikipedia.org/wiki/Anchor", individuals.Single( x => x.Name == "Anchor" ).WikipediaLink );
            Assert.AreEqual( "http://en.wikipedia.org/wiki/Keel", individuals.Single( x => x.Name == "Keel" ).WikipediaLink );
        }

        [TestMethod]
        public void AddShortAbstracts()
        {
            List<Individual> individuals = MockIndividuals();
            NTripleCollection shortAbstracts = reader.ReadFile( MockFolderPath + "short_abstracts_en.nt" );

            individuals = generator.AddShortAbstracts( individuals, shortAbstracts );

            Assert.IsTrue( individuals.Single( x => x.Name == "Anchor" ).ShortAbstract.StartsWith( "An anchor is a device, normally made of metal, that is used" ) );
            Assert.IsTrue( individuals.Single( x => x.Name == "Keel" ).ShortAbstract.StartsWith( "In boats and ships, keel can refer to either" ) );
        }

        private List<Entity> MockEntities()
        {
            return new List<Entity>()
            {
                new Entity("Sailing_ship_elements", "Nautical_terms"),
                new Entity("Nautical_terms", null)
            };
        }

        private List<Individual> MockIndividuals()
        {
            return new List<Individual>()
            {
                new Individual(new NTriple("<http://dbpedia.org/resource/Anchor>", "<http://purl.org/dc/terms/subject>", "<http://dbpedia.org/resource/Category:Sailing_ship_elements>"))
                {
                    ShortAbstract = "Anchor SA",
                    WikipediaLink = "http://www.anchor"
                },
                new Individual(new NTriple("<http://dbpedia.org/resource/Keel>", "<http://purl.org/dc/terms/subject>", "<http://dbpedia.org/resource/Category:Sailing_ship_elements>"))
                {
                    ShortAbstract = "Keel SA",
                    WikipediaLink = "http://www.keel"
                },
                new Individual(new NTriple("<http://dbpedia.org/resource/Hawsehole>", "<http://purl.org/dc/terms/subject>", "<http://dbpedia.org/resource/Category:Sailing_ship_elements>"))
                {
                    ShortAbstract = "Hawsehole SA",
                    WikipediaLink = "http://www.hawsehole"
                },
            };
        }
    }
}
