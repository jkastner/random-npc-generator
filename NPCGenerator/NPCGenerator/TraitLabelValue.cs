using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCGenerator
{
    /// <summary>
    /// Semi-tuple. Tuple values are readonly in the data grid.
    /// </summary>
    public class TraitLabelValue
    {
        private String _label;

        public String Label
        {
            get { return _label; }
            private set { _label = value; }
        }
        private String _value;

        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public TraitLabelValue(string label, string value)
        {
            this.Label = label;
            this.Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

  
    }
}
