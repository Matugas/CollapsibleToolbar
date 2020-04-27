using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CollapsibleToolbar.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CollapsibleToolbar.Controls
{
    public partial class CollapsiblePage : ContentPage
    {

        #region BindableProperties
        public static readonly BindableProperty HeaderProperty =
            BindableProperty.Create(
                propertyName: nameof(Header),
                returnType: typeof(View),
                declaringType: typeof(CollapsiblePage),
                defaultValue: null,
                BindingMode.OneWay,
                null
            );

        public View Header
        {
            get { return (View)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly BindableProperty BodyProperty =
            BindableProperty.Create(
                propertyName: nameof(Body),
                returnType: typeof(View),
                declaringType: typeof(CollapsiblePage),
                defaultValue: null,
                BindingMode.OneWay,
                null
            );

        public View Body
        {
            get { return (View)GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }
        #endregion

        #region Properties
        private bool Touch { get; set; }
        private double OldScrollY { get; set; } = 0;
        private double VerticalDelta { get; set; }
        private bool Animating { get; set; }

        private readonly double MinSpeedToSnap = 0.5;
        private readonly double MinSpeedToSnapOnRelease = 10;
        private readonly int ThresholdToSnap = 0;
        private readonly int MinFooterSize = 128;

        private bool UseCustomThreshold => ThresholdToSnap != 0;

        public NavBar NavBar { get; set; }
        public NavBar FixedNavBar { get; set; }

        public IList<NavBarItem> NavBarItems { get; internal set; }

        public bool HasBackButton
        {
            get
            {
                return NavBar.HasBackButton;
            }
            set
            {
                NavBar.HasBackButton = value;
                FixedNavBar.HasBackButton = value;
            }
        }

        public bool SearchBarIsVisible
        {
            get
            {
                return NavBar.SearchBarIsVisible;
            }
            set
            {
                NavBar.SearchBarIsVisible = value;
                FixedNavBar.SearchBarIsVisible = value;
            }
        }

        public double SearchBarScaleY
        {
            get
            {
                return NavBar.SearchBarScaleY;
            }
            set
            {
                NavBar.SearchBarScaleY = value;
                FixedNavBar.SearchBarScaleY = value;
            }
        }
        #endregion

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public CollapsiblePage()
        {
            InitializeComponent();

            NavBar = new NavBar
            {
                TitleOpacity = 0,
                Page = this,
                BindingContext = this,
            };
            FixedNavBar = new NavBar
            {
                Opacity = 0,
                Page = this,
                BindingContext = this
            };

            headerLayout.Children.Add(NavBar);
            fixedLayout.Children.Add(FixedNavBar);

            var navBarItems = new ObservableCollection<NavBarItem>();
            navBarItems.CollectionChanged += NavBarItems_CollectionChanged;
            NavBarItems = navBarItems;

            scrollView.Scrolled += Scrolled;
            scrollView.TouchDown += ScrollView_TouchDown;
            scrollView.TouchUp += ScrollView_TouchUp;

            contentLayout.SizeChanged += ContentLayout_SizeChanged;

            PropertyChanged += OnAnyPropertyChanged;


            var keyboardService = DependencyService.Get<IKeyboardService>();
            keyboardService.KeyboardIsShown += KeyboardService_KeyboardIsShown;
            keyboardService.KeyboardIsHidden += KeyboardService_KeyboardIsHidden;
        }

        private void NavBarItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action != NotifyCollectionChangedAction.Add)
                return;

            foreach (NavBarItem item in args.NewItems)
            {
                NavBar.AddItem(item);
                FixedNavBar.AddItem(item);
            }
        }

        public bool KeyboardIsVisible { get; set; }

        private void KeyboardService_KeyboardIsHidden(object sender, EventArgs e)
        {
            KeyboardIsVisible = false;
            CalculateFiller();
            VerticalDelta = 0;
            Snap();
        }

        private void KeyboardService_KeyboardIsShown(object sender, EventArgs e)
        {
            KeyboardIsVisible = true;
            CalculateFiller();

            DelayCollapse();
        }

        private async void DelayCollapse()
        {
            await Task.Delay(50);
            if (scrollView.ScrollY > 0 && scrollView.ScrollY < headerLayout.Height - NavBar.Height)
            {
                ScrollTo(headerLayout.Height - NavBar.Height);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            foreach (NavBarItem navBarItem in NavBarItems)
            {
                SetInheritedBindingContext(navBarItem, BindingContext);
            }
            NavBar.BindingContext = BindingContext;
            FixedNavBar.BindingContext = BindingContext;
        }

        private void ScrollView_TouchDown(object sender, EventArgs e)
        {
            Animating = false;
            Touch = true;
        }

        private void ScrollView_TouchUp(object sender, EventArgs e)
        {
            Touch = false;
            if (VerticalDelta < MinSpeedToSnapOnRelease && VerticalDelta > -MinSpeedToSnapOnRelease)
            {
                VerticalDelta = scrollView.ScrollY - OldScrollY;
            }
            Snap();
        }

        private void ContentLayout_SizeChanged(object sender, EventArgs e)
        {
            CalculateFiller();
        }

        public void CalculateFiller()
        {
            var screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;


            double statusBarHeight = DependencyService.Get<IStatusBar>().GetHeight();
            double bottomSafeAreaHeight = DependencyService.Get<IStatusBar>().GetBottomHeight();

            if (Device.RuntimePlatform == Device.Android)
            {
                statusBarHeight /= DeviceDisplay.MainDisplayInfo.Density;
                bottomSafeAreaHeight /= DeviceDisplay.MainDisplayInfo.Density;
            }

            var filler = screenHeight - contentLayout.Height - NavBar.Height - statusBarHeight - bottomSafeAreaHeight;

            if (filler < MinFooterSize)
                filler = MinFooterSize;

            footerFiller.HeightRequest = filler;
        }

        private void Scrolled(object sender, ScrolledEventArgs e)
        {
            var distanceUntilStick = headerLayout.Height - NavBar.Height;

            lblBigTitle.Opacity = 1 - (e.ScrollY / distanceUntilStick);
            NavBar.TitleOpacity = (e.ScrollY / distanceUntilStick);

            if ((e.ScrollY / distanceUntilStick) > 1)
            {
                FixedNavBar.Opacity = 1;
            }
            else
            {
                FixedNavBar.Opacity = 0;
            }

            VerticalDelta = e.ScrollY - OldScrollY;
            OldScrollY = e.ScrollY;

            Snap();
        }

        private void Snap()
        {
            double thresholdToSnap = ThresholdToSnap;
            if (!UseCustomThreshold)
            {
                //thresholdToSnap = ((headerLayout.Height - NavBar.Height) / 2);
                thresholdToSnap = ((headerLayout.Height - NavBar.Height) * 0.3);
            }

            if (!Animating && !Touch && VerticalDelta >= -MinSpeedToSnap && VerticalDelta <= MinSpeedToSnap)
            {
                if (scrollView.ScrollY > 0 && scrollView.ScrollY < headerLayout.Height - NavBar.Height)
                {
                    //scrollView.IsEnabled = false;
                    if (scrollView.ScrollY > thresholdToSnap || KeyboardIsVisible)
                    {
                        ScrollTo(headerLayout.Height - NavBar.Height);
                    }
                    else
                    {
                        ScrollTo(0);
                    }
                }
            }
        }

        private void ScrollTo(double targetY)
        {
            if (!Animating)
            {
                Animating = true;
                var animation = new Animation(
                    y => scrollView.ScrollToAsync(0, y, false),
                    scrollView.ScrollY,
                    targetY
                );
                animation.Commit(
                    scrollView,
                    "Snap",
                    length: 200,
                    easing: Easing.SinIn,
                    finished: (a, b) =>
                    {
                        Animating = false;
                        //scrollView.IsEnabled = true;
                    }
                );
            }
        }

        private void OnAnyPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TitleProperty.PropertyName)
            {
                lblBigTitle.Text = Title;

                NavBar.Title = Title;
                FixedNavBar.Title = Title;
            }

            if (e.PropertyName == BodyProperty.PropertyName)
            {
                contentLayout.Children.Add(Body);
            }
        }
    }
}
