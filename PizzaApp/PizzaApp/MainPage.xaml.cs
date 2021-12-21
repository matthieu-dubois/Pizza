using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PizzaApp.Model;
using Xamarin.Forms;

namespace PizzaApp
{
    public partial class MainPage : ContentPage
    {
        //decommenter pour effectuer du synchrone 

        //List<Pizza> pizzas = new List<Pizza>();

        public MainPage()
        {
            InitializeComponent();

            #region Synchrone

            //Synchrone
            // lien du fichier JSON 
            //const string URL = "https://drive.google.com/uc?export=download&id=1CJorEKQMfEC-QPJpq9reLet835qMAF3d";

            //string pizzasJson = "";

            //using (var webClient = new WebClient())
            //{
            //    try
            //    {
            //        pizzasJson = webClient.DownloadString(URL);
            //    }
            //    catch (Exception ex)
            //    {
            //        //thread Réseau 
            //        Device.BeginInvokeOnMainThread(() =>
            //        {
            //            DisplayAlert("Erreur", " Une erreur réseau s'est produite : " + ex, "OK");

            //        });

            //    }

            //    pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pizzasJson);

            //    listeView.ItemsSource = pizzas;


            //}

            #endregion

            #region Asynchrone
            //Asynchrone
            //const string URL = "https://drive.google.com/uc?export=download&id=1CJorEKQMfEC-QPJpq9reLet835qMAF3d";

            //listeView.IsVisible = false;
            //WaitLayout.IsVisible = true;

            //using (var webClient = new WebClient())
            //{
            //    try
            //    {
            //        webClient.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
            //        {
            //            string pizzasJson = e.Result;
            //            List<Pizza> pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pizzasJson);

            //                    // ici on reçoit les données et on les affiches
            //                    Device.BeginInvokeOnMainThread(() =>
            //            {
            //                listeView.ItemsSource = pizzas;
            //                listeView.IsVisible = true;
            //                WaitLayout.IsVisible = false;
            //            });
            //        };

            //        webClient.DownloadStringAsync(new Uri(URL));
            //    }
            //    catch (Exception ex)
            //    {
            //        //thread Réseau 
            //        Device.BeginInvokeOnMainThread(() =>
            //        {
            //            DisplayAlert("Erreur", " Une erreur réseau s'est produite : " + ex, "OK");

            //        });
            //    }
            //}

            #endregion

            #region pullToRefresh
            //pull to refresh

            listeView.RefreshCommand = new Command((obj) =>
            {
                Console.WriteLine("RefreshCommand");
                DownloadData((pizzas) =>
                {
                    listeView.ItemsSource = pizzas;
                    listeView.IsRefreshing = false;
                });
            });

            listeView.IsVisible = false;
            WaitLayout.IsVisible = true;

            Console.WriteLine("ETAPE 1");


            // Appel a ma fonction
            DownloadData((pizzas) =>
            {
                listeView.ItemsSource = pizzas;

                listeView.IsVisible = true;
                WaitLayout.IsVisible = false;
            });


            // Thread


            Console.WriteLine("ETAPE 4");
            #endregion

        }


        public void DownloadData(Action<List<Pizza>> action)
        {
            const string URL = "https://drive.google.com/uc?export=download&id=1CJorEKQMfEC-QPJpq9reLet835qMAF3d";

            using (var webclient = new WebClient())
            {

                // Thread Main (UI)
                //pizzasJson = webclient.DownloadString(URL);

                Console.WriteLine("ETAPE 2");

                webclient.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
                {
                    //
                    Console.WriteLine("ETAPE 5");

                    //Console.WriteLine("Données téléchargées: " + e.Result);
                    try
                    {
                        string pizzasJson = e.Result;

                        List<Pizza> pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pizzasJson);

                        //

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            action.Invoke(pizzas);

                        });

                    }
                    catch (Exception ex)
                    {
                        // Thread réseau
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Erreur", "Une erreur réseau s'est produite: " + ex.Message, "OK");
                            action.Invoke(null);
                        });
                    }

                };

                Console.WriteLine("ETAPE 3");

                webclient.DownloadStringAsync(new Uri(URL));


            }
        }
        }
    }
