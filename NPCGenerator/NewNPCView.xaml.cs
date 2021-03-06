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
    public partial class NewNPCView : NPCBaseWindow
    {
       

        internal NewNPCView(NPCViewModel npcViewModel)
        {
            InitializeComponent();
            this._npcViewModel = npcViewModel;


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

        private void Batch_Generate_NPCs(object sender, RoutedEventArgs e)
        {
            String quantity = Generation_Quantity_TestBox.Text;
            int Num;
            bool isNum = int.TryParse(quantity, out Num);
            if (isNum)
            {
                for (int x = 0; x < Num; x++)
                {
                    String ethnicity = PossibleNameEthnicities_ListBox.SelectedItem.ToString();
                    _npcViewModel.GenerateNPC(Gender.SelectedItem.ToString(), ethnicity, _npcViewModel.CurrentWorld);
                    String name = _npcViewModel.GeneratedRandomNames.FirstOrDefault();
                    if (name == null)
                    {
                        name = "NoName.";
                    }
                    _npcViewModel.CurNPC.Traits[0] = new TraitLabelValue("Name", name);
                    _npcViewModel.FinalizeCurrentNPC();
                }
            }
            else
                MessageBox.Show("Invalid number");
            
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
            GenerateNPC();
        }

        private void GenerateNPC()
        {
            String ethnicity = PossibleNameEthnicities_ListBox.SelectedItem.ToString();
            NPC producedNPC = _npcViewModel.GenerateNPC(Gender.SelectedItem.ToString(), ethnicity, _npcViewModel.CurrentWorld);
            if (String.IsNullOrWhiteSpace(_npcViewModel.ErrorMessage))
            {
                NPC_Generated();
                if (GeneratedNames_ListBox.Items.Count > 0)
                    GeneratedNames_ListBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Error occurred during generation - " + _npcViewModel.ErrorMessage);
                this.Close();
            }
        }

        private void NPC_Generated()
        {
            NewNPC_DataGrid.ItemsSource = _npcViewModel.CurNPC.Traits;
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            _wasSaved = true;
            ExecuteSave();
            Close();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void WindowClosedEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (!_wasSaved)
            {
                System.Windows.Forms.DialogResult result1 = System.Windows.Forms.MessageBox.Show("Save the current NPC?",
               "Save " + _npcViewModel.GeneratedRandomNames.FirstOrDefault(),
               System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result1 == System.Windows.Forms.DialogResult.Yes)
                    ExecuteSave();
                if (!_wasSaved)
                    _npcViewModel.CurNPC = null;
            }
        }



        public bool _wasSaved { get; set; }

        private void NewNPC_DataGrid_TextSelection(object sender, SelectedCellsChangedEventArgs e)
        {
            if(NewNPC_DataGrid.SelectedCells.Count==2)
            {
                //Todo - auto select single cell
            }
        }


        protected override void ExecuteSave()
        {
            if (_npcViewModel != null)
            {
                _npcViewModel.FinalizeCurrentNPC();
                _wasSaved = true;
            }
        }

        protected override void OpenNewWorldSuccessful()
        {
            NewNPC_DataGrid.ItemsSource = null;
            GenerateNPC();
        }

        private void OnWindowOpen(object sender, RoutedEventArgs e)
        {
            SetDataContext();
            GenerateNPC();
        }
    }

}
