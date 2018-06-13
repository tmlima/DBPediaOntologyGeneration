using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBPediaOntologyGeneration.Scripts;
using DBPediaOntologyGeneration.Domain.NTriple;
using System.Linq;

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
    }
}
