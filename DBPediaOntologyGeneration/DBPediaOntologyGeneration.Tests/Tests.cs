using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBPediaOntologyGeneration.Scripts;
using DBPediaOntologyGeneration.Domain.NTriple;

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
            Assert.AreEqual( 12, nTriples.Triples.Count );
            Assert.AreEqual( "<http://dbpedia.org/resource/Category:Futurama>", nTriples.Triples[ 4 ].Triple.Item1 );
            Assert.AreEqual( "<http://www.w3.org/2004/02/skos/core#broader>", nTriples.Triples[ 4 ].Triple.Item2 );
            Assert.AreEqual( "<http://dbpedia.org/resource/Category:Works_by_Matt_Groening>", nTriples.Triples[ 4 ].Triple.Item3 );
        }
    }
}
