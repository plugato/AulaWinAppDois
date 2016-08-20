using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AulaWinApp2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btnConsulta_Click(object sender, RoutedEventArgs e)
        {
            var texto = txtConsulta.Text;
            if (String.IsNullOrWhiteSpace(texto))
            {
                var dialog = new MessageDialog("Texto é obrigatório!", "Oooopps");
                await dialog.ShowAsync();
            }
            else
            {
                var client = new HttpClient();
                //client.BaseAddress = new Uri("https://api.github.com/");
                //client.
                client.BaseAddress = new Uri("https://api.github.com");
                client.DefaultRequestHeaders.Add(
                    "Authorization",
                    "token colocaraquioseutoken");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add(
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");
                var ret = await client.GetAsync($"users/{texto}");

                try
                {
                    ret.EnsureSuccessStatusCode();

                    var jsonText = await ret.Content.ReadAsStringAsync();

                    var model = JsonConvert.DeserializeObject<RootObject>(jsonText);

                    imgAvatar.Source = new BitmapImage(new Uri(model.avatar_url));

                    txbLogin.Text = string.IsNullOrWhiteSpace(model.login) ? "" : model.login;
                    txbNome.Text = string.IsNullOrWhiteSpace(model.name) ? "" : model.name;
                    txbLocalidade.Text = string.IsNullOrWhiteSpace(model.location) ? "" : model.location;

                }
                catch (Exception ex)
                {
                    var msg = string.IsNullOrWhiteSpace(ex.Message) ?
                    ex.InnerException.Message :
                    ex.Message;
                    var dialog =
                        new MessageDialog(msg, "Ooooopsssss...");

                    await dialog.ShowAsync();

                }
            }




        }
    }
}
