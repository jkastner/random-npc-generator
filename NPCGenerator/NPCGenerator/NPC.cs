using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCGenerator
{
    class NPC
    {
        private ObservableCollection<TraitLabelValue> _traits = new ObservableCollection<TraitLabelValue>();

        public NPC():
            this("Placeholder")
        {

        }
      
        public NPC(string p1)
        {
            AddTrait("Name", p1);
            AddTrait("Note", "");
            AddTrait("Gender", "");
        }

        public NPC(string p1, string p2)
        {
            //todo - remove
            AddTrait(p1, p2);
        }

        public ObservableCollection<TraitLabelValue> Traits
        {
            get { return _traits; }
            set { _traits = value; }
        }

        public override string ToString()
        {
            return Traits.First().ToString();
        }

        public void AddTrait(String label, String value)
        {
            Traits.Add(new TraitLabelValue(label, value));
        }

        private String _worldName;

        public String WorldName
        {
            get { return _worldName; }
            set { _worldName = value; }
        }
        



        internal string GetValueForLabel(string traitLabel)
        {
            foreach (TraitLabelValue curTrait in _traits)
            {
                if (curTrait.Label.Equals(traitLabel))
                    return curTrait.Value;
            }
            return "";
        }
        internal bool SetValueForLabel(string traitLabel, string newValue)
        {
            foreach (TraitLabelValue curTrait in _traits)
            {
                if (curTrait.Label.Equals(traitLabel))
                {
                    curTrait.Value = newValue;
                    return true;
                }
            }
            return false;
        }

        internal bool HasValueForLabel(string traitLabel)
        {
            return !String.IsNullOrWhiteSpace(GetValueForLabel(traitLabel));
        }

        internal bool HasTraitWithLabel(string label)
        {
            foreach (TraitLabelValue curTrait in _traits)
            {
                if (curTrait.Label.Equals(label))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
