using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCGenerator
{
    class NPCViewModel
    {
        private NPC _curNPC;
        private static Random _random = new Random();

        public NPC CurNPC
        {
            get { return _curNPC; }
            set { _curNPC = value; }
        }
        

        string _firstNameFile = @"Data\Names\All First Names.txt";
        string _lastNameFile = @"Data\Names\All Last Names.txt";
        string _error = "";


        ObservableCollection<NPC> _npcs = new ObservableCollection<NPC>();
        public ObservableCollection<NPC> NPCs
        {
            get{return _npcs;}
            set{_npcs = value;}
        }

        private ObservableCollection<String> _nameEthnicities = new ObservableCollection<string>();
        public ObservableCollection<String> NameEthnicities
        {
            get { return _nameEthnicities; }
            set { _nameEthnicities = value; }
        }

        //Latin -  Male and Female  - Latin --> Bobicus, Tedimaximus

        private Dictionary<String, NameList> _names = new Dictionary<string, NameList>();
        private ObservableCollection<String> _generatedRandomNames = new ObservableCollection<string>();
        public ObservableCollection<String> GeneratedRandomNames
        {
            get { return _generatedRandomNames; }
            private set { _generatedRandomNames = value; }
        }
        
        public NPCViewModel()
        {
            NPC bob = new NPC("Bob");
            NPC sam = new NPC("Sam");
            NPC terri = new NPC("Terri");
            
            bob.AddTrait("Age", "21");
            sam.AddTrait("Age", "22");
            terri.AddTrait("Age", "23");
            NPCs.Add(bob);
            NPCs.Add(sam);
            NPCs.Add(terri);
            ReadNameFile();
        }

        private void ReadNameFile()
        {
            NameEthnicities.Add("Weighted Random");
            NameEthnicities.Add("Random");
            NameEthnicities.Add("English");
            
            
            string[] names = System.IO.File.ReadAllLines(_firstNameFile);
            AddToDictionary(names);
            names = System.IO.File.ReadAllLines(_lastNameFile);
            AddToDictionary(names);
            
        }

        private void AddToDictionary(string[] readNames)
        {
            foreach (String curLine in readNames)
            {
                String[] lineInfo = curLine.Split('\t');
                String name, gender, ethnicity;
                ethnicity = lineInfo.Last();
                if(!_names.ContainsKey(ethnicity))
                {
                    _names.Add(ethnicity, new NameList(ethnicity));
                }
                
                if (lineInfo.Length == 3)
                {
                    name = lineInfo[0];
                    gender = lineInfo[1];
                    String[] genderTypes = gender.Split(' ');
                    foreach (String curGender in genderTypes)
                    {
                        if(curGender.Trim().Length>0)
                            _names[ethnicity].AddFirstName(name, curGender.Trim());
                    }
                }
                else//length is 2, Last Name
                {
                    name = lineInfo[0];
                    _names[ethnicity].AddLastName(name);
                }
                if(!NameEthnicities.Contains(ethnicity))
                    NameEthnicities.Add(ethnicity);
            }
        }




        internal void GenerateNPC(string gender, string ethnicity)
        {
            _error = "";
            GeneratedRandomNames.Clear();
            
            NameList matchingList = _names[ethnicity];
            if (!matchingList.FirstNames.ContainsKey(gender))
            {
                _error = "No gender match for ethnicity - " + ethnicity;
                return;
            }
            MakeRandomNames(gender, ethnicity);

        }

        private void MakeRandomNames(string gender, string ethnicity)
        {
            FixRandom(ref gender, ref ethnicity);
            List<String> possibleFirstNames = new List<String>(_names[ethnicity].FirstNames[gender]);
            List<String> possibleLastNames = new List<String>(_names[ethnicity].LastNames);
            while (GeneratedRandomNames.Count < 10 && possibleFirstNames.Count > 0)
            {
                String firstName = possibleFirstNames[RandomValue(0, possibleFirstNames.Count)];
                String lastName = "";
                if(possibleLastNames.Count>0)
                    lastName = possibleLastNames[RandomValue(0, possibleLastNames.Count)];
                GeneratedRandomNames.Add((firstName + " " + lastName).Trim());
                possibleFirstNames.Remove(firstName);
            }
        }

        private void FixRandom(ref string gender, ref string ethnicity)
        {
            if (gender.Equals("Random"))
            {
                if (RandomValue(1, 2) == 1)
                    gender = "Male";
                else
                    gender = "Female";
            }


        }
        /// <summary>
        /// Produces a number min and max. Min = 0, max = 100, could produce 0..14..99.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private int RandomValue(int min, int max)
        {
            return _random.Next(min, max);
        }

    }
}
