using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace NPCGenerator
{
    public abstract class NPCBaseWindow : Window
    {
        protected NPCViewModel _npcViewModel;
        public static RoutedCommand OpenWorldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public NPCBaseWindow()
        {
            OpenWorldCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
        }

        protected void OpenWorldCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenWold_MenuItem_Click(sender, e);
        }

        protected void SaveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ExecuteSave();
        }

        protected abstract void ExecuteSave();

        protected void OpenWold_MenuItem_Click(object sender, RoutedEventArgs e)
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
    }
}
