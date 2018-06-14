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
            string path = args[0] + @"\";

            Console.WriteLine( "Starting..." );

            const string RootCategory = "<http://dbpedia.org/resource/Category:Nautical_terms>";
            NTripleReader reader = new NTripleReader();
            NTripleCollection skolCategories = reader.GetTriplesRecursvely( path + "skos_categories_en.nt", RootCategory );

            List<string> categories = skolCategories.Subjects;
            categories.Add( RootCategory );
            NTripleCollection articleCategories = reader.GetTriplesByNTripleObjects( path + "article_categories_en.nt", categories );

            List<string> articles = articleCategories.Subjects.Distinct().ToList();
            NTripleCollection labels = reader.GetTriplesByNTripleObjects( path + "labels_en.nt", articles);

            NTripleCollection shortAbstracts = reader.GetTriplesByNTripleObjects(path + "short_abstracts_en.nt", articles);

            Console.WriteLine( "Done" );
        }
    }
}
