using DBPediaOntologyGeneration.Domain.Ontology;
using System;
using System.Collections.Generic;
using System.Xml;

namespace DBPediaOntologyGeneration.Scripts.Ontology
{
    public class OwlGenerator
    {
        private const string OntologyName = "generated-ontology";
        private const string NamespaceOwl = "http://www.w3.org/2002/07/owl#";
        private const string NamespaceRdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        private const string NamespaceOntology = "http://www.semanticweb.org/thiago/ontologies/2018/5/" + OntologyName;
        private const string NamespaceRdfs = "http://www.w3.org/2000/01/rdf-schema#";
        private const string NamespaceXmlSchema = "http://www.w3.org/2001/XMLSchema#";

        public XmlDocument Generate( List<Entity> entities, List<Individual> individuals )
        {
            XmlDocument document = GenerateDocument( entities, individuals );
            return document;
        }

        private XmlDocument GenerateDocument( List<Entity> entities, List<Individual> individuals )
        {
            XmlDocument document = new XmlDocument();

            XmlElement rdf = document.CreateElement( "rdf", "RDF", "http://www.w3.org/1999/02/22-rdf-syntax-ns#" );
            rdf.SetAttribute( "xmlns", NamespaceOntology + "#" );
            rdf.SetAttribute( "xml:base", NamespaceOntology );
            rdf.SetAttribute( "xmlns:rdf", NamespaceRdf );
            rdf.SetAttribute( "xmlns:owl", NamespaceOwl );
            rdf.SetAttribute( "xmlns:xml", "http://www.w3.org/XML/1998/namespace" );
            rdf.SetAttribute( "xmlns:xsd", NamespaceXmlSchema );
            rdf.SetAttribute( "xmlns:rdfs", NamespaceRdfs );
            rdf.SetAttribute( "xmlns:untitled-ontology-11", NamespaceOntology + "#" );
            document.AppendChild( rdf );

            XmlElement ontology = document.CreateElement( "owl", "Ontology", NamespaceOwl );
            ontology.SetAttribute( "about", NamespaceRdf, NamespaceOntology );
            rdf.AppendChild( ontology );

            GenerateDataPropertyDeclaration( document, rdf, "ShortDescription" );
            GenerateDataPropertyDeclaration( document, rdf, "WikipediaUrl" );

            GenerateEntities( document, rdf, entities );
            GenerateIndividuals( document, rdf, individuals );

            return document;
        }

        private void GenerateEntities(XmlDocument document, XmlElement parent, List<Entity> entities )
        {
            foreach (Entity e in entities)
            {
                XmlElement entity = document.CreateElement( "owl", "Class", NamespaceOwl );
                entity.SetAttribute( "about", NamespaceRdf, NamespaceOntology + "#" + e.Name );
                if ( e.Parent != null )
                {
                    XmlElement subClassOf = document.CreateElement( "rdfs", "subClassOf", NamespaceRdfs );
                    subClassOf.SetAttribute( "resource", NamespaceRdf, NamespaceOntology + "#" + e.Parent );
                    entity.AppendChild( subClassOf );
                }

                parent.AppendChild( entity );    
            }
        }

        private void GenerateIndividuals( XmlDocument document, XmlElement parent, List<Individual> individuals )
        {
            foreach (Individual i in individuals)
            {
                XmlElement entity = document.CreateElement( "owl", "NamedIndividual", NamespaceOwl );
                entity.SetAttribute( "about", NamespaceRdf, NamespaceOntology + "#" + i.Name );

                XmlElement subClassOf = document.CreateElement( "rdf", "type", NamespaceRdf );
                subClassOf.SetAttribute( "resource", NamespaceRdf, NamespaceOntology + "#" + i.Category );
                entity.AppendChild( subClassOf );

                GenerateDataProperty( document, entity, "WikipediaUrl", i.WikipediaLink );
                GenerateDataProperty( document, entity, "ShortDescription", i.ShortAbstract );

                parent.AppendChild( entity );
            }
        }

        private void GenerateDataPropertyDeclaration(XmlDocument document, XmlElement parent, string dataPropertyName)
        {

            XmlElement ontology = document.CreateElement( "owl", "DatatypeProperty", NamespaceOwl );
            ontology.SetAttribute( "about", NamespaceRdf, NamespaceOntology + "#" + dataPropertyName );
            parent.AppendChild( ontology );
        }

        private void GenerateDataProperty( XmlDocument document, XmlElement parent, string dataPropertyName, string value )
        {
            XmlElement subClassOf = document.CreateElement( OntologyName, dataPropertyName, NamespaceOntology + "#" );
            subClassOf.SetAttribute( "datatype", NamespaceRdf, NamespaceXmlSchema + "string" );
            subClassOf.InnerText = value;
            parent.AppendChild( subClassOf );
        }
    }
}
