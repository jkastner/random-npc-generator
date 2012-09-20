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
    public partial class NPCView : Window
    {
        private NPCViewModel _npcViewModel;
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
                NPCList_ListBox.ItemsSource = _npcViewModel.NPCs;


            }
            catch (Exception e)
            {
                MessageBox.Show("Error - " + e.Message);
            }
        }

        void NPCList_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            _npcViewModel.CurNPC = NPCList_ListBox.SelectedItem as NPC;
            SingleNPC_DataGrid.ItemsSource = _npcViewModel.CurNPC.Traits;
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

        private void NPCView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Open_MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenWold_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text Files (.txt)|*.txt";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory()+"\\"+_npcViewModel.WorldDirectory;
            bool? userClickedOK = openFileDialog1.ShowDialog();
            if (userClickedOK == true)
            {
                String fileName = openFileDialog1.FileName;
                _npcViewModel.OpenWorldFromPath(fileName);
            }
        }

        private void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



    }
}
