
using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using SenYing.Common.Model;
using CommunityToolkit.Mvvm.Messaging;
using SenYing.Common.Message;

namespace SenYing.WPF.Views
{
    public partial class VideoWindow : Window
    {
        public VideoWindow(string url)
        {
            InitializeComponent();
            this.Loaded += (s, e) => InitializeWithUrlAsync(url);
            WeakReferenceMessenger.Default.Register<ChangeEpisodeMessage>(this, (w, m) =>{
                InitializeWithUrlAsync(m.url);
            });
        }
        public VideoWindow()
        {
            InitializeComponent();
        }

        private async void InitializeWithUrlAsync(string url)
        {
            string html = $@"
                    <html>
                    <head>
                        <script src='https://cdn.jsdelivr.net/npm/hls.js@latest'></script>
                    </head>
                    <body style='margin:0; background:black;'>
                        <video id='video' width='100%' height='100%' controls autoplay></video>
                        <script>
                            const video = document.getElementById('video');
                            const videoSrc = '{url}';

                            if (Hls.isSupported()) {{
                                const hls = new Hls();
                                hls.loadSource(videoSrc);
                                hls.attachMedia(video);
                                hls.on(Hls.Events.MANIFEST_PARSED, function () {{
                                    video.play();
                                }});
                            }} else if (video.canPlayType('application/vnd.apple.mpegurl')) {{
                                video.src = videoSrc;
                                video.addEventListener('loadedmetadata', function () {{
                                    video.play();
                                }});
                            }}
                        </script>
                    </body>
                    </html>";

            await webView.EnsureCoreWebView2Async();

            webView.CoreWebView2.NavigateToString(html);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void WindowRestore(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal : WindowState.Maximized;

            if(WindowState == WindowState.Maximized)
            {
                TopBar.Visibility = Visibility.Collapsed;
            }

        }
        private void WindowMinimize(object sender, RoutedEventArgs e)
        {

            WindowState = WindowState.Minimized;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            // 停止视频播放
            if (webView != null)
            {
                // 停止音视频
                webView.CoreWebView2?.ExecuteScriptAsync("document.querySelector('video')?.pause();");
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && TopBar.Visibility == Visibility.Collapsed)
            {
                TopBar.Visibility = Visibility.Visible;
                WindowState = WindowState.Normal;

            }

        }
    }
}
