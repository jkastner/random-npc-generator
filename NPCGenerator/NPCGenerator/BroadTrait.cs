using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCGenerator
{
    class BroadTrait
    {
        private String _traitName;
        private int _maxWeight;
        
        /// <summary>
        /// TraitValues are defined as
        ///     The value (I.e. red hair) which leads to ---
        ///     The weight of this particular trait
        ///     A dictionary of any affected traits and what number it affects that trait by.
        /// 
        /// </summary>

        private List <SingleTraitValue>  _traitValues = new List<SingleTraitValue>();

        public List <SingleTraitValue>  TraitValues
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

        internal void AddValue(string traitValue, int traitWeight, Dictionary<string, int> linkedValues)
        {
            SingleTraitValue tv = new SingleTraitValue(traitValue, traitWeight, linkedValues);
            TraitValues.Add(tv);
        }

    }
}
