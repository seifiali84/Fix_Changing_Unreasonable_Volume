using System.Data;
using Microsoft.Win32;
using NAudio.CoreAudioApi;

namespace Fix_Changing_Unreasonable_Volume
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            MMDevice speakers = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia); // Get default multimedia playback device

            speakers.AudioEndpointVolume.MasterVolumeLevelScalar = 1.0f; // Set speaker volume level to maximum (1.0f)

            Console.WriteLine("Microphone volume changed - Speakers set to Max Volume!");
             
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator(); // Create a new instance of the audio device enumerator 
            MMDevice mic = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications); // Get the default communication microphone

            mic.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification; // Subscribe to Volume Notification events
        }
        string path = "data/data.csv";
        private bool GetStartup()
        {
            string[] Lines = File.ReadAllLines(path);
            if (Lines[0].Split(',')[1] == "True")
                return true;
            else
                return false;
        }
        private void ChangeStartup(bool data)
        {
            string[] Lines = { "Startup," + data.ToString() };
            File.WriteAllLines(path, Lines);
        }
        bool programChange = false;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (programChange)
            {
                programChange = false;
            }
            else
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (checkBox1.Checked)
                {
                    DialogResult result = MessageBox.Show("Are you sure to make this Program Startup?", "Confirmation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        rk.SetValue(Application.ProductName, Application.ExecutablePath);
                    }
                    else
                    {
                        programChange = true;
                        checkBox1.Checked = false;
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show("do you need to Disable Startup this Program?", "Confirmation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        rk.DeleteValue(Application.ProductName, false);
                    }
                    else
                    {
                        programChange = true;
                        checkBox1.Checked = true;
                    }
                }
            }
        }
    }
}