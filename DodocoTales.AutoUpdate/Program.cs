using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DodocoTales.AutoUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo dir = new DirectoryInfo("update/DodocoTales");

            if (!dir.Exists)
            {
                MessageBox.Show("错误：未找到更新文件；请不要手动运行本更新程序", "嘟嘟可故事集 更新", MessageBoxButtons.OK);
                return;
            }
            var prols = Process.GetProcessesByName("DodocoTales");
            if(prols.Any())
            {
                foreach(var pro in prols)
                {
                    pro.Kill();
                    Thread.Sleep(1000);
                }
                foreach (var pro in prols)
                {
                    if (!pro.HasExited) pro.WaitForExit();
                    
                }
            }
            
            MoveFromDirectory(dir, new DirectoryInfo(Directory.GetCurrentDirectory()));

            try
            {
                if (File.Exists("update/DodocoTales.zip"))
                    File.Delete("update/DodocoTales.zip");
                Directory.Delete("update");
            }
            catch (Exception)
            {

            }
            Process.Start("DodocoTales.exe");
        }

        static void MoveFromDirectory(DirectoryInfo sourceDir,DirectoryInfo destDir)
        {
            foreach(var dir in sourceDir.GetDirectories())
            {
                var destl = destDir.GetDirectories(dir.Name);
                if (!destl.Any())
                {
                    dir.MoveTo(destDir.FullName + "/" + dir.Name);
                }
                else
                {
                    MoveFromDirectory(dir, destl[0]);
                }
            }
            foreach(var file in sourceDir.GetFiles())
            {
                var destl = destDir.GetFiles(file.Name);
                if (destl.Any())
                {
                    destl[0].Delete();
                }
                file.MoveTo(destDir.FullName + "/" + file.Name);
            }
            sourceDir.Delete();
        }
    }
}
