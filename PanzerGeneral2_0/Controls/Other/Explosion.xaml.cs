using System;
using System.IO;
using System.Media;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;
using static System.Net.Mime.MediaTypeNames;

namespace PanzerGeneral2_0.Controls.Other
{
    /// <summary>
    /// Logika interakcji dla klasy Explosion.xaml
    /// </summary>
    public partial class Explosion : UserControl
    {
        public Explosion()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // wyświetl eksplozję
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("pack://application:,,,/Resources/Explosion/explosion.gif", UriKind.Absolute);
            image.EndInit();
            ImageBehavior.SetAnimatedSource(explosion, image);
            ImageBehavior.SetAutoStart(explosion, true);
            ImageBehavior.SetRepeatBehavior(explosion, new RepeatBehavior(1));

            // włącz dźwięk eksplozji
            new SoundPlayer(Properties.Resources.explosion).Play();
        }
    }
}
