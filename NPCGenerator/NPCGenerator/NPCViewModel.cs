using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NPCGenerator
{
    class NPCViewModel :INotifyPropertyChanged
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
        string _worldDir =  @"Data\Worlds";
        string _traitDir = @"Data\Traits";
        string _error = "";
        private String _currentEthnicityAndGender;

        public String CurrentEthnicityAndGender
        {
            get { return _currentEthnicityAndGender; }
            set 
            { 
                _currentEthnicityAndGender = value;
                OnPropertyChanged("CurrentEthnicityAndGender");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


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
        private ObservableCollection<String> _genders = new ObservableCollection<String>();
        public ObservableCollection<String> Genders
        {
            get { return _genders; }
            set { _genders = value; }
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
            ReadNameFile();
            ProcessTraitFiles();
            ProcessWorldFiles();
        }

        private void ProcessTraitFiles()
        {
            List<FileInfo> AllTraitFiles = GatherFiles(new DirectoryInfo(_traitDir)).ToList();
            ReadTraitFiles(AllTraitFiles);

        }



        private Dictionary<String, BroadTrait> _allTraits = new Dictionary<string, BroadTrait>();
        private Dictionary<string, World> _allWorlds = new Dictionary<string, World>();
        private void ReadTraitFiles(List<FileInfo> AllTraitFiles)
        {
            foreach (FileInfo curFile in AllTraitFiles)
            {
                String traitName = curFile.Name.Split('.')[0].Trim();
                BroadTrait newTrait = new BroadTrait(traitName);
                _allTraits.Add(traitName, newTrait);
                ReadTraitFile(newTrait, curFile);
            }
        }

        private void ReadTraitFile(BroadTrait newTrait, FileInfo curFile)
        {
            string[] lines = System.IO.File.ReadAllLines(curFile.FullName);
            int maxTraitWeight = 0;
            foreach (String theLine in lines)
            {
                Dictionary<String, int> linkedValues = new Dictionary<string, int>();
                String line = theLine.Trim();
                //Skip blank lines
                if(String.IsNullOrWhiteSpace(line))
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
                    if(String.IsNullOrWhiteSpace(splitLine[1]))
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
                        if(String.IsNullOrWhiteSpace(splitLine[curAffectedIndex]))
                            continue;
                        //Ventrue		Derangements,20 Status,10
                        String [] curLinked = splitLine[curAffectedIndex].Split(',');
                        linkedValues.Add(curLinked[0].Trim(), Int32.Parse(curLinked[1]));
                    }
                }
                newTrait.AddValue(traitValue, currentTableWeight, linkedValues);
            }
            newTrait.MaxWeight = maxTraitWeight;
        }
        void ProcessWorldFiles()
        {
            List<FileInfo> AllWorldFiles = GatherFiles(new DirectoryInfo(_worldDir)).ToList();
            ReadWorldFiles(AllWorldFiles);
        }

        private IEnumerable<FileInfo> GatherFiles(DirectoryInfo directoryInfo)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;
            files = directoryInfo.GetFiles("*.*");
            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    yield return fi;
                }
                subDirs = directoryInfo.GetDirectories();
                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    GatherFiles(dirInfo);
                }
            }
        }

        private void ReadWorldFiles(List<FileInfo> AllWorldFiles)
        {
            foreach (FileInfo curFile in AllWorldFiles)
            {
                String worldName = curFile.Name.Split('.')[0].Trim();
                World newWorld = new World(worldName);
                WorldNames.Add(worldName);
                ReadWorldFile(worldName, curFile);
            }
        }



        const String RandomNameDistribution = "Random Name Distribution:";
        const String TraitFiles = "Trait Files:";
        const String OutputFile = "Output File:";
        const String OutputOrder = "Output Order:";
        enum ReadingType { None, TraitFiles, RandomNameDistribution, OutputFile, OutputOrder };
        private void ReadWorldFile(string worldName, FileInfo curFile)
        {
            string[] lines = System.IO.File.ReadAllLines(curFile.FullName);
            ReadingType curReading = ReadingType.None;
            World curWorld = new World(worldName);
            int maxNameWeight = 0;
            for (int curIndex = 0; curIndex < lines.Length; curIndex++)
            {
                String line = lines[curIndex].Trim();
                //Skip blank lines
                if (String.IsNullOrWhiteSpace(line))
                    continue;
                //# is comment mark
                if (line[0].Equals('#'))
                    continue;
                switch (line)
                {
                    case TraitFiles:
                        curReading = ReadingType.TraitFiles;
                        continue;
                    case RandomNameDistribution:
                        curReading = ReadingType.RandomNameDistribution;
                        continue;
                    case OutputFile:
                        curReading = ReadingType.OutputFile;
                        continue;
                    case OutputOrder:
                        curReading = ReadingType.OutputOrder;
                        continue;
                }
                switch (curReading)
                {
                    case ReadingType.RandomNameDistribution:
                        string[] splitLine = line.Split('\t');
                        int curNameWeight, currentTableWeight;


                        if (splitLine.Length == 1)
                        {
                            maxNameWeight++;
                            curNameWeight = 1;
                        }
                        else
                        {
                            curNameWeight = Int32.Parse(splitLine[1]);
                        }
                        currentTableWeight = curNameWeight + maxNameWeight;
                        curWorld.AddNameWeight(splitLine[0], currentTableWeight);
                        maxNameWeight += curNameWeight;
                        break;
                    case ReadingType.OutputFile:
                        curWorld.OutputFile = line;
                        break;
                    case ReadingType.OutputOrder:
                        curWorld.OutputOrder = line.Split('\t').ToList();
                        break;
                    case ReadingType.TraitFiles:
                        String traitWithoutExtension = line.Split('.')[0].Trim();
                        curWorld.AddTrait(traitWithoutExtension);
                        break;
                }
            }
            curWorld.MaxNameWeight = maxNameWeight;
            if (String.IsNullOrWhiteSpace(curWorld.OutputFile))
            {
                curWorld.OutputFile = worldName + "_Created.txt";
            }
            if (curWorld.NameWeightDistribution.Count == 0)
            {
                for(int curIndex = 0;curIndex<NameEthnicities.Count;curIndex++)
                    curWorld.AddNameWeight(NameEthnicities[curIndex], curIndex+1);
                curWorld.MaxNameWeight = NameEthnicities.Count;
            }
            if (curWorld.AssociatedTraits.Count == 0)
            {
                foreach (KeyValuePair<String, BroadTrait> curPair in _allTraits)
                {
                    curWorld.AddTrait(curPair.Key);
                }
            }
            if (curWorld.OutputOrder.Count == 0)
            {
                curWorld.OutputOrder.Add("Name");
                curWorld.OutputOrder.Add("Note");
                curWorld.OutputOrder.Add("Gender");
            }
            _allWorlds.Add(worldName, curWorld);
        }


        private void ReadNameFile()
        {
            NameEthnicities.Add("Weighted Random");
            NameEthnicities.Add("Random");
            NameEthnicities.Add("English");
            
            
            string[] names = System.IO.File.ReadAllLines(_firstNameFile);
            ReadNamesToDictionary(names);
            names = System.IO.File.ReadAllLines(_lastNameFile);
            ReadNamesToDictionary(names);
            Genders.Insert(0, "Random");
            
        }

        private void ReadNamesToDictionary(string[] readNames)
        {
            foreach (String curLine in readNames)
            {
                String[] lineInfo = curLine.Split('\t');
                String name, gender;
                List<String> ethnicitiesAssociatedWithThisName = lineInfo.Last().Split(',').ToList();
                for (int index = ethnicitiesAssociatedWithThisName.Count-1; index >= 0; index--)
                {
                    ethnicitiesAssociatedWithThisName[index] = ethnicitiesAssociatedWithThisName[index].Trim();
                    if (String.IsNullOrWhiteSpace(ethnicitiesAssociatedWithThisName[index]))
                        ethnicitiesAssociatedWithThisName.RemoveAt(index);
                }

                foreach (string curEthnicity in ethnicitiesAssociatedWithThisName)
                {
                    if (!_names.ContainsKey(curEthnicity))
                    {
                        _names.Add(curEthnicity.Trim(), new NameList(curEthnicity));
                    }
                }
                if (lineInfo.Length == 3)
                {
                    name = lineInfo[0];
                    gender = lineInfo[1];
                    String[] genderTypes = gender.Split(' ');
                    for(int curGenderIndex =0;curGenderIndex < genderTypes.Length;curGenderIndex++)
                    {
                        String curGender = genderTypes[curGenderIndex].Trim();
                        if (String.IsNullOrWhiteSpace(curGender))
                            continue;
                        if(!Genders.Contains(curGender))
                            Genders.Add(curGender);
                        foreach (string curEthnicity in ethnicitiesAssociatedWithThisName)
                        {
                            _names[curEthnicity].AddFirstName(name, curGender);
                        }
                    }
                    
                }
                else//length is 2, Last Name
                {
                    name = lineInfo[0];
                    foreach (string curEthnicity in ethnicitiesAssociatedWithThisName)
                    {
                        _names[curEthnicity].AddLastName(name);
                    }
                }
                foreach (string curEthnicity in ethnicitiesAssociatedWithThisName)
                {
                    if (!NameEthnicities.Contains(curEthnicity))
                        NameEthnicities.Add(curEthnicity);
                }
            }
        }




        internal NPC GenerateNPC(string gender, string ethnicity, String worldName)
        {
            _error = "";
            GeneratedRandomNames.Clear();
            NPC newNPC = new NPC();
            gender = FixRandomGender(gender);
            ethnicity = FixRandomEthnicity(gender, ethnicity, worldName);
            if (MakeRandomNames(gender, ethnicity, worldName))
            {
                CurrentEthnicityAndGender = gender+"--"+ethnicity;
                NameList matchingList = _names[ethnicity];
                if (!matchingList.FirstNames.ContainsKey(gender))
                {
                    _error = "No gender match for ethnicity - " + ethnicity;
                    return _curNPC;
                }
                PopulateRandomTraits(newNPC, worldName);
                newNPC.SetValueForLabel("Gender", gender);
                _curNPC = newNPC;
            }
            newNPC.WorldName = worldName;
            return newNPC;
        }

        private void PopulateRandomTraits(NPC newNPC, String worldName)
        {
            List<BroadTrait> worldTraits = new List<BroadTrait>();
            foreach (String associatedTrait in _allWorlds[worldName].AssociatedTraits)
            {
                worldTraits.Add(_allTraits[associatedTrait]);
                newNPC.AddTrait(associatedTrait, "");
            }
            foreach (BroadTrait curTrait in worldTraits)
            {
                RollTrait(curTrait, newNPC, true);

            }

            
        }

        private void RollTrait(BroadTrait curTrait, NPC newNPC, bool chainMod)
        {
            RollTrait(curTrait, newNPC, 0, chainMod);
        }

        
        private void RollTrait(BroadTrait curTrait, NPC newNPC, int modValue, bool chainMod)
        {
            int rolled = RandomValue(1, curTrait.MaxWeight + 1) + modValue;
            bool traitFound = false;
            foreach (ValueWeight curSingleTrait in curTrait.TraitValues)
            {
                if (rolled <= curSingleTrait.TraitWeight)
                {
                    traitFound = true;
                    if (curSingleTrait.LinkedValues.Count > 0)
                    {
                        if (chainMod)
                        {
                            ModifyLinkedTrait(newNPC, curSingleTrait);
                        }
                    }
                    newNPC.SetValueForLabel(curTrait.TraitName, curSingleTrait.TraitValue);
                    break;
                }
            }
            if (!traitFound)
            {
                newNPC.AddTrait(curTrait.TraitName, "No match found.");
            }
        }

        private void ModifyLinkedTrait(NPC newNPC, ValueWeight curSingleTrait)
        {
            foreach (KeyValuePair<string, int> curPair in curSingleTrait.LinkedValues)
            {
                String targetTrait = curPair.Key;
                int valueChange = curPair.Value;
                if (!_allTraits.ContainsKey(targetTrait))
                {
                    _error = _error + " linked trait " + targetTrait + " not found for base trait result roll " + curSingleTrait.TraitValue;
                    return;
                }
                BroadTrait affectedTrait = _allTraits[targetTrait];
                RollTrait(affectedTrait, newNPC, valueChange, false);
            }

        }

        private bool MakeRandomNames(string gender, string ethnicity, string curWorld)
        {
            if (!_names[ethnicity].FirstNames.ContainsKey(gender))
            {
                return false;
            }
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
            return true;
        }

        private string FixRandomEthnicity(string gender, string ethnicity, string worldID)
        {
            String foundEthnicity = ethnicity;
            if (ethnicity.Equals("Random"))
            {
                bool foundName = false;
                {
                    int randomNameNumber = RandomValue(0, _names.Count);
                    foundEthnicity = NameEthnicities[randomNameNumber];
                }
            }
            if (foundEthnicity.Equals("Weighted Random"))
            {
                World curWorld = _allWorlds[worldID];
                int rolled = RandomValue(0, curWorld.MaxNameWeight);
                foreach (ValueWeight curName in curWorld.NameWeightDistribution)
                {
                    if (rolled <= curName.TraitWeight)
                    {
                        foundEthnicity = curName.TraitValue;
                        break;
                    }
                }
            }
            if (foundEthnicity.Equals("Random") || foundEthnicity.Equals("Weighted Random"))
                return FixRandomEthnicity(gender, ethnicity, worldID);
            //If the user just wanted a random name but none matching was found, roll again.
            if ((ethnicity.Equals("Random") || ethnicity.Equals("Weighted Random"))&&
                (!_names[foundEthnicity].FirstNames.ContainsKey(gender)))
                    return FixRandomEthnicity(gender, ethnicity, worldID);
            return foundEthnicity;
        }

        private string FixRandomGender(string gender)
        {
            if (gender.Equals("Random"))
            {
                int randomGender = RandomValue(1, Genders.Count);
                return Genders[randomGender];
            }
            return gender;
        }

        /// <summary>
        /// Produces a number min and max. Min = 0, max = 100, could produce 0..14..99.
        /// </summary>
        private int RandomValue(int min, int max)
        {
            return _random.Next(min, max);
        }

        private ObservableCollection<String> _worldNames = new ObservableCollection<string>();
        private string _saveDir = "Output";
        public ObservableCollection<String> WorldNames 
        {
            get { return _worldNames; }
            set { _worldNames = value; }
        }

        internal void SaveCurrentNPC()
        {
            CurNPC.Saved = true;
            NPCs.Add(CurNPC);
            ObservableCollection<TraitLabelValue> finalNPCTraits = new ObservableCollection<TraitLabelValue>();
            World NPCWorld = _allWorlds[CurNPC.WorldName];
            //Add in order
            for (int curIndex = 0; curIndex < NPCWorld.OutputOrder.Count; curIndex++)
            {
                String curTraitLabel = NPCWorld.OutputOrder[curIndex];
                finalNPCTraits.Add(new TraitLabelValue(curTraitLabel, CurNPC.GetValueForLabel(curTraitLabel)));
            }
            //Add extra on the end.
            foreach (TraitLabelValue curTrait in CurNPC.Traits)
            {
                bool alreadyPresent = false;
                foreach (TraitLabelValue curCheck in finalNPCTraits)
                {
                    if(curCheck.Label.Equals(curTrait.Label))
                    {
                        alreadyPresent = true;
                        break;
                    }
                }
                if (!alreadyPresent)
                {
                    finalNPCTraits.Add(curTrait);
                }
            }
            CurNPC.Traits = finalNPCTraits;
            WriteOutNPC(CurNPC);
        }

        internal void WriteOutNPC(NPC CurNPC)
        {
            World curWorld = _allWorlds[CurNPC.WorldName];
            String outFileName = _saveDir + @"\" + curWorld.OutputFile;
            if (!Directory.Exists(_saveDir))
            {
                Directory.CreateDirectory(_saveDir);
            }
            StringBuilder outInfo = new StringBuilder();
            if (!File.Exists(outFileName))
            {
                foreach (String curLabel in curWorld.OutputOrder)
                {
                    outInfo.Append(curLabel + "\t");
                }
                outInfo.Append("\n");
            }
            foreach (TraitLabelValue curTrait in CurNPC.Traits)
            {
                outInfo.Append(curTrait.Value + "\t");
            }
            outInfo.Append("\n");
            using (StreamWriter outfile = new StreamWriter(outFileName, true))
            {
                outfile.Write(outInfo);
            }
            
        }
    }
}
