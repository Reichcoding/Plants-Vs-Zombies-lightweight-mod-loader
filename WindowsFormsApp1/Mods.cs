using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Mods
    {
        public bool Folder_ok = false;
        public string Mods_folder;
        public string Config_file;
        public string CurrentDirectory;
        public List<string> mods_list;
        public Mods()
        {
            CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Mods_folder = Path.Combine(CurrentDirectory, "mods");
            Config_file = Path.Combine(CurrentDirectory, "loaded_mod.txt");
            if (!Directory.Exists(Mods_folder)) runFirstInit();

            Folder_ok = true;

            UpdateMods();



            if (File.ReadAllText(Config_file) == "") {
                LoadMod("ORIGINAL");
            }
        }

        public void runFirstInit() {
            if (!File.Exists(Path.Combine(CurrentDirectory, "PlantsVsZombies.exe")))
            {
                MessageBox.Show("PlantsVsZombies.exe не обнаружен");
                Application.Exit();
                return;
            }


            Directory.CreateDirectory(Mods_folder);

            string NoModFolder = Path.Combine(Mods_folder, "ORIGINAL");
            Directory.CreateDirectory(NoModFolder);

            CopyFromTo(CurrentDirectory, NoModFolder);
            MessageBox.Show("После первоначальной настройки. Требуется перезапуск");
            UpdateMods();
            if (!File.Exists(Config_file)) {
                File.Create(Config_file);
                LoadMod("ORIGINAL");
            }

        }

        public void UpdateMods()
        {
            string[] mods = Directory.GetDirectories(Mods_folder);

            for (int i = 0; i < mods.Length; i++) {
                mods[i] = Path.GetFileName(mods[i]);
            }


            mods_list = new List<string>(mods);
            
        }

        public List<string> GetMods() { 
            return mods_list;
        }
        public string GetLoadedMod() {
            try
            {
                return File.ReadAllText(Config_file);
            }
            catch
            {
                return "Cannot found";
            }
        }

        private void LoadMod(string Mod_name) {
            if (mods_list.IndexOf(Mod_name) != -1)
            {
                File.WriteAllText(Config_file, Mod_name);
            }
        }

        public void FullModLoad(string Mod_name) {
            
            foreach (string file in Directory.GetFiles(CurrentDirectory))
            {
                string fileName = Path.GetFileName(file);
                if (skipFiles.Contains(fileName)) continue;
                File.Delete(file);
            }
            foreach (string subDirectory in Directory.GetDirectories(CurrentDirectory))
            {
                string dirName = Path.GetFileName(subDirectory);
                if (skipFiles.Contains(dirName)) continue;
                Directory.Delete(subDirectory, true);
            }

            

            CopyFromTo(Path.Combine(Mods_folder, Mod_name), CurrentDirectory);

            LoadMod(Mod_name);


        }

        //public void SaveOGFiles() {}



        static string[] skipFiles = { "mods", "loaded_mod.txt", "ModLoader.exe", "WindowsFormsApp1.exe" };
        private static void CopyFromTo(string source, string destination) {
            string sourceDirectory = source;
            string targetDirectory = destination;
            try
            {
                CopyAllFiles(sourceDirectory, targetDirectory);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при копировании: {ex.Message}");
            }
        }
        private static void CopyAllFiles(string sourceDirName, string destDirName)
        {
            Directory.CreateDirectory(destDirName);
            foreach (string file in Directory.GetFiles(sourceDirName))
            {
                string fileName = Path.GetFileName(file);
                if (skipFiles.Contains(fileName)) continue;
                string destFile = Path.Combine(destDirName, fileName);
                File.Copy(file, destFile, true);
            }
            foreach (string subDirectory in Directory.GetDirectories(sourceDirName))

            {
                string dirName = Path.GetFileName(subDirectory);
                if (skipFiles.Contains(dirName)) continue;
                string destSubDir = Path.Combine(destDirName, dirName);
                CopyAllFiles(subDirectory, destSubDir);
            }
        }
    }
}
