using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCGenerator
{
    class NameList
    {
        private Dictionary<String, List<String>> _firstNames = new Dictionary<String, List<string>>();
        public Dictionary<String, List<String>> FirstNames
        {
            get {return _firstNames;}
            private set {_firstNames = value;}
        }
        
        private List<String> _lastNames = new List<String>();
        public List<String> LastNames
        {
            get {return _lastNames;}
            private set { _lastNames = value; }
        }


        private String _nameEthnicity;
        private string ethnicity;

        public NameList(string ethnicity)
        {
            this.ethnicity = ethnicity;
        }
        public String NameEthnicity
        {
            get { return _nameEthnicity; }
            set { _nameEthnicity = value; }
        }


        internal void AddFirstName(string name, String gender)
        {
            if (!_firstNames.ContainsKey(gender))
            {
                _firstNames.Add(gender, new List<String>());
            }
            _firstNames[gender].Add(name);
        }

        internal void AddLastName(string name)
        {
            _lastNames.Add(name);
        }



    }
}
