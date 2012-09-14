using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCGenerator
{
    class DataPair
    {
        private String _label;
        public String Label
        {
            get { return _label; }
            set { _label = value; }
        }

        private String _value;

        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public DataPair(string label, string value)
        {
            this.Label = label;
            this.Value = value;
        }
        public override string ToString()
        {
            return Label + " - " + Value;
        }

        public override bool Equals(object obj)
        {
            DataPair other = obj as DataPair;
            if(other==null)
                return false;
            if (other.Label.Equals(this.Label) && other.Value.Equals(this.Value))
                return true;
            return false;
        }
    }
}
