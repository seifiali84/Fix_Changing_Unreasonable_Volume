using System.Data;
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

            Console.WriteLine("Microphone volume changed - Speakers set to Max Volume!"); // Play sound notification when speaker volumes are set at max.

        }
        private void button1_Click(object sender, EventArgs e)
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator(); // Create a new instance of the audio device enumerator 
            MMDevice mic = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications); // Get the default communication microphone

            mic.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification; // Subscribe to Volume Notification events
        }
        // add version tag
    }
}