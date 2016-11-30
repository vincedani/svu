using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors;
using StudentVsUniversity.Models;
using StudentVsUniversity.Pages;
using StudentVsUniversity.Services;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace StudentVsUniversity.Popups
{
    public sealed partial class NewActivity : UserControl
    {
        private Color? _color;
        private LocalStorageService _service;
        private PromodoTimer _parent;
        private Popup _parentPopup;

        public NewActivity()
        {
            this.InitializeComponent();
        }

        private void ColorChoosed(object sender, TappedRoutedEventArgs e)
        {
            Rectangle sendeRectangle = sender as Rectangle;
            SolidColorBrush brush = (SolidColorBrush) sendeRectangle.Fill;
            _color = brush.Color;
            
            _parentPopup = Parent as Popup;
            Grid grid = _parentPopup.Parent as Grid;
            _parent = grid.Parent as PromodoTimer;
            _service = _parent.Service;
        }

        private void AddNewActivity(object sender, TappedRoutedEventArgs e)
        {
            if (Validation())
            {
                MappedActivity activity = new MappedActivity()
                {
                    Name = NameTextBox.Text,
                    WorkTime = Int32.Parse(WorkTimeTextBox.Text),
                    RestTime = Int32.Parse(RestTimeTextBox.Text),
                    Color = _color.Value
                };

                _service.AddActivity(activity);
                _parent.RefreshActivityList();
               ClosePopup();
            }
        }

        private void CancelAdding(object sender, TappedRoutedEventArgs e)
        {
            if (_parentPopup == null)
                _parentPopup = Parent as Popup;

            ClosePopup();
        }

        private bool Validation()
        {
            WarningTextBox.Text = String.Empty;
            bool valid = true;

            if (NameTextBox.Text == String.Empty)
            {
                valid = false;
                WarningTextBox.Text = "The name field is required. ";
            }

            int tmp;

            if (WorkTimeTextBox.Text == String.Empty || !Int32.TryParse(WorkTimeTextBox.Text, out tmp))
            {
                valid = false;
                WarningTextBox.Text += "The work time field is required. ";
            }

            if (RestTimeTextBox.Text == String.Empty || !Int32.TryParse(RestTimeTextBox.Text, out tmp))
            {
                valid = false;
                WarningTextBox.Text += "The rest time field is required. ";
            }

            if (!_color.HasValue)
            {
                valid = false;
                WarningTextBox.Text += "The color is required. Please choose.";
            }

            return valid;
        }

        private void ClosePopup()
        {
            NameTextBox.Text = WorkTimeTextBox.Text = RestTimeTextBox.Text = String.Empty;
            _color = null;
            _parentPopup.IsOpen = false;
        }
    }
}
