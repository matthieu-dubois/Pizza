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

        //enumeration pour le Tri


        List<Pizza> pizzas = new List<Pizza>();

        enum e_tri
        {
            TRI_AUCUN,
            TRI_NOM,
            TRI_PRIX,
            TRI_FAV
        }

        e_tri tri = e_tri.TRI_AUCUN;

        const string KEY_TRI = "tri";

        public MainPage()
        {
            InitializeComponent();


            // Fonction pour le Tri

            if (Application.Current.Properties.ContainsKey(KEY_TRI))
            {
                tri = (e_tri)Application.Current.Properties[KEY_TRI];
                SortButton.Source = GetImageSourceFromTri(tri);
            }

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
                    //listeView.ItemsSource = pizzas;
                    listeView.ItemsSource = GetPizzasFromTri(tri, pizzas);
                    listeView.IsRefreshing = false;


                });
            });

            listeView.IsVisible = false;
            WaitLayout.IsVisible = true;

            Console.WriteLine("ETAPE 1");


            // Appel a ma fonction
            DownloadData((pizzas) =>
            {
                //listeView.ItemsSource = pizzas;

                listeView.ItemsSource = GetPizzasFromTri(tri, pizzas);

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

                         pizzas = JsonConvert.DeserializeObject<List<Pizza>>(pizzasJson);

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


        void SortButtonClicked(object sender, System.EventArgs e)
        {
            Console.WriteLine("SortButtonClicked");

            if (tri == e_tri.TRI_AUCUN)
            {
                tri = e_tri.TRI_NOM;
            }
            else if (tri == e_tri.TRI_NOM)
            {
                tri = e_tri.TRI_PRIX;
            }
            else if (tri == e_tri.TRI_PRIX)
            {
                tri = e_tri.TRI_AUCUN;
            }

            SortButton.Source = GetImageSourceFromTri(tri);
            listeView.ItemsSource = GetPizzasFromTri(tri, pizzas);

            Application.Current.Properties[KEY_TRI] = (int)tri;
            Application.Current.SavePropertiesAsync();
        }

        private string GetImageSourceFromTri(e_tri t)
        {
            switch (t)
            {
                case e_tri.TRI_NOM:
                    return "sort_nom.png";
                case e_tri.TRI_PRIX:
                    return "sort_prix.png";

            }

            return "sort_none.png";
        }

        // afficher les pizzas en fonction du tri
        private List<Pizza> GetPizzasFromTri(e_tri t, List<Pizza> l)
        {
            if (l == null)
            {
                return null;
            }

            switch (t)
            {
                case e_tri.TRI_NOM:
                    {
                        List<Pizza> ret = new List<Pizza>(l);

                        ret.Sort((p1, p2) => { return p1.Titre.CompareTo(p2.Titre); });

                        return ret;
                    }
                case e_tri.TRI_PRIX:
                    {
                        List<Pizza> ret = new List<Pizza>(l);

                        ret.Sort((p1, p2) => { return p2.Prix.CompareTo(p1.Prix); });

                        return ret;
                    }

            }

            return l;
        }


    }
}
