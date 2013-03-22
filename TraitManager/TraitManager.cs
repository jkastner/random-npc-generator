using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPCGenerator;

namespace TraitManagerProject
{
    public class TraitManager
    {

        private ObservableCollection<TraitFileContents> _traits = new ObservableCollection<TraitFileContents>();

        public ObservableCollection<TraitFileContents> Traits
        {
            get { return _traits; }
            set { _traits = value; }
        }
        


        internal void PopulateInfo()
        {
            List<FileInfo> foundFiles = new List<FileInfo>();
            GatherFiles(new DirectoryInfo(NPCViewModel.TraitDirectory), foundFiles);
            foreach (var cur in foundFiles)
            {
                TraitCategory nc = ReadTrait(cur);
            }
        }

        private TraitCategory ReadTrait(FileInfo cur)
        {
            throw new NotImplementedException();
        }


        private void GatherFiles(DirectoryInfo directoryInfo, List<FileInfo> foundFiles)
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
    }
}
