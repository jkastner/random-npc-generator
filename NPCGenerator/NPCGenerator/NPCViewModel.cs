﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace NPCGenerator
{
    public class NPCViewModel : INotifyPropertyChanged
    {
        private const String RandomNameDistribution = "Random Name Distribution:";
        private const String TraitFiles = "Trait Files:";
        private const String OutputFile = "Output File:";
        private const String OutputOrder = "Output Order:";
        private const String RegisteredRollers = "Registered Rollers:";
        
        public static readonly Random Randomize = new Random();
        private readonly Dictionary<String, BroadTrait> _allTraits = new Dictionary<string, BroadTrait>();
        private readonly Dictionary<string, World> _allWorlds = new Dictionary<string, World>();
        private readonly Dictionary<String, NameList> _names = new Dictionary<string, NameList>();
        private readonly Dictionary<String, BaseRegisteredRoller> _registeredRollers = new Dictionary<string, BaseRegisteredRoller>();
        
        private NPC _curNPC;
        private String _generatedResultMessage = "";
        private String _currentWorld;


        private string _firstNameFile = @"Data\Names\All First Names.txt";
        private ObservableCollection<String> _genders = new ObservableCollection<String>();
        private ObservableCollection<String> _generatedRandomNames = new ObservableCollection<string>();
        private string _lastNameFile = @"Data\Names\All Last Names.txt";
        private ObservableCollection<String> _nameEthnicities = new ObservableCollection<string>();
        private ObservableCollection<NPC> _npcs = new ObservableCollection<NPC>();
        private ObservableCollection<NPC> _resultNPCs = new ObservableCollection<NPC>();
        private string _saveDir = "Output";
        private string _traitDir = @"Data\Traits";
        private string _worldDirectory = @"Data\Worlds";
        private ObservableCollection<String> _worldNames = new ObservableCollection<string>();

        public NPCViewModel()
        {
            ReadNameFile();
            ProcessTraitFiles();
            ProcessWorldFiles();
            ProcessRegisterRollers();
        }

        public NPC CurNPC
        {
            get { return _curNPC; }
            set { _curNPC = value; }
        }

        public String GeneratedResultMessage
        {
            get { return _generatedResultMessage; }
            set
            {
                _generatedResultMessage = value;
                OnPropertyChanged("GeneratedResultMessage");
            }
        }

        public ObservableCollection<NPC> AllNPCs
        {
            get { return _npcs; }
            set { _npcs = value; }
        }

        public ObservableCollection<NPC> ResultNPCs
        {
            get { return _resultNPCs; }
            set { _resultNPCs = value; }
        }


        public ObservableCollection<String> NameEthnicities
        {
            get { return _nameEthnicities; }
            set { _nameEthnicities = value; }
        }

        public ObservableCollection<String> Genders
        {
            get { return _genders; }
            set { _genders = value; }
        }


        //Latin -  Male and Female  - Latin --> Bobicus, Tedimaximus

        public ObservableCollection<String> GeneratedRandomNames
        {
            get { return _generatedRandomNames; }
            private set { _generatedRandomNames = value; }
        }

        public ObservableCollection<String> WorldNames
        {
            get { return _worldNames; }
            set { _worldNames = value; }
        }

        public String CurrentWorld
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_currentWorld))
                    return _allWorlds.FirstOrDefault().Key;
                return _currentWorld;
            }
            set
            {
                _currentWorld = value;
                OnPropertyChanged("CurrentWorld");
            }
        }

     
        public string WorldDirectory
        {
            get { return _worldDirectory; }
            private set { _worldDirectory = value; }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private string _errorMessage;
        public String ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ProcessTraitFiles()
        {
            List<FileInfo> AllTraitFiles = new List<FileInfo>();
            GatherFiles(new DirectoryInfo(_traitDir), AllTraitFiles);
            ReadTraitFiles(AllTraitFiles);
        }

        private void ProcessRegisterRollers()
        {
            _registeredRollers.Add("VampireBloodlineGenerator", new VampireBloodlineGenerator());
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
            foreach (String theLine in lines)
            {
                try
                {
                    var linkedTableEdits = new Dictionary<string, int>();
                    var linkedTableEntryEdits = new Dictionary<string, List<ValueWeight>>();
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
                    //Daeva 10  EvilRating,SuperEvil,50 PrettyRating,SuperPretty,400
                    if (splitLine.Length >= 3)
                    {
                        //Ventrue   1
                        for (int curAffectedIndex = 2; curAffectedIndex < splitLine.Length; curAffectedIndex++)
                        {
                            if (String.IsNullOrWhiteSpace(splitLine[curAffectedIndex]))
                                continue;
                            
                            String[] curLinked = splitLine[curAffectedIndex].Split(',');
                            //Just modify a whole table
                            //Ventrue		Derangements,20 Status,10
                            if (curLinked.Length == 2)
                            {
                                linkedTableEdits.Add(curLinked[0].Trim(), Int32.Parse(curLinked[1]));
                            }
                            //Modify a specific entry on a table
                            //Strinking Looks   Clan,Daeva,5    Clan,Nosferatu,-5
                            else if (curLinked.Length == 3)
                            {
                                ValueWeight affectedTableEntry = new ValueWeight(curLinked[1].Trim(), Int32.Parse(curLinked[2]));
                                string affectedBroadTrait = curLinked[0].Trim();
                                if(linkedTableEntryEdits.ContainsKey(affectedBroadTrait))
                                {
                                    linkedTableEntryEdits[affectedBroadTrait].Add(affectedTableEntry);
                                }
                                else
                                {
                                    List<ValueWeight> affectedValues = new List<ValueWeight>();
                                    affectedValues.Add(affectedTableEntry);
                                    linkedTableEntryEdits.Add(curLinked[0].Trim(), affectedValues);
                                }
                            }
                            else
                            {
                                throw new DataNotFoundException("Error in line "+line+" of file "+curFile.Name+" - the linked trait must take the format of either 'Trait file affected, value change' or 'Trait file aFfected, trait affected, attribute value changed.");
                            }
                        }
                    }
                    newTrait.AddValue(traitValue, currentTableWeight, linkedTableEdits, linkedTableEntryEdits);
                    newTrait.MaxWeight = maxTraitWeight;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Improper data format - line " + theLine + " in file " + curFile);
                    //TODO
                    //I tried to just throw the exception, but it didn't exit, so I wasn't sure what the problem was.
                    //Hence the forced exit.
                    System.Environment.Exit(-1);
                }
            }
        }

        private void ProcessWorldFiles()
        {
            List<FileInfo> AllWorldFiles = new List<FileInfo>();
            GatherFiles(new DirectoryInfo(_worldDirectory), AllWorldFiles);
            ReadWorldFiles(AllWorldFiles);
        }

        private void GatherFiles(DirectoryInfo directoryInfo, List<FileInfo> foundFiles )
        {

            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;
            files = directoryInfo.GetFiles("*.*");
            if (files != null)
            {
                foreach (FileInfo fi in files)
                {
                    foundFiles.Add(fi);
                }
                subDirs = directoryInfo.GetDirectories();
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    GatherFiles(dirInfo, foundFiles);
                }
            }
        }

        private void ReadWorldFiles(List<FileInfo> AllWorldFiles)
        {
            foreach (FileInfo curFile in AllWorldFiles)
            {
                String worldName = curFile.Name.Split('.')[0].Trim();
                var newWorld = new World(worldName);
                WorldNames.Add(worldName);
                ReadWorldFile(worldName, curFile);
            }
        }


        private void ReadWorldFile(string worldName, FileInfo curFile)
        {
            string[] lines = File.ReadAllLines(curFile.FullName);
            var curReading = ReadingType.None;
            var curWorld = new World(worldName);
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
                    case RegisteredRollers:
                        curReading = ReadingType.RegisteredRollers;
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
                    case ReadingType.RegisteredRollers:
                        curWorld.AddRegisteredRoller(line);
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
                for (int curIndex = 0; curIndex < NameEthnicities.Count; curIndex++)
                    curWorld.AddNameWeight(NameEthnicities[curIndex], curIndex + 1);
                curWorld.MaxNameWeight = NameEthnicities.Count;
            }
            if (curWorld.AssociatedTraits.Count == 0)
            {
                foreach (var curPair in _allTraits)
                {
                    curWorld.AddTrait(curPair.Key);
                }
            }
            if (curWorld.OutputOrder.Count == 0)
            {
                curWorld.OutputOrder.Add("Name");
                curWorld.OutputOrder.Add("Note");
                curWorld.OutputOrder.Add("Gender");
                foreach (String curTrait in curWorld.AssociatedTraits)
                {
                    curWorld.OutputOrder.Add(curTrait);
                }
            }
            _allWorlds.Add(worldName, curWorld);
        }


        private void ReadNameFile()
        {
            NameEthnicities.Add("Weighted Random");
            NameEthnicities.Add("Random");
            NameEthnicities.Add("English");

            var tempEthnicityStorageForSorting = new List<string>();
            string[] names = File.ReadAllLines(_firstNameFile);
            ReadNamesToDictionary(names, tempEthnicityStorageForSorting);
            names = File.ReadAllLines(_lastNameFile);
            ReadNamesToDictionary(names, tempEthnicityStorageForSorting);
            tempEthnicityStorageForSorting.Sort();
            foreach (String curEth in tempEthnicityStorageForSorting)
                NameEthnicities.Add(curEth);
            Genders.Insert(0, "Random");
        }

        private void ReadNamesToDictionary(string[] readNames, List<String> tempEthnicityStorageForSorting)
        {
            foreach (String curLine in readNames)
            {
                String[] lineInfo = curLine.Split('\t');
                String name, gender;
                List<String> ethnicitiesAssociatedWithThisName = lineInfo.Last().Split(',').ToList();
                for (int index = ethnicitiesAssociatedWithThisName.Count - 1; index >= 0; index--)
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
                    for (int curGenderIndex = 0; curGenderIndex < genderTypes.Length; curGenderIndex++)
                    {
                        String curGender = genderTypes[curGenderIndex].Trim();
                        if (String.IsNullOrWhiteSpace(curGender))
                            continue;
                        if (!Genders.Contains(curGender))
                            Genders.Add(curGender);
                        foreach (string curEthnicity in ethnicitiesAssociatedWithThisName)
                        {
                            _names[curEthnicity].AddFirstName(name, curGender);
                        }
                    }
                }
                else //length is 2, Last Name
                {
                    name = lineInfo[0];
                    foreach (string curEthnicity in ethnicitiesAssociatedWithThisName)
                    {
                        _names[curEthnicity].AddLastName(name);
                    }
                }
                foreach (string curEthnicity in ethnicitiesAssociatedWithThisName)
                {
                    if (!tempEthnicityStorageForSorting.Contains(curEthnicity))
                        tempEthnicityStorageForSorting.Add(curEthnicity);
                }
            }
        }


        internal NPC GenerateNPC(string gender, string ethnicity, String worldName)
        {
            var newNPC = new NPC();
            Dictionary<String, List<int>> weightBackup = new Dictionary<string, List<int>>();
            Dictionary<String, int> maxWeightsBackup = new Dictionary<string, int>();
            //Since the weight of each trait can be altered multiple times during NPC generation, 
            //they are 'backed up' first.
            foreach (var cur in _allTraits)
            {
                weightBackup.Add(cur.Key, cur.Value.OriginalTraitWeights());
                maxWeightsBackup.Add(cur.Key, cur.Value.MaxWeight);
            }

            try
            {
                GeneratedResultMessage = "";
                GeneratedRandomNames.Clear();
                gender = FixRandomGender(gender);
                ethnicity = FixRandomEthnicity(gender, ethnicity, worldName);
                if (MakeRandomNames(gender, ethnicity, worldName))
                {
                    GeneratedResultMessage = gender + "--" + ethnicity;
                    NameList matchingList = _names[ethnicity];
                    PopulateRandomTraits(newNPC, worldName);
                    newNPC.SetValueForLabel("Gender", gender);
                    RunRegisteredRollers(newNPC, worldName);
                    _curNPC = newNPC;
                }
                else
                {
                    GeneratedResultMessage = "No match for\nGender: " + gender + "\nEthnicity: " + ethnicity;
                    return null;
                }
                newNPC.WorldName = worldName;
            }
            catch (NPCGeneratorExceptions e)
            {
                ErrorMessage = e.UserMessage;
            }
            finally
            {
                //Restore the original weights and values to each broad trait.
                foreach (var curOriginalWeight in weightBackup)
                {
                    List<int> originalWeights = curOriginalWeight.Value;
                    _allTraits[curOriginalWeight.Key].MaxWeight = maxWeightsBackup[curOriginalWeight.Key];
                    for (int curIndex = 0; curIndex < originalWeights.Count; curIndex++)
                    {
                        _allTraits[curOriginalWeight.Key].TraitValues[curIndex].TraitWeight = originalWeights[curIndex];
                    }
                }
            }
            return newNPC;
        }

        private void RunRegisteredRollers(NPC newNPC, string worldName)
        {
            var curWorld = _allWorlds[worldName];
            foreach (var curRollerName in curWorld.RegisteredRollers)
            {
                if (_registeredRollers.ContainsKey(curRollerName))
                {
                    var curRoller = _registeredRollers[curRollerName];
                    curRoller.Run(newNPC);
                }
            }
        }

         private void PopulateRandomTraits(NPC newNPC, String worldName)
        {
            var worldTraits = new List<BroadTrait>();
            foreach (String associatedTrait in _allWorlds[worldName].AssociatedTraits)
            {
                if (!_allTraits.ContainsKey(associatedTrait))
                {
                    throw new DataNotFoundException(worldName+" requested trait "+associatedTrait+" but the trait file was not found.");
                }
                worldTraits.Add(_allTraits[associatedTrait]);
                newNPC.AddTrait(associatedTrait, "");
            }
            foreach (BroadTrait curTrait in worldTraits)
            {
                RollTrait(curTrait, newNPC, true, true);
            }
        }


        private void RollTrait(BroadTrait curTrait, NPC newNPC, bool chainMod, bool overwriteExisting)
        {
            int rolled = RandomValue(1, curTrait.MaxWeight + 1);
            //If off the charts, use the last item on the chart
            if (rolled > curTrait.MaxWeight)
            {
                newNPC.SetValueForLabel(curTrait.TraitName, curTrait.TraitValues.LastOrDefault().TraitValue);
            }
            else
            {
                foreach (ValueWeight curSingleTrait in curTrait.TraitValues)
                {
                    if (rolled <= curSingleTrait.TraitWeight)
                    {
                        if (curSingleTrait.LinkedValues.Count > 0)
                        {
                            if (chainMod)
                            {
                                ModifyLinkedTrait(newNPC, curSingleTrait);
                            }
                        }
                        if (curSingleTrait.LinkedTableEntryEdits.Count > 0)
                        {
                            if (chainMod)
                            {
                                ModifySingleTableEntry(newNPC, curSingleTrait);
                            }
                        }
                        if (overwriteExisting)
                            newNPC.SetValueForLabel(curTrait.TraitName, curSingleTrait.TraitValue);
                        else
                        {
                            if (!newNPC.HasValueForLabel(curTrait.TraitName))
                            {
                                newNPC.SetValueForLabel(curTrait.TraitName, curSingleTrait.TraitValue);
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void ModifySingleTableEntry(NPC newNPC, ValueWeight curSingleTrait)
        {
            foreach (var curPair in curSingleTrait.LinkedTableEntryEdits)
            {
                String targetTrait = curPair.Key;
                List<ValueWeight> valuesToChange = curPair.Value;
                if (!_allTraits.ContainsKey(targetTrait))
                {
                    throw new DataNotFoundException("Linked trait " + targetTrait + " not found for base trait result roll " + curSingleTrait.TraitValue);
                }
                BroadTrait affectedTrait = _allTraits[targetTrait];
                bool wasFound = false;
                foreach (ValueWeight curValueWeightChange in valuesToChange)
                {
                    foreach (ValueWeight curItem in affectedTrait.TraitValues)
                    {
                        if (curItem.TraitValue.Equals(curValueWeightChange.TraitValue))
                        {
                            curItem.TraitWeight += curValueWeightChange.TraitWeight;
                            affectedTrait.MaxWeight += curValueWeightChange.TraitWeight;
                            wasFound = true;
                        }
                    }
                    if (!wasFound)
                    {
                        throw new DataNotFoundException("Linked table entry - target value " + targetTrait + " was not found on Trait " + affectedTrait.TraitName);
                    }
                    //Roll the affected trait again, modified with the mod weight.
                    RollTrait(affectedTrait, newNPC, false, true);
                }
            }
        }

        private void ModifyLinkedTrait(NPC newNPC, ValueWeight curSingleTrait)
        {
            foreach (var curPair in curSingleTrait.LinkedValues)
            {
                String targetTrait = curPair.Key;
                int valueChange = curPair.Value;
                if (!_allTraits.ContainsKey(targetTrait))
                {
                   throw new DataNotFoundException("Linked trait " + targetTrait + " not found for base trait result roll " +curSingleTrait.TraitValue);
                }
                BroadTrait affectedTrait = _allTraits[targetTrait];
                //Decrease the affected weight by the mod value.
                //So if the trait was normally 0-100, with 0 being 'not on fire' and 100 being 'really on fire'
                //and it was modified by a modvalue of 50 when they rolled 'made of gasoline' under another trait
                //They would be instead rolling on a table from 0-50, with 0 being 'mostly on fire' and 50 being 'really on fire'.
                foreach (ValueWeight curTrait in affectedTrait.TraitValues)
                {
                    curTrait.TraitWeight -= valueChange;
                }
                affectedTrait.MaxWeight -= valueChange;
                //Roll the affected trait again, modified with the mod weight.
                RollTrait(affectedTrait, newNPC, false, true);
            }
        }

        private bool MakeRandomNames(string gender, string ethnicity, string curWorld)
        {
            if (!_names[ethnicity].FirstNames.ContainsKey(gender))
            {
                return false;
            }
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
            if ((ethnicity.Equals("Random") || ethnicity.Equals("Weighted Random")) &&
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
        internal static int RandomValue(int min, int max)
        {
            return Randomize.Next(min, max);
        }

        internal void FinalizeCurrentNPC()
        {
            ResultNPCs.Add(CurNPC);
            AllNPCs.Add(CurNPC);
            var finalNPCTraits = new ObservableCollection<TraitLabelValue>();
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
                    if (curCheck.Label.Equals(curTrait.Label))
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
        }

        internal void WriteOutNPCs(World curWorld, List<NPC> NPCList)
        {
            String outFileName = _saveDir + @"\" + curWorld.OutputFile;
            if (!Directory.Exists(_saveDir))
            {
                Directory.CreateDirectory(_saveDir);
            }
            var outInfo = new StringBuilder();
            if (!File.Exists(outFileName))
            {
                foreach (String curLabel in curWorld.OutputOrder)
                {
                    outInfo.Append(curLabel + "\t");
                }
                outInfo.Append("\n");
            }
            foreach (NPC curNPC in NPCList)
            {
                foreach (String curLabel in curWorld.OutputOrder)
                {
                    var curField = curNPC.GetValueForLabel(curLabel);
                    curField = curField.Replace("\t", "{TAB}");
                    curField = curField.Replace(Environment.NewLine, "{NEWLINE}");
                    outInfo.Append(curField + "\t");
                }
                outInfo.Append("\n");
            }
            using (var outfile = new StreamWriter(outFileName, true))
            {
                outfile.Write(outInfo);
            }
        }


        internal void WriteOutNPCs(IEnumerable<NPC> NPCList)
        {
            Dictionary<World, List<NPC>> npcsSortedByWorld = new Dictionary<World, List<NPC>>();
            foreach (NPC curNPC in NPCList)
            {
                World foundWorld = _allWorlds[curNPC.WorldName];
                if (!npcsSortedByWorld.ContainsKey(foundWorld))
                {
                    npcsSortedByWorld.Add(foundWorld, new List<NPC>());
                }
                npcsSortedByWorld[foundWorld].Add(curNPC);
            }
            foreach (KeyValuePair<World, List<NPC>> curPair in npcsSortedByWorld)
            {
                WriteOutNPCs(curPair.Key, curPair.Value);
            }
        }


        internal void OpenWorldFromPath(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            fileName = fileName.Replace(".txt", "");
            CurrentWorld = fileName;
            World matchingWorld = _allWorlds[CurrentWorld];
            OpenNPCFile(matchingWorld);
        }

        private void OpenNPCFile(World matchingWorld)
        {
            AllNPCs.Clear();
            ResultNPCs.Clear();
            if (!File.Exists(_saveDir + "\\" + matchingWorld.OutputFile))
                return;
            string[] lines = File.ReadAllLines(_saveDir + "\\" + matchingWorld.OutputFile);
            //Skip header line
            var readHeaders = new List<string>();
            for (int curIndex = 0; curIndex < lines.Length; curIndex++)
            {
                if (String.IsNullOrWhiteSpace(lines[curIndex]))
                    continue;
                String[] brokenLine = lines[curIndex].Split('\t');
                if (curIndex == 0)
                {
                    foreach (String curHeader in brokenLine)
                    {
                        if(!String.IsNullOrEmpty(curHeader))
                            readHeaders.Add(curHeader);
                    }
                }
                else
                {
                    var readNPC = new NPC();
                    foreach (String curHeader in readHeaders)
                    {
                        if (!readNPC.HasTraitWithLabel(curHeader))
                            readNPC.AddTrait(curHeader, "");
                    }
                    for (int curDataIndex = 0; curDataIndex < readHeaders.Count&&curDataIndex<brokenLine.Length; curDataIndex++)
                    {
                        readNPC.SetValueForLabel(readHeaders[curDataIndex], brokenLine[curDataIndex]);
                    }
                    readNPC.WorldName = matchingWorld.WorldName;
                    AllNPCs.Add(readNPC);
                }
            }
            SetResultsToAll();
        }

        internal void SearchNPCs(string searchInfo)
        {
            if (String.IsNullOrWhiteSpace(searchInfo))
            {
                SetResultsToAll();
                return;
            }
            searchInfo = searchInfo.ToLower().Trim();
            ResultNPCs.Clear();
            String[] searchTerms = searchInfo.Split(' ');
            foreach (NPC curNPC in AllNPCs)
            {
                bool isValidNPC = true;
                foreach (String curTerm in searchTerms)
                {
                    bool NPCHasCurTerm = false;
                    foreach (TraitLabelValue curTrait in curNPC.Traits)
                    {
                        if (curTrait.Value.ToLower().Contains(curTerm))
                        {
                            NPCHasCurTerm = true;
                            break;
                        }
                    }
                    isValidNPC &= NPCHasCurTerm;
                    if (!isValidNPC)
                    {
                        break;
                    }
                }
                if (isValidNPC)
                {
                    ResultNPCs.Add(curNPC);
                }
            }
        }

        private void SetResultsToAll()
        {
            if (ResultNPCs.Count != AllNPCs.Count)
                foreach (NPC curNPC in AllNPCs)
                {
                    ResultNPCs.Add(curNPC);
                }
        }

        internal void SaveAllNPCS()
        {
            if (File.Exists(_saveDir + @"\" + _allWorlds[CurrentWorld].OutputFile))
                File.Delete(_saveDir+@"\"+_allWorlds[CurrentWorld].OutputFile);
            WriteOutNPCs(AllNPCs);
        }



        #region Nested type: ReadingType

        private enum ReadingType
        {
            None,
            TraitFiles,
            RandomNameDistribution,
            OutputFile,
            OutputOrder,
            RegisteredRollers
        };

        #endregion
    }
}