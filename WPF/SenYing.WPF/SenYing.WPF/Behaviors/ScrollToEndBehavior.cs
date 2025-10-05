using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Microsoft.Xaml.Behaviors;

namespace SenYing.WPF.Behaviors
{
    public class ScrollToEndBehavior : Behavior<ListView>
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(System.Windows.Input.ICommand),
                typeof(ScrollToEndBehavior),
                new PropertyMetadata(null));

        public System.Windows.Input.ICommand Command
        {
            get => (System.Windows.Input.ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        private ScrollViewer? _scrollViewer;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnListViewLoaded;
        }

        protected override void OnDetaching()
        {
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged -= OnScrollChanged;
            }
            AssociatedObject.Loaded -= OnListViewLoaded;
            base.OnDetaching();
        }

        private void OnListViewLoaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = GetScrollViewer(AssociatedObject);
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged += OnScrollChanged;
            }
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_scrollViewer == null) return;

            // 当滚动到底部 50px 以内时触发
            if (_scrollViewer.VerticalOffset + _scrollViewer.ViewportHeight >= _scrollViewer.ExtentHeight - 50)
            {
                if (Command?.CanExecute(null) == true)
                    Command.Execute(null);
            }
        }

        private ScrollViewer? GetScrollViewer(DependencyObject dep)
        {
            if (dep is ScrollViewer sv) return sv;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dep); i++)
            {
                var child = VisualTreeHelper.GetChild(dep, i);
                var result = GetScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }
    }
}
