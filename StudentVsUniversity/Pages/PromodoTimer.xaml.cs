using System;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.Notifications;
using Ninject;
using StudentVsUniversity.Helpers;
using StudentVsUniversity.Models;
using StudentVsUniversity.Popups;
using StudentVsUniversity.Services;
using StudentVsUniversity.Services.Interfaces;
using StudentVsUniversity.Settings;
using StudentVsUniversity.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StudentVsUniversity.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PromodoTimer : Page
    {
        #region Constants
        public static string SOUND_SETTINGS = "SOUNDS";
        public static string NOTIFICATION_SETTINGS = "NOTIFICATIONS";


        #endregion

        private IStorageService _service;
        private ActivityViewModel _viewModel;
        private Windows.Storage.ApplicationDataContainer _localSettings;
        private DispatcherTimer _timer;

        private bool _isClockRunning = false;
        private bool _isActiveStatus = true;

        public LocalStorageService Service => (LocalStorageService) _service;


        public PromodoTimer()
        {
            this.InitializeComponent();

            CoreApplication.Exiting += ApplicationExiting;
            _service = new LocalStorageService();
            _viewModel = new ActivityViewModel();
            SetUpActivities();
            SetupSettings();
            this.DataContext = _viewModel;

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += Timer_Tikk;

        }

        private void SetUpActivities()
        {
            _viewModel.Activities = _service.GetActivities().OrderBy(x => x.CreatedTime).ToList();
            _viewModel.SelectedActivity = _viewModel.Activities.FirstOrDefault();

            if (_viewModel.SelectedActivity != null)
            {
                RefreshEllapseLabels();
            }
            else
            {
                EllapsedTime.Text = String.Empty;
                EllapsedWorkTIme.Text = String.Empty;
                EllapsedRestTime.Text = String.Empty;
            }

            SelectedActivityHeader.Text = _viewModel.SelectedActivity == null ? "Add new activity to begin!" : String.Empty;

            TaskGrid.Visibility = _viewModel.SelectedActivity == null ? Visibility.Collapsed : Visibility.Visible;
            Details.Visibility = _viewModel.SelectedActivity == null ? Visibility.Collapsed : Visibility.Visible;
            ActivityListView.ItemsSource = _viewModel.Activities;

            if (_viewModel.Activities.Count > 0)
                ActivityListView.SelectedIndex = 0;
        }


        private void ActivityListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MappedActivity selectedActivity = (MappedActivity) (sender as ListView).SelectedItem;

            if (selectedActivity != null)
            {
                MainGrid.Background = new SolidColorBrush(selectedActivity.Color);
                ActivityNameHeader.Text = selectedActivity.Name;
                _viewModel.SelectedActivity = selectedActivity;


                MinuteTextBlock.Text = _viewModel.SelectedActivity.WorkTime.ToString();
                SecondTextBlock.Text = "00";
                RefreshEllapseLabels();
                _isActiveStatus = true;
                StartButton.Content = "Start!";
            }
        }

        #region Menu item handlers
        private void OpenNewActivityPopup(object sender, RoutedEventArgs e)
        {
            NewActivityPopup.IsOpen = true;
            NewActivityPopup.VerticalOffset = ActualHeight/2 - 800;
            NewActivityPopup.HorizontalOffset = ActualWidth/2 - 300;
        }
        private void OpenSettingsPopup(object sender, RoutedEventArgs e)
        {
            SettingsPopup.IsOpen = true;
            SettingsPopup.VerticalOffset = ActualHeight / 2 - 800;
            SettingsPopup.HorizontalOffset = ActualWidth / 2 - 175;
        }

        private void QuitButton_OnClick(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        #endregion

        #region Remote controls
        public void RefreshActivityList()
        {
            int index = ActivityListView.SelectedIndex;
            _viewModel.Activities = null;

            _viewModel.Activities = _service.GetActivities().OrderBy(x => x.CreatedTime).ToList();

            if (index == -1) index = 0;

            ActivityListView.ItemsSource = _viewModel.Activities;
            ActivityListView.SelectedItem = _viewModel.Activities[index];
            ActivityListView.SelectedIndex = index;
          

            TaskGrid.Visibility = Visibility.Visible;
            Details.Visibility = Visibility.Visible;
        }

        public void RefreshSettings(bool sounds, bool notifications)
        {
            _viewModel.StudentSettings.Sounds = sounds;
            _viewModel.StudentSettings.Notifications = notifications;
        }

        public StudentSettings GetSettings()
        {
            return _viewModel.StudentSettings;
        }
        #endregion

        #region Application helpers
        private void ApplicationExiting(object sender, object o)
        {
            _localSettings.Values[SOUND_SETTINGS] = _viewModel.StudentSettings.Sounds;
            _localSettings.Values[NOTIFICATION_SETTINGS] = _viewModel.StudentSettings.Notifications;
            // TODO: implement saving;
        }

        public void SetupSettings()
        {
            _localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            _viewModel.StudentSettings = new StudentSettings();

            try
            {
                _viewModel.StudentSettings.Sounds = (bool) _localSettings.Values[SOUND_SETTINGS];
                _viewModel.StudentSettings.Notifications = (bool) _localSettings.Values[NOTIFICATION_SETTINGS];
            }
            catch (Exception)
            {
                _viewModel.StudentSettings.Sounds = true;
                _viewModel.StudentSettings.Notifications = true;
            }
        }
        #endregion

        #region Timer helpers
        private async void Timer_Tikk(object sender, object e)
        {
            int minute = Convert.ToInt32(MinuteTextBlock.Text);
            int second = Convert.ToInt32(SecondTextBlock.Text);

            second--;
            if (minute <= 0 && second <= 0)
            {
                _timer.Stop();

                if(_viewModel.StudentSettings.Notifications)
                    ShowNotification();

                second = 0;
                MessageDialog dialog = new MessageDialog("Do you want to start next interval?");

                dialog.Commands.Add(new UICommand("Yes") { Id = 0 });
                dialog.Commands.Add(new UICommand("No") { Id = 1 });

                if (_isActiveStatus)
                {
                    _viewModel.SelectedActivity.EllapsedMinutes += _viewModel.SelectedActivity.WorkTime;
                    _viewModel.SelectedActivity.EllapsedWorkTime += _viewModel.SelectedActivity.WorkTime;
                }
                else
                {
                    _viewModel.SelectedActivity.EllapsedMinutes += _viewModel.SelectedActivity.RestTime;
                    _viewModel.SelectedActivity.EllapsedRestTime += _viewModel.SelectedActivity.RestTime;
                }

                SaveSelectedActivity();

                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                IUICommand result = await dialog.ShowAsync();

                int id = Convert.ToInt32(result.Id);

                if (id == 0)
                {
                    _isActiveStatus = !_isActiveStatus;
                    minute = _isActiveStatus
                        ? _viewModel.SelectedActivity.WorkTime
                        : _viewModel.SelectedActivity.RestTime;
                    _timer.Start();

                }
                else
                {
                    minute = _viewModel.SelectedActivity.WorkTime;
                    _isClockRunning = false;
                    _isActiveStatus = true;
                    StartButton.Content = "Start!";
                }

            } else if (second <= 0)
            {
                second = 59;
                minute--;
            }

            RestTextBlock.Visibility = _isActiveStatus ? Visibility.Collapsed : Visibility.Visible;
            MinuteTextBlock.Text = minute.ToString();
            SecondTextBlock.Text = second.ToString();
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            ActivityListView.IsHitTestVisible = false;
            if (!_isClockRunning)
            {
                StartButton.Content = "Pause!";
                _timer.Start();
                _isClockRunning = true;
            }
            else
            {
                StartButton.Content = "Start!";
                _isClockRunning = false;
                _timer.Stop();

            }
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            RestTextBlock.Visibility = Visibility.Collapsed;

            int eMinute = Convert.ToInt32(MinuteTextBlock.Text);
            int workMin = _viewModel.SelectedActivity.WorkTime;

            int workedTime = workMin - eMinute;
            _viewModel.SelectedActivity.EllapsedWorkTime += workedTime;
            _viewModel.SelectedActivity.EllapsedMinutes += workedTime;
            

            _isClockRunning = false;
            ActivityListView.IsHitTestVisible = true;
            StartButton.Content = "Start!";

            MinuteTextBlock.Text = _viewModel.SelectedActivity.WorkTime.ToString();
            SecondTextBlock.Text = "00";

            SaveSelectedActivity();
        }
        #endregion

        #region Save finished activity

        private void SaveSelectedActivity()
        {
            _service = new LocalStorageService();
            _service.ModifActivity(_viewModel.SelectedActivity);

            RefreshEllapseLabels();
        }

        private void RefreshEllapseLabels()
        {
            EllapsedTime.Text = _viewModel.SelectedActivity.EllapsedMinutes + " min";
            EllapsedWorkTIme.Text = _viewModel.SelectedActivity.EllapsedWorkTime + " min";
            EllapsedRestTime.Text = _viewModel.SelectedActivity.EllapsedRestTime + " min";
        }

        #endregion

        #region Notification

        private void ShowNotification()
        {
            ToastContent content = new ToastContent()
            {
                Launch = "app-defined-string",

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Student vs University"
                            },

                            new AdaptiveText()
                            {
                                Text = "Interval ended, check the app to continue!"
                            }
                        }
                    }
                },
                Audio = new ToastAudio()
                {
                    Silent = true
                }
            };

            if (_viewModel.StudentSettings.Sounds)
            {
                content.Audio = new ToastAudio()
                {
                    Src = new Uri("ms-winsoundevent:Notification.Reminder"),
                    Silent = false
                };
            }

            Show(content);
        }


        private void Show(INotificationContent content)
        {
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(content.GetXml()));
        }

        #endregion
    }
}
