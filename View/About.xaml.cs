using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation; //For the animated text/bacground/page
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MTG.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class About : Page
    {
        public About()
        {
            //Storyboard will start, needed for the colour chaning of the About Page
            this.InitializeComponent();
            ((Storyboard)Resources["GradientAnimation"]).Begin(); 
        }


        private void Exit_Click(object sender, RoutedEventArgs e) 
        {
            //Calling the Exit
            Application.Current.Exit();
        }


        private void BackButton_Click(object sender, RoutedEventArgs e) 
        {
            //Calling the goBack from button click
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();

            }

        }

        private async void Button_Click_Sound(object sender, RoutedEventArgs e) 
        {
            //Calling the speech SpeechSynthesisStream and setting text to be spoken by the SpeechSynthesizer
            MediaElement mediaElement = new MediaElement();
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Indeed.......... You have chosen wisely...We thankyou for playing this game, now go and slay more monsters!");
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }
    }
}