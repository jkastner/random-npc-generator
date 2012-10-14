using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCGenerator
{
    class BroadTrait
    {
        private String _traitName;
        public String TraitName
        {
            get
            {
                return _traitName;
            }
            set
            {
                _traitName = value;
            }
        }
        private int _maxWeight;
        
        /// <summary>
        /// TraitValues are defined as
        ///     The value (I.e. red hair) which leads to ---
        ///     The weight of this particular trait
        ///     A dictionary of any affected traits and what number it affects that trait by.
        /// 
        /// </summary>

        private List <ValueWeight>  _traitValues = new List<ValueWeight>();
        private BroadTrait affectedTrait;

        public List <ValueWeight>  TraitValues
        {
            get { return _traitValues; }
            set { _traitValues = value; }
        }
        

        public int MaxWeight
        {
            get { return _maxWeight; }
            set { _maxWeight = value; }
        }


        public BroadTrait(string traitName)
        {
            this._traitName = traitName;
        }

        

        public List<int> OriginalTraitWeights()
        {
            List<int> originalValues = new List<int>();
            foreach (ValueWeight cur in TraitValues)
            {
                originalValues.Add(cur.TraitWeight);
            }
            return originalValues;
        }



        internal void AddValue(string traitValue, int traitWeight, Dictionary<string, int> linkedTableEdits, Dictionary<string, List<ValueWeight>> linkedTableEntryEdits)
        {
            ValueWeight tv = new ValueWeight(traitValue, traitWeight, linkedTableEdits, linkedTableEntryEdits);
            TraitValues.Add(tv);
        }
    }
}
