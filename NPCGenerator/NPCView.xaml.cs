using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        }

        private void SetData()
        {
            _npcViewModel = new NPCViewModel();
            NPCList_ListBox.ItemsSource = _npcViewModel.ResultNPCs;
            OpenWorld(_npcViewModel.WorldNames.FirstOrDefault());
        }

        private void NPCList_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            _npcViewModel.CurNPC = NPCList_ListBox.SelectedItem as NPC;
            if (_npcViewModel.CurNPC != null)
            {
                SingleNPC_DataGrid.ItemsSource = _npcViewModel.CurNPC.SortedDisplayTraits;
                NPCManagementGrid.DataContext = _npcViewModel.CurNPC;
                
            }
            else
            {
                SingleNPC_DataGrid.SelectedItem = null;
                NPCManagementGrid.DataContext = null;
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetData();
            DataContext = _npcViewModel;
            //EventManager.RegisterClassHandler(typeof (TextBox), PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyHandleMouseButton), true);
            EventManager.RegisterClassHandler(typeof(TextBox), GotKeyboardFocusEvent, new RoutedEventHandler(SearchBox_GotFocus), true);
            openedSuccessfully = true;
        }

        private void NewNPC_Button_Click(object sender, RoutedEventArgs e)
        {
            _newNPCView = new NewNPCView(_npcViewModel);
            _newNPCView.ShowDialog();
            if (_npcViewModel.CurNPC != null)
                SingleNPC_DataGrid.ItemsSource = _npcViewModel.CurNPC.Traits;
        }


        private void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void SearchBox_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_npcViewModel != null)
            {
                _npcViewModel.SearchNPCs(SearchBox_TextBox.Text);
                if (NPCList_ListBox.Items.Count > 0)
                {
                    NPCList_ListBox.SelectedIndex = 0;
                }
            }
        }

        private void SaveNPCS_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSave();
        }

        protected override void ExecuteSave()
        {
            _npcViewModel.SaveAllNPCS();
            MessageBox.Show("Save completed.");
        }

        protected override void OpenNewWorldSuccessful()
        {
            SingleNPC_DataGrid.ItemsSource = null;
        }


        private void SelectivelyHandleMouseButton(object sender, MouseButtonEventArgs e)
        {
            var textbox = (sender as TextBox);
            if (textbox != null && !textbox.IsKeyboardFocusWithin)
            {
                if (e.OriginalSource.GetType().Name == "TextBoxView")
                {
                    e.Handled = true;
                    textbox.Focus();
                }
            }
        }


        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null) textBox.SelectAll();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PromptToSave();
        }

        
        private void RandomSelection_Button_Click(object sender, RoutedEventArgs e)
        {
            if (NPCList_ListBox.Items.Count > 0)
            {
                int randomSelection =NPCViewModel.RandomValue(0, NPCList_ListBox.Items.Count);
                NPCList_ListBox.ScrollIntoView(NPCList_ListBox.Items[randomSelection]);
                NPCList_ListBox.SelectedIndex = randomSelection;
            }
        }
    }
}