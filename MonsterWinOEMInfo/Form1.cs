using Microsoft.Win32;
using MonsterWinOEMInfo.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace MonsterWinOEMInfo
{
    public partial class Form1 : Form
    {
        const String IMAGE_PATH = @"C:\Windows\Web\monster_notebook.bmp";
        const String MANUFACTURER = "Monster Notebook";
        const String SUPPORT_HOURS = "Pazar günü hariç 09:00-19:00";
        const String SUPPORT_PHONE = "+90 850 255 11 11";
        const String SUPPORT_URL = "https://www.monsternotebook.com.tr/teknik-servis/";
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process WinSPro = new Process();
            ProcessStartInfo WinSSInfo = new ProcessStartInfo();
            WinSSInfo.FileName = @"c:\windows\system32\control.exe";
            WinSSInfo.Arguments = "/name Microsoft.System";
            WinSPro.StartInfo = WinSSInfo;
            WinSPro.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String notebookModel = textBox1.Text.Trim();
            OEMInformation(notebookModel);
        }
        private void OEMInformation(String model)
        {
            button1.Enabled = false;
            CopyLogo();
            SetOEMRegistry("Logo", IMAGE_PATH);
            SetOEMRegistry("Manufacturer", MANUFACTURER);
            if (!String.IsNullOrEmpty(model))  SetOEMRegistry("Model", model);
            SetOEMRegistry("SupportHours", SUPPORT_HOURS);
            SetOEMRegistry("SupportPhone", SUPPORT_PHONE);
            SetOEMRegistry("SupportURL", SUPPORT_URL);
            DialogResult dialog = new DialogResult();
            MessageBox.Show("Monster Notebook OEM Information has been installed to Windows.", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void DeleteOEMRegistries()
        {
            string keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation";
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
                {
                    if (key == null)
                    {
                    }
                    else
                    {
                        try
                        {
                            key.DeleteValue("Logo");
                            key.DeleteValue("Manufacturer");
                            key.DeleteValue("Model");
                            key.DeleteValue("SupportHours");
                            key.DeleteValue("SupportPhone");
                            key.DeleteValue("SupportURL");
                            key.DeleteValue("SupportPhone");
                            key.DeleteValue("SupportProvider");
                            key.DeleteValue("SupportAppURL");
                        }
                        catch(ArgumentException)
                        { }
                    }
                }
                MessageBox.Show("OEM Information has been reset.", "MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(SecurityException)
            {
                MessageBox.Show("Run the program as administrator", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SetOEMRegistry(String key, String value)
        {
            const string keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation";
            Registry.SetValue(keyName, key, value);
        }
        private void CopyLogo()
        {
            if (!File.Exists(IMAGE_PATH))
            {
                Image image = Resources.monster_notebook;
                byte[] arr = image.ToByteArray(ImageFormat.Bmp);
                File.WriteAllBytes(IMAGE_PATH, arr);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button1.Enabled = true;
            DeleteOEMRegistries();
            if (File.Exists(IMAGE_PATH))
            {
                File.Delete(IMAGE_PATH);
            }
            button3.Enabled = true;
        }
    }
    public static class ImageExtensions
    {
        public static byte[] ToByteArray(this Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }
    }
}
