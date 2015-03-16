using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using NPCGenerator.Annotations;

namespace NPCGenerator
{
    public class NPC : INotifyPropertyChanged
    {
        private ObservableCollection<TraitLabelValue> _traits = new ObservableCollection<TraitLabelValue>();
        private string _npcNote;

        public NPC() :
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

        public String NPCNote
        {
            get { return _traits.First(x => x.Label.Equals("Note")).Value; }
            set
            {
                if (value == _npcNote) return;
                _npcNote = value;
                _traits.First(x => x.Label.Equals("Note")).Value = value;
                OnPropertyChanged("NPCNote");
            }
        }

        public ObservableCollection<TraitLabelValue> Traits
        {
            get { return _traits; }
            set { _traits = value; }
        }

        public String WorldName { get; set; }

        public override string ToString()
        {
            return Traits.First().ToString();
        }

        public void AddTrait(String label, String value)
        {
            
            
            Traits.Add(new TraitLabelValue(label, value));
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
            if (traitLabel.Contains("Note"))
            {
                newValue = newValue.Replace("{TAB}", "\t");
                newValue = newValue.Replace("{NEWLINE}", Environment.NewLine);
            }
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

        public String CharacterName
        {
            get
            {
                var nameTrait = Traits.FirstOrDefault(x => x.Label.Equals("Name"));
                if (nameTrait != null)
                {
                    return nameTrait.Value;
                }
                return "";
            }
        }

        public ObservableCollection<TraitLabelValue> SortedDisplayTraits
        {
            get
            {
                List <TraitLabelValue> hasContent = new List<TraitLabelValue>();
                List <TraitLabelValue> noContent = new List<TraitLabelValue>();
                hasContent.AddRange(Traits.Where(x=>!String.IsNullOrEmpty(x.Value) && !x.Label.Equals("Note")));
                noContent.AddRange(Traits.Where(x => String.IsNullOrEmpty(x.Value) && !x.Label.Equals("Note")));
                hasContent.AddRange(noContent);
                return new ObservableCollection<TraitLabelValue>(hasContent);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public int IndexOfTrait(string note)
        {
            var match = Traits.FirstOrDefault(x => x.Label.Equals(note));
            if (match == null)
            {
                return -1;
            }
            return Traits.IndexOf(match);
        }
    }
}