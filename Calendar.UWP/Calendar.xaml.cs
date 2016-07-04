using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Calendar.UWP
{
    /// <summary>
    /// Customizable calendar control.
    /// </summary>
    /// <remarks>
    /// <para>
    /// "DayTemplateSelector" can be set to change the needed days style.
    /// </para>
    /// </remarks>
    public sealed partial class Calendar : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Standard CalendarView control is used to pick a month.
        /// </summary>
        CalendarView _monthPicker;

        public Calendar()
        {
            InitializeComponent();
            Loaded += LoadedHandler;

            // Each FlipView item is a month.
            CalendarHolder.ItemsSource = Months;            
        }

        /// <summary>
        /// Handles the _monthPicker (CalendarView) display mode changed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dp"></param>
        void DisplayModeChangedHandler(DependencyObject sender, DependencyProperty dp)
        {
            if(_monthPicker.DisplayMode == CalendarViewDisplayMode.Month)
            {
                var parent = _monthPicker.Parent;
                var dates = _monthPicker.SelectedDates;
                _monthPicker.CalendarViewDayItemChanging += DatePickerChangeHandler;
            }
        }

        /// <summary>
        /// List of months.
        /// </summary>
        ObservableCollection<MonthViewItem> _months = new ObservableCollection<MonthViewItem>();

        /// <summary>
        /// Gets the months list.
        /// </summary>
        public ObservableCollection<MonthViewItem> Months
        {
            get
            {
                return _months;
            }

        }

        /// <summary>
        /// Refreshes the data in calendar.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It's used after changes in the data model occur, and one or more days need to use a different template. 
        /// This method must be called explicitly.
        /// </para>
        /// </remarks>
        public void RefreshCalendar()
        {
            if(CalendarHolder.SelectedItem != null)
            {
                var currentMonth = (MonthViewItem)CalendarHolder.SelectedItem;
                LoadNewData(currentMonth.Month, currentMonth.Year);
            }
        }
        
        /// <summary>
        /// Gets o sets the selected date.
        /// </summary>
        public object SelectedDate
        {
            get
            {
                return (DateTime)GetValue(SelectedDateProperty);
            }
            set
            {
                SetValue(SelectedDateProperty, value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Dependency property for selected date. Allows to use this property in XAML.
        /// </summary>
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime),
                typeof(Calendar), new PropertyMetadata(null));

        
        /// <summary>
        /// Day template selector.
        /// </summary>
        CalendarTemplateSelectorBase _dayTemplateSelector;

        /// <summary>
        /// Gets or sets the day template selector.
        /// </summary>
        public CalendarTemplateSelectorBase DayTemplateSelector
        {
            get
            {
                return _dayTemplateSelector;
            }
            set
            {
                _dayTemplateSelector = value;
                _dayTemplateSelector.HostControl = this;
            }
        }

        /// <summary>
        /// Handles the loading event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            LoadNewData(DateTime.Now.Month, DateTime.Now.Year);
        }

        /// <summary>
        /// Loads new data to calendar.
        /// </summary>
        /// <remarks>
        /// <para>
        /// To be used when jumping to another month or year, but not when moving one month back or forth.
        /// </para>
        /// </remarks>
        /// <param name="month">Month to display</param>
        /// <param name="year">Year to display</param>
        void LoadNewData(int month, int year)
        {
            // Hinding the calendar data.
            AnimateOpacity(CalendarHolder, 0, async (s, es) =>
            {
                // Showing a wait ring.
                WaitRing.Visibility = Visibility.Visible;
                WaitRing.IsActive = true;

                // Waiting for the ring to start turning.
                await Task.Delay(10);

                // Disabling the selection changed event.
                CalendarHolder.SelectionChanged -= CalendarHolderSelectionChangedHandler;

                // Clearinf months list.
                Months.Clear();

                // Adding a month.
                Months.Add(new MonthViewItem(month, year, DayTemplateSelector));

                // Setting the provided month as currently displayed (selected).
                CalendarHolder.SelectedItem = Months[0];
                CalendarHolder.UpdateLayout();

                // Hinding the wait ring.
                WaitRing.Visibility = Visibility.Collapsed;
                WaitRing.IsActive = false;

                // Waiting for it to hide.
                await Task.Delay(10);

                // Showing the calendar data back.
                AnimateOpacity(CalendarHolder, 1, (sender, args) =>
                {
                    // Adding one month before the selection.
                    AddPreviousMonth();

                    // Adding one month after the selection.
                    AddNextMonth();

                    // Enabling the selection changed event back.
                    CalendarHolder.SelectionChanged += CalendarHolderSelectionChangedHandler;
                });                
            });
        }

        /// <summary>
        /// Handles month change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalendarHolderSelectionChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            CheckIfThereAreEnoughItems();
        }

        // A switch notifying if data adding is in progress or not.
        bool _isAddingInProgress;

        /// <summary>
        /// Adds new months if there are not enough.
        /// </summary>
        async void CheckIfThereAreEnoughItems()
        {
            if (_isAddingInProgress) return;
            _isAddingInProgress = true;
            if (CalendarHolder.SelectedIndex >= Months.Count - 1)
            {
                // add 1 month ahead
                AddNextMonth();
            }
            else if(CalendarHolder.SelectedIndex <= 1)
            {
                // Waiting for the animation to finish before loading data.
                await Task.Delay(500);
                // add 1 month before.
                AddPreviousMonth();
            }
            _isAddingInProgress = false;
        }

        /// <summary>
        /// Adds 1 month at the end of the list.
        /// </summary>
        void AddNextMonth()
        {
            if (!Months.Any()) return;

            // It's possible to load more months, but for performance reasons especially on the phones I set only one. 
            // This is why the cycle is here.
            // It will be enough to change the "i < 1" to "i < 6" to load for half a year ahead.
            for (var i = 0; i < 1; i++)
            {
                var lastLoadedMonth = Months.Last();
                var month = lastLoadedMonth.Month < 12 ? lastLoadedMonth.Month + 1 : 1;
                var year = lastLoadedMonth.Month < 12 ? lastLoadedMonth.Year : lastLoadedMonth.Year + 1;
                Months.Add(new MonthViewItem(month, year, DayTemplateSelector));
            }         
        }

        /// <summary>
        /// Adds 1 month at the beginning of he list.
        /// </summary>
        void AddPreviousMonth()
        {
            if (!Months.Any()) return;            

            // It's possible to load more months, but for performance reasons especially on the phones I set only one. 
            // This is why the cycle is here.
            // It will be enough to change the "i < 1" to "i < 6" to load for half a year before.
            for (var i = 0; i < 1; i++)
            {
                var firstLoadedMonth = Months.First();
                var month = firstLoadedMonth.Month > 1 ? firstLoadedMonth.Month - 1 : 12;
                var year = firstLoadedMonth.Month > 1 ? firstLoadedMonth.Year : firstLoadedMonth.Year - 1;
                Months.Insert(0, new MonthViewItem(month, year, DayTemplateSelector));
            }          
        }

        #region INotifyPropertyChanged
        /// <summary>
        /// Realizes INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param propertyName="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        /// <summary>
        /// Handles the click on the previous month button (arrow up at the left top corner).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousMonthClickHandler(object sender, RoutedEventArgs e)
        {
            if (CalendarHolder.SelectedIndex == 0) return;
            CalendarHolder.SelectedIndex--;
        }

        /// <summary>
        /// Handles the click on the next month button (arrow down at the right top corner).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextMonthClickHandler(object sender, RoutedEventArgs e)
        {
            if (CalendarHolder.SelectedIndex == Months.Count - 1) return;
            CalendarHolder.SelectedIndex++;
        }

        /// <summary>
        /// Day tapped event.
        /// </summary>
        public event EventHandler<TappedRoutedEventArgs> DayTapped;

        /// <summary>
        /// Fires the day tapped event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDayTapped(object sender, TappedRoutedEventArgs e)
        {
            DayTapped?.Invoke(sender, e);
        }

        /// <summary>
        /// Handles the day tap.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DayTappedHandler(object sender, TappedRoutedEventArgs e)
        {
            // Changing the selected day.
            SelectedDate = ((DayViewItem)((FrameworkElement)sender).DataContext).Date;

            if (SelectionAnimationIsOn)
            {
                // Animating selection.
                AnimateSelection((FrameworkElement)sender);
                await Task.Delay(200);
            }
            OnDayTapped(sender, e);
        }

        /// <summary>
        /// Gets or sets whether the day selection animation is on.
        /// </summary>
        public bool SelectionAnimationIsOn { get; set; }

        /// <summary>
        /// Animates the a day selection.
        /// </summary>
        /// <param name="dayElement"></param>
        void AnimateSelection(FrameworkElement dayElement)
        {
            var animation = (Storyboard)Resources["SelectionAnimation"];
            animation.Stop();
            dayElement.RenderTransform = new CompositeTransform();
            dayElement.RenderTransformOrigin = new Point(0.5, 0.5);
            Storyboard.SetTarget(animation, (CompositeTransform)dayElement.RenderTransform);
            animation.Begin();
        }

        /// <summary>
        /// Handles the date picker changed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DatePickerChangeHandler(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            // If a year is selected, doing nothing.
            if (sender.DisplayMode != CalendarViewDisplayMode.Month) return;

            // If a month is selected, hiding the picker and changing the calendar month.

            // Disabling the event.
            _monthPicker.CalendarViewDayItemChanging -= DatePickerChangeHandler;

            // Selected date.
            var date = args.Item.Date.Date;

            // Hinding the month picker.
            AnimateOpacity(_monthPicker, 0, async (s, e) =>
            {                
                _monthPickerPopup.IsOpen = false;
                await Task.Delay(100);

                // Loading new data to the calendar.
                LoadNewData(date.Month, date.Year);
            });
        }

        /// <summary>
        /// Animates opacity of an element.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="to"></param>
        /// <param name="completedHandler"></param>
        void AnimateOpacity(FrameworkElement item, double to, EventHandler<object> completedHandler)
        {
            var story = new Storyboard();
            var animation = new DoubleAnimation
            {
                To = to,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            Storyboard.SetTarget(animation, item);
            Storyboard.SetTargetProperty(animation, "Opacity");
            story.Children.Add(animation);
            if (completedHandler != null)
            {
                story.Completed += completedHandler;
            }
            story.Begin();
        }

        /// <summary>
        /// The month picker is placed in a popup.
        /// </summary>
        Popup _monthPickerPopup;

        /// <summary>
        /// Handles the month click. Shows the month picker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthPickerClickHandler(object sender, RoutedEventArgs e)
        {
            // Getting the correct position to place the popup.
            var point = ((Button)sender).TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));
            var calendarPosition = TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));
            _monthPicker = new CalendarView { DisplayMode = CalendarViewDisplayMode.Year, Opacity = 0 };
            _monthPickerPopup = new Popup
            {
                Child = _monthPicker,
                IsLightDismissEnabled = true,
                HorizontalOffset = calendarPosition.X + ActualWidth / 2 - 150,
                VerticalOffset = point.Y + 40
            };

            // Enabling event.
            _monthPicker.CalendarViewDayItemChanging += DatePickerChangeHandler;
            _monthPickerPopup.IsOpen = true;

            // Animating the month picker showing.
            AnimateOpacity(_monthPicker, 1, null);
        }
    }
}
