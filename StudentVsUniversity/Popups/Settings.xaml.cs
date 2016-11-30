using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using StudentVsUniversity.Pages;
using StudentVsUniversity.Settings;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace StudentVsUniversity.Popups
{
    public sealed partial class Settings : UserControl
    {
        private bool _soundIsOn;
        private bool _notificationsAreOn;

        private Popup _parentPopup;
        private PromodoTimer _parent;


        public Settings()
        {
            this.InitializeComponent();
        }

        private void SoundsCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            CheckBox sounds = sender as CheckBox;
            _soundIsOn = sounds.IsChecked.Value;
        }

        private void NotificationsCheckBoxChecked(object sender, TappedRoutedEventArgs e)
        {
            CheckBox notifications = sender as CheckBox;
            _notificationsAreOn = notifications.IsChecked.Value;
        }

        private void SaveSettings(object sender, TappedRoutedEventArgs e)
        {
            _parent.RefreshSettings(_soundIsOn, _notificationsAreOn);
            _parentPopup.IsOpen = false;
        }

        private async void SetupParentHierarchy(object sender, RoutedEventArgs e)
        {
            _parentPopup = Parent as Popup;
            Grid grid = _parentPopup.Parent as Grid;
            _parent = grid.Parent as PromodoTimer;

            await Task.Delay(TimeSpan.FromSeconds(0.5));
            StudentSettings settings = _parent.GetSettings();

            SoundsCheckBox.IsChecked = _soundIsOn = settings.Sounds;
            NotificationsCheckBox.IsChecked = _notificationsAreOn = settings.Notifications;
        }
    }
}
