using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCGenerator
{
    class SingleTraitValue : IComparable
    {
        private String _traitValue;

        public String TraitValue
        {
            get { return _traitValue; }
            set { _traitValue = value; }
        }

        private Dictionary<String, int> _linkedValues = new Dictionary<String, int>();
        
        private int _traitWeight;
        public int TraitWeight
        {
            get { return _traitWeight; }
            set { _traitWeight = value; }
        }

        public SingleTraitValue(string traitValue, int traitWeight, Dictionary<string, int> linkedValues)
        {
            // TODO: Complete member initialization
            this.TraitValue = traitValue;
            this.TraitWeight = traitWeight;
            this.LinkedValues = linkedValues;
        }

        public Dictionary<String, int> LinkedValues
        {
            get
            {
                return _linkedValues;
            }
            set
            {
                _linkedValues = value;
            }
        }



        #region IComparable Members

        public int CompareTo(object obj)
        {
            SingleTraitValue other = obj as SingleTraitValue;
            if(this.TraitWeight>other.TraitWeight)
                return 1;
            return 0;
        }

        #endregion
    }
}
