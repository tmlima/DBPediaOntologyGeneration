using DBPediaOntologyGeneration.Domain.NTriple;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DBPediaOntologyGeneration.Scripts
{
    public class NTripleReader
    {
        public NTripleCollection ReadFile( string path )
        {
            return this.GetTriplesRecursvely( path, new List<string>() );
        }

        public NTripleCollection GetTriplesRecursvely( string path, string rootObject )
        {
            return this.GetTriplesRecursvely( path, new List<string>() { rootObject } );
        }

        public NTripleCollection GetTriplesRecursvely( string path, List<string> filters )
        {
            NTripleCollection nTripleCollection = GetTriplesByNTripleObjects( path, filters );

            bool newTriplesFound = true;
            while (newTriplesFound)
            {
                List<string> subjects = nTripleCollection.Subjects;
                int before = nTripleCollection.Triples.Count;
                nTripleCollection.AddTriples( GetTriplesByNTripleObjects( path, subjects ).Triples );
                int after = nTripleCollection.Triples.Count;
                newTriplesFound = after > before;
            }

            return nTripleCollection;
        }

        public NTripleCollection GetTriplesByNTripleObjects( string path, List<string> nTripleObjects )
        {
            Console.WriteLine( "Reading triples from " + path );
            NTripleCollection nTripleCollection = new NTripleCollection();
            long currentLine = 0;
            StreamReader reader = new StreamReader( path );
            while ( !reader.EndOfStream )
            {
                currentLine++;
                Console.Write( "\rLine {0}", currentLine );
                string line = reader.ReadLine();
                if ( nTripleObjects.Count == 0 || nTripleObjects.Any( x => line.Contains( x ) ) )
                {
                    string[] values = line.Split( ' ' );
                    if (nTripleObjects.Count == 0 || nTripleObjects.Any(x => values[2].Contains(x)))
                        nTripleCollection.Triples.Add( new NTriple( values[ 0 ], values[ 1 ], values[ 2 ] ) );
                }
            }

            Console.WriteLine( Environment.NewLine + "Reading triples from " + path + " - Done");

            reader.Close();

            return nTripleCollection;
        }

        public NTripleCollection GetTriplesByNTripleSubjects( string path, List<string> nTripleObjects )
        {
            throw new NotImplementedException();
        }
    }
}
