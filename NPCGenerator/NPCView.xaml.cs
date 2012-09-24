using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace NPCGenerator
{
    /// <summary>
    /// Interaction logic for NPCView.xaml
    /// </summary>
    public partial class NPCView : NPCBaseWindow
    {
        internal NewNPCView _newNPCView;
        public NPCView()
        {
            InitializeComponent();
            SetData();
        }

        private void SetData()
        {
            try
            {
                _npcViewModel = new NPCViewModel();
                DataContext = _npcViewModel;
                NPCList_ListBox.ItemsSource = _npcViewModel.ResultNPCs;
                _npcViewModel.OpenWorldFromPath(_npcViewModel.WorldNames.FirstOrDefault());


            }
            catch (Exception e)
            {
                MessageBox.Show("Error - " + e.Message);
            }
        }

        void NPCList_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            _npcViewModel.CurNPC = NPCList_ListBox.SelectedItem as NPC;
            if (_npcViewModel.CurNPC != null)
                SingleNPC_DataGrid.ItemsSource = _npcViewModel.CurNPC.Traits;
            else
                SingleNPC_DataGrid.SelectedItem = null;
        }


        





        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _npcViewModel;
        }

        private void NewNPC_Button_Click(object sender, RoutedEventArgs e)
        {
            _newNPCView = new NewNPCView(_npcViewModel);
            _newNPCView.ShowDialog();
            if(_npcViewModel.CurNPC != null)
                SingleNPC_DataGrid.ItemsSource = _npcViewModel.CurNPC.Traits;
        }





        private void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }





        private void SearchBox_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(_npcViewModel != null)
                _npcViewModel.SearchNPCs(SearchBox_TextBox.Text);
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBox_TextBox.SelectAll();

        }

        private void SaveNPCS_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSave();
        }

        protected override void ExecuteSave()
        {
            _npcViewModel.SaveAllNPCS();
        }
    }
}
