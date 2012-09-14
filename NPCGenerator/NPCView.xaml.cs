using System;
using System.Collections.Generic;
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

namespace NPCGenerator
{
    /// <summary>
    /// Interaction logic for NPCView.xaml
    /// </summary>
    public partial class NPCView : Window
    {
        private NPCViewModel _npcViewModel;
        public NPCView()
        {
            InitializeComponent();
            SetData();
        }

        private void SetData()
        {
            try
            {
                Gender.SelectedIndex = 0;
                _npcViewModel = new NPCViewModel();
                this.DataContext = _npcViewModel;
                NPCList_ListBox.ItemsSource = _npcViewModel.NPCs;
                PossibleNameEthnicities_ListBox.ItemsSource = _npcViewModel.NameEthnicities;
                PossibleNameEthnicities_ListBox.SelectedIndex = 0;
                GeneratedNames_ListBox.ItemsSource = _npcViewModel.GeneratedRandomNames;
                NPC cur = NPCList_ListBox.SelectedItem as NPC;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error - "+e.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_npcViewModel.CurNPC.Traits[1].Value);
        }

        void NPCList_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            _npcViewModel.CurNPC = NPCList_ListBox.SelectedItem as NPC;
            SingleNPC_DataGrid.ItemsSource = _npcViewModel.CurNPC.Traits;
        }

        private void Generate_Button_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)Gender.SelectedItem;
            String gender = typeItem.Content.ToString();
            String ethnicity = PossibleNameEthnicities_ListBox.SelectedItem.ToString();
            _npcViewModel.GenerateNPC(gender, ethnicity); 

        }

    }
}
