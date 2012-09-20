﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace NPCGenerator
{
    /// <summary>
    /// Interaction logic for NewNPCView.xaml
    /// </summary>
    public partial class NewNPCView : Window
    {
        private NPCViewModel _npcViewModel;

        internal NewNPCView(NPCViewModel npcViewModel)
        {
            InitializeComponent();
            this._npcViewModel = npcViewModel;
            SetDataContext();
        }

        private void SetDataContext()
        {
            DataContext = _npcViewModel;
            PossibleNameEthnicities_ListBox.ItemsSource = _npcViewModel.NameEthnicities;
            PossibleNameEthnicities_ListBox.SelectedIndex = 0;
            GeneratedNames_ListBox.ItemsSource = _npcViewModel.GeneratedRandomNames;
            Gender.ItemsSource = _npcViewModel.Genders;
            Gender.SelectedIndex = 0;
        }
        private void test100_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < 10000; x++)
            {
                String ethnicity = PossibleNameEthnicities_ListBox.SelectedItem.ToString();
                _npcViewModel.GenerateNPC(Gender.SelectedItem.ToString(), ethnicity, _npcViewModel.CurrentWorld);
                _npcViewModel.SaveCurrentNPC();
            }
        }

        private void NamesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //New to trigger the datagrid to change.
            if (GeneratedNames_ListBox.SelectedItem != null)
            {
                _npcViewModel.CurNPC.Traits[0] = new TraitLabelValue("Name", GeneratedNames_ListBox.SelectedItem.ToString());
            }
        }

        private void Generate_Button_Click(object sender, RoutedEventArgs e)
        {
            String ethnicity = PossibleNameEthnicities_ListBox.SelectedItem.ToString();
            _npcViewModel.GenerateNPC(Gender.SelectedItem.ToString(), ethnicity, _npcViewModel.CurrentWorld);
            NPC_Generated();
        }

        private void NPC_Generated()
        {
            NewNPC_DataGrid.ItemsSource = _npcViewModel.CurNPC.Traits;
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_npcViewModel != null)
            {
                _npcViewModel.SaveCurrentNPC();
                _wasSaved = true;
                this.Close();
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OpenWold_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text Files (.txt)|*.txt";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + "\\" + _npcViewModel.WorldDirectory;
            bool? userClickedOK = openFileDialog1.ShowDialog();
            if (userClickedOK == true)
            {
                String fileName = openFileDialog1.FileName;
                _npcViewModel.OpenWorldFromPath(fileName);
            }
        }

        private void WindowClosedEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!_wasSaved)
                _npcViewModel.CurNPC = null;
        }



        public bool _wasSaved { get; set; }
    }
}
