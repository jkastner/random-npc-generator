using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace NPCGenerator
{
    internal class NPCViewModel
    {
        private static readonly Random _random = new Random();
        private readonly Dictionary<String, BroadTrait> _allTraits = new Dictionary<string, BroadTrait>();
        private readonly Dictionary<String, NameList> _names = new Dictionary<string, NameList>();
        private NPC _curNPC;
        private string _error = "";


        private string _firstNameFile = @"Data\Names\All First Names.txt";
        private ObservableCollection<String> _generatedRandomNames = new ObservableCollection<string>();
        private string _lastNameFile = @"Data\Names\All Last Names.txt";
        private ObservableCollection<String> _nameEthnicities = new ObservableCollection<string>();


        private ObservableCollection<NPC> _npcs = new ObservableCollection<NPC>();
        private string _traitDir = @"Data\Traits";
        private string _worldDir = @"Data\Worlds";
        public ObservableCollection<String> tempTraits = new ObservableCollection<string>();

        public NPCViewModel()
        {
            ProcessTraitFiles();
            ProcessWorldFiles();
            ReadNameFile();
        }

        public NPC CurNPC
        {
            get { return _curNPC; }
            set { _curNPC = value; }
        }

        public ObservableCollection<NPC> NPCs
        {
            get { return _npcs; }
            set { _npcs = value; }
        }

        public ObservableCollection<String> NameEthnicities
        {
            get { return _nameEthnicities; }
            set { _nameEthnicities = value; }
        }

        public ObservableCollection<String> GeneratedRandomNames
        {
            get { return _generatedRandomNames; }
            private set { _generatedRandomNames = value; }
        }

        private void ProcessTraitFiles()
        {
            List<FileInfo> AllTraitFiles = GatherFiles(new DirectoryInfo(_traitDir)).ToList();
            ReadTraitFiles(AllTraitFiles);
        }

        private void ReadTraitFiles(List<FileInfo> AllTraitFiles)
        {
            foreach (FileInfo curFile in AllTraitFiles)
            {
                String traitName = curFile.Name.Split('.')[0].Trim();
                var newTrait = new BroadTrait(traitName);
                _allTraits.Add(traitName, newTrait);
                ReadTraitFile(newTrait, curFile);
            }
        }

        private void ReadTraitFile(BroadTrait newTrait, FileInfo curFile)
        {
            string[] lines = File.ReadAllLines(curFile.FullName);
            int maxTraitWeight = 0;
            var linkedValues = new Dictionary<string, int>();
            foreach (String theLine in lines)
            {
                String line = theLine.Trim();
                //Skip blank lines
                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                //# is comment mark
                if (line[0].Equals('#'))
                    continue;

                string[] splitLine = line.Split('\t');
                int traitWeight, currentTableWeight;
                String traitValue = splitLine[0];
                //Is a trait and weight
                if (splitLine.Length >= 2)
                {
                    if (String.IsNullOrWhiteSpace(splitLine[1]))
                        traitWeight = 1;
                    else
                        traitWeight = Int32.Parse(splitLine[1]);
                }
                    //Just trait
                else
                    traitWeight = 1;
                currentTableWeight = traitWeight + maxTraitWeight;
                maxTraitWeight += traitWeight;

                //Trait has a linked trait
                if (splitLine.Length >= 3)
                {
                    //Ventrue   1
                    for (int curAffectedIndex = 2; curAffectedIndex < splitLine.Length; curAffectedIndex++)
                    {
                        if (String.IsNullOrWhiteSpace(splitLine[curAffectedIndex]))
                            continue;
                        //Ventrue		Derangements,20 Status,10
                        String[] curLinked = splitLine[curAffectedIndex].Split(',');
                        linkedValues.Add(curLinked[0].Trim(), Int32.Parse(curLinked[1]));
                    }
                }
                newTrait.AddValue(traitValue, currentTableWeight, linkedValues);
            }
            newTrait.MaxWeight = maxTraitWeight;
        }

        private void ProcessWorldFiles()
        {
            List<FileInfo> AllWorldFiles = GatherFiles(new DirectoryInfo(_worldDir)).ToList();
            ReadWorldFiles(AllWorldFiles);
        }

        private IEnumerable<FileInfo> GatherFiles(DirectoryInfo directoryInfo)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;
            files = directoryInfo.GetFiles("*.*");
            if (files != null)
            {
                foreach (FileInfo fi in files)
                {
                    yield return fi;
                }
                subDirs = directoryInfo.GetDirectories();
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    GatherFiles(dirInfo);
                }
            }
        }

        private void ReadWorldFiles(List<FileInfo> AllWorldFiles)
        {
        }


        private void ReadNameFile()
        {
            NameEthnicities.Add("Weighted Random");
            NameEthnicities.Add("Random");
            NameEthnicities.Add("English");


            string[] names = File.ReadAllLines(_firstNameFile);
            AddToDictionary(names);
            names = File.ReadAllLines(_lastNameFile);
            AddToDictionary(names);
        }

        private void AddToDictionary(string[] readNames)
        {
            foreach (String curLine in readNames)
            {
                String[] lineInfo = curLine.Split('\t');
                String name, gender, ethnicity;
                ethnicity = lineInfo.Last();
                if (!_names.ContainsKey(ethnicity))
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
                        if (curGender.Trim().Length > 0)
                            _names[ethnicity].AddFirstName(name, curGender.Trim());
                    }
                }
                else //length is 2, Last Name
                {
                    name = lineInfo[0];
                    _names[ethnicity].AddLastName(name);
                }
                if (!NameEthnicities.Contains(ethnicity))
                    NameEthnicities.Add(ethnicity);
            }
        }


        internal NPC GenerateNPC(string gender, string ethnicity)
        {
            _error = "";
            GeneratedRandomNames.Clear();
            MakeRandomNames(ref gender, ref ethnicity);
            NameList matchingList = _names[ethnicity];
            if (!matchingList.FirstNames.ContainsKey(gender))
            {
                _error = "No gender match for ethnicity - " + ethnicity;
                return _curNPC;
            }
            var newNPC = new NPC();
            PopulateRandomTraits(newNPC);
            _curNPC = newNPC;
            return newNPC;
        }

        private void PopulateRandomTraits(NPC newNPC)
        {
            tempTraits.Clear();
            foreach (var curPair in _allTraits)
            {
                String traitLabel = curPair.Key;
                int rolled = RandomValue(1, curPair.Value.MaxWeight + 1);
                curPair.Value.TraitValues.Sort();
                foreach (SingleTraitValue curSingleTrait in curPair.Value.TraitValues)
                {
                    if (rolled <= curSingleTrait.TraitWeight)
                    {
                        newNPC.AddTrait(traitLabel, curSingleTrait.TraitValue);
                        break;
                    }
                }
            }
        }

        private void MakeRandomNames(ref string gender, ref string ethnicity)
        {
            FixRandom(ref gender, ref ethnicity);
            var possibleFirstNames = new List<String>(_names[ethnicity].FirstNames[gender]);
            var possibleLastNames = new List<String>(_names[ethnicity].LastNames);
            while (GeneratedRandomNames.Count < 10 && possibleFirstNames.Count > 0)
            {
                String firstName = possibleFirstNames[RandomValue(0, possibleFirstNames.Count)];
                String lastName = "";
                if (possibleLastNames.Count > 0)
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