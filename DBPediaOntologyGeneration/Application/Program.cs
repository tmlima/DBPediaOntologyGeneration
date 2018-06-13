using DBPediaOntologyGeneration.Domain.NTriple;
using DBPediaOntologyGeneration.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    class Program
    {
        static void Main( string[] args )
        {
            string path = @"C:\Users\Thiago\Desktop\DbPedia 3.6\";

            Console.WriteLine( "Starting..." );

            const string RootCategory = "<http://dbpedia.org/resource/Category:Nautical_terms>";
            NTripleReader reader = new NTripleReader();
            NTripleCollection skolCategories = reader.GetTriplesRecursvely( path + "skos_categories_en.nt", RootCategory );

            List<string> categories = skolCategories.Subjects;
            categories.Add( RootCategory );
            NTripleCollection articleCategories = reader.GetTriplesByNTripleObjects( path + "article_categories_en.nt", categories );

            List<string> articles = new List<string>();
            NTripleCollection labels = reader.GetTriplesByNTripleObjects( path + "labels_en.nt", articles);

            List<string> resources = new List<string>();
            NTripleCollection shortAbstracts = reader.GetTriplesByNTripleObjects( path + "short_abstracts_en.nt", resources );

            Console.WriteLine( "Done" );
        }
    }
}
