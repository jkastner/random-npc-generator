using System;
using System.Collections.Generic;

namespace NPCGenerator
{
    internal class World
    {
        public List<ValueWeight> NameWeightDistribution = new List<ValueWeight>();

        private List<String> _associatedTraits = new List<string>();

        private List<string> _outputOrder = new List<string>();
        private List<string> _registeredRollers = new List<string>();

        public World(string worldName)
        {
            // TODO: Complete member initialization
            this.WorldName = worldName;
        }

        public string WorldName { get; set; }
        public String OutputFile { get; set; }

        public List<String> AssociatedTraits
        {
            get { return _associatedTraits; }
            set { _associatedTraits = value; }
        }
        public List<String> RegisteredRollers
        {
            get { return _registeredRollers; }
            set { _registeredRollers = value; }
        }

        public List<string> OutputOrder
        {
            get { return _outputOrder; }
            set { _outputOrder = value; }
        }

        public int MaxNameWeight { get; set; }

        internal void AddNameWeight(string NameEthnicity, int NameFrequency)
        {
            NameWeightDistribution.Add(new ValueWeight(NameEthnicity, NameFrequency));
        }

        internal void AddTrait(string line)
        {
            AssociatedTraits.Add(line);
        }

        internal void AddRegisteredRoller(string rollerName)
        {
            _registeredRollers.Add(rollerName);
        }
    }
}