using System;

namespace DBPediaOntologyGeneration.Domain.Ontology
{
    public class Entity
    {
        public string Name { get; set; }
        public string Parent { get; set; }

        public Entity(string name, string parent )
        {
            this.Name = name;
            this.Parent = parent;
        }
    }
}
