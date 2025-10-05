using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

using CommunityToolkit.Mvvm.Messaging;

using SenYing.Common.Message;
using SenYing.Common.Model;
using SenYing.WPF.Views;

namespace SenYing.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
            MessageBox.Show("本软件所有内容均收集于互联网各种视频网站，本软件不会保存,复制,传播任何资源。本站不负任何法律责任, 本软件只供学习计算机相关知识, 禁止一切商用行为, 如果因为滥用本软件导致的后果请自行承担! \r\n如果侵犯了您的权益，请通知作者，作者会第一时间及时删除侵权内容，谢谢合作！\r\n请支持正版！");
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void WindowRestore(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized 
                ? WindowState.Normal : WindowState.Maximized;
        }
        private void WindowMinimize(object sender, RoutedEventArgs e)
        {

            WindowState = WindowState.Minimized;
        }

        private void OpenMenu(object sender, RoutedEventArgs e)
        {
            var animation = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut }
            };
            if(Menu.Width == 0)
            {
                animation.To = 200;
                Menu.BeginAnimation(WidthProperty, animation);
            }
            else
            {
                animation.To = 0;
                Menu.BeginAnimation(WidthProperty, animation);
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key != Key.Enter)
            {
                return;
            }
            SearchTextBox.Text = SearchTextBox.Text.Trim();

            WeakReferenceMessenger.Default.Send(new GoToSearchMessage());
            WeakReferenceMessenger.Default
                .Send(new SearchByKeywordMessage(SearchTextBox.Text));
        }
    }
}
