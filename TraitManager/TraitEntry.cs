using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraitManagerProject
{
    class TraitEntry
    {
        private String _value;
        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }
        
        private int _weight;
        public int Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        
    }
}
