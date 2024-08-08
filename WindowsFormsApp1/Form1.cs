using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        private string GameUrl = "steam://rungameid/3590";

        public Mods modmanager;
        public dynamic ModName_lab;

        public Form1()
        {
            InitializeComponent();
            modmanager = new Mods();
            
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            ModName_lab = ModName_label;

            ModName_lab.Text = modmanager.GetLoadedMod();

            foreach (var mod in modmanager.GetMods())
            {
                listBox1.Items.Add(mod);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = GameUrl,
                UseShellExecute = true
            });
        }

        private void ModName_label_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string current_mod_selected = listBox1.SelectedItem.ToString();
            if (current_mod_selected == null) return;
            modmanager.FullModLoad(current_mod_selected);
            ModName_lab.Text = modmanager.GetLoadedMod();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            modmanager.UpdateMods();
            listBox1.Items.Clear();
            foreach (var mod in modmanager.GetMods())
            {
                listBox1.Items.Add(mod);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", modmanager.Mods_folder);
        }
    }
}
