using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage;
using AutoDL.Utilities;

namespace AutoDL.Models
{
    public sealed class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool HaveAccessForLocator
        {
            get
            {
                var task = Geolocator.RequestAccessAsync();
                task.AsTask().Wait();
                return task.GetResults() == GeolocationAccessStatus.Allowed;
            }
        }
        
        public bool FirstTimeToLaunch
        {
            get => Convert.ToBoolean(ProgramSettings.GetStringSetting(nameof(FirstTimeToLaunch)));
            set
            {
                ProgramSettings.SetStringSetting(nameof(FirstTimeToLaunch), value.ToString());
                OnPropertyChanged();
            }
        }

        public bool SwitcherEnabled
        {
            get => Convert.ToBoolean(ProgramSettings.GetStringSetting(nameof(SwitcherEnabled)));
            set
            {
                ProgramSettings.SetStringSetting(nameof(SwitcherEnabled), value.ToString());
                OnPropertyChanged();
            }
        }

        public TimeSpan DarkTimeStart
        {
            get
            {
                TimeSpan start;
                TimeSpan.TryParse(ProgramSettings.GetStringSetting(nameof(DarkTimeStart)), out start);
                return start;
            }
            set
            {
                ProgramSettings.SetStringSetting(nameof(DarkTimeStart), value.ToString());
                OnPropertyChanged();
            }
        }
        public TimeSpan DarkTimeEnd
        {
            get
            {
                TimeSpan start;
                TimeSpan.TryParse(ProgramSettings.GetStringSetting(nameof(DarkTimeEnd)), out start);
                return start;
            }
            set
            {
                ProgramSettings.SetStringSetting(nameof(DarkTimeEnd), value.ToString());
                OnPropertyChanged();
            }
        }

        public SettingsViewModel()
        {
            if (ProgramSettings.LocalSettings is null)
            {
                ProgramSettings.CreateSettings();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
