using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCGenerator
{
    class World
    {
        private string worldName;
        public List<ValueWeight> NameWeightDistribution = new List<ValueWeight>();
        public String OutputFile { get; set; }

        private List<String> _associatedTraits = new List<string>();

        public List<String> AssociatedTraits
        {
            get { return _associatedTraits; }
            set { _associatedTraits = value; }
        }

        
        public World(string worldName)
        {
            // TODO: Complete member initialization
            this.worldName = worldName;
        }




        internal void AddNameWeight(string NameEthnicity, int NameFrequency)
        {
            NameWeightDistribution.Add(new ValueWeight(NameEthnicity, NameFrequency));
        }


        private List<string> _outputOrder = new List<string>();
        public List<string> OutputOrder { get { return _outputOrder; } set { _outputOrder = value; } }

        internal void AddTrait(string line)
        {
            AssociatedTraits.Add(line);
        }

        public int MaxNameWeight { get; set; }
    }
}
