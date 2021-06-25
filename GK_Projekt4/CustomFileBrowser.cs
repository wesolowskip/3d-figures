using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GK_Projekt4
{
    class CustomFileBrowser : FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            openFileDialog.Title = "Wybierz obraz:";
            openFileDialog.Filter = "Pliki obrazów (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png";
            string CombinedPath = Path.Combine(Directory.GetCurrentDirectory(),
                @"..\..\Textures");
            openFileDialog.InitialDirectory = Path.GetFullPath(CombinedPath);
        }
    }
}
