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
        bool openedSuccessfully = false;
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
                SingleNPC_DataGrid.ItemsSource = _npcViewModel.CurNPC.Traits;
            else
                SingleNPC_DataGrid.SelectedItem = null;
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
            if (openedSuccessfully)
            {
                System.Windows.Forms.DialogResult result1 = System.Windows.Forms.MessageBox.Show("Save the character file?",
                       "Save " + _npcViewModel.CurrentWorld,
                       System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result1 == System.Windows.Forms.DialogResult.Yes)
                    ExecuteSave();
            }
        }
    }
}