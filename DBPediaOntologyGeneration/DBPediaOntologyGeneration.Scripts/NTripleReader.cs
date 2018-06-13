using DBPediaOntologyGeneration.Domain.NTriple;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBPediaOntologyGeneration.Scripts
{
    public class NTripleReader
    {
        public NTripleCollection ReadFile( string path )
        {
            NTripleCollection nTripleCollection = new NTripleCollection();
            long currentLine = 0;
            StreamReader reader = new StreamReader( path );
            while ( !reader.EndOfStream )
            {
                currentLine++;
                Console.Write( "\rLine {0}", currentLine );
                string[] line = reader.ReadLine().Split(' ');
                nTripleCollection.Triples.Add( new NTriple( line[ 0 ], line[ 1 ], line[ 2 ] ) );
            }

            reader.Close();

            return nTripleCollection;
        }
    }
}
