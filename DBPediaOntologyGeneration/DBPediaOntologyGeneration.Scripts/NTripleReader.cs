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
            NTripleCollection nTripleCollection = GetTriples( path, filters, NtripleSearchType.Object );

            bool newTriplesFound = true;
            while (newTriplesFound)
            {
                List<string> subjects = nTripleCollection.Subjects;
                int before = nTripleCollection.Triples.Count;
                nTripleCollection.AddTriples( GetTriples( path, subjects, NtripleSearchType.Object ).Triples );
                int after = nTripleCollection.Triples.Count;
                newTriplesFound = after > before;
            }

            return nTripleCollection;
        }

        public NTripleCollection GetTriples(string path, List<string> nTriples, NtripleSearchType searchType)
        {
            int searchTypeIndex = GetSearchTripeIndex( searchType );
            Console.WriteLine( "Reading triples from " + path );
            NTripleCollection nTripleCollection = new NTripleCollection();
            long currentLine = 0;
            StreamReader reader = new StreamReader( path );
            while ( !reader.EndOfStream )
            {
                currentLine++;
                Console.Write( "\rLine {0}", currentLine );
                string line = reader.ReadLine();
                if ( nTriples.Count == 0 || nTriples.Any( x => line.Contains( "<" + x + ">" ) ) )
                {
                    string[] values = line.Split( '>' );
                    if ( nTriples.Count == 0 || nTriples.Any( x => values[ searchTypeIndex ].Contains( x ) ) )
                        nTripleCollection.Triples.Add( new NTriple( RemoveUnwantedChars( values[ 0 ] ), RemoveUnwantedChars( values[ 1 ] ), RemoveUnwantedChars( values[ 2 ] ) ) );
                }
            }

            Console.WriteLine( Environment.NewLine + "Reading triples from " + path + " - Done" );

            reader.Close();

            return nTripleCollection;
        }

        private string RemoveUnwantedChars(string text)
        {
            text = text.Trim();
            if ( text.First() == '<' )
                text = text.Remove( 0, 1 );
            if ( text.First() == '\"' )
                text = text.Remove( 0, 1 );
            text = text.Replace( "\"@en .", "" );
            return text;
        }

        private int GetSearchTripeIndex(NtripleSearchType searchType)
        {
            switch ( searchType )
            {
                case NtripleSearchType.Subject:
                    return 0;
                case NtripleSearchType.Object:
                    return 2;
                default:
                    throw new Exception();
            }
        }
        public enum NtripleSearchType
        {
            Subject,
            Object 
        }
    }
}
