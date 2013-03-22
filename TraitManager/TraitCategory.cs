using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraitManagerProject
{
    class TraitCategory
    {
        private List<TraitEntry> _traitEntries;

        public List<TraitEntry> TraitEntries
        {
            get { return _traitEntries; }
            set { _traitEntries = value; }
        }
        
    }
}
