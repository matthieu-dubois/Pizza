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

       //declaration d'une liste d'objet pizza

        List<Pizza> pizzas = new List<Pizza>();

        // declaration de la liste de favoris pizza

        List<string> pizzasFav = new List<string>();

        //enumeration pour le Tri
        enum e_tri
        {
            TRI_AUCUN,
            TRI_NOM,
            TRI_PRIX,
            TRI_FAV
        }

        e_tri tri = e_tri.TRI_AUCUN;

        const string KEY_TRI = "tri";

        const string KEY_FAV = "fav";

        public MainPage()
        {
            //methode chargement de la liste des favoris
            LoadFavList();

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
                    //pour le TRI
                    //listeView.ItemsSource = GetPizzasFromTri(tri, pizzas);

                    
                    listeView.ItemsSource = GetPizzaCells(GetPizzasFromTri(tri, pizzas), pizzasFav);
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
                //Pour le tri sans favoris
                //listeView.ItemsSource = GetPizzasFromTri(tri, pizzas);

                listeView.ItemsSource = GetPizzaCells(GetPizzasFromTri(tri, pizzas), pizzasFav);

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

            //if (tri == e_tri.TRI_AUCUN)
            //{
            //    tri = e_tri.TRI_NOM;
            //}
            //else if (tri == e_tri.TRI_NOM)
            //{
            //    tri = e_tri.TRI_PRIX;
            //}
            //else if (tri == e_tri.TRI_PRIX)
            //{
            //    tri = e_tri.TRI_AUCUN;
            //}

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
                tri = e_tri.TRI_FAV;
            }
            else if (tri == e_tri.TRI_FAV)
            {
                tri = e_tri.TRI_AUCUN;
            }

            SortButton.Source = GetImageSourceFromTri(tri);

            //Pour le tri classique sans favoris
            //listeView.ItemsSource = GetPizzasFromTri(tri, pizzas);

            listeView.ItemsSource = GetPizzaCells(GetPizzasFromTri(tri, pizzas), pizzasFav);

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
                case e_tri.TRI_FAV:
                    return "sort_fav.png";

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
                case e_tri.TRI_FAV:

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

        // afficher les pizzas qui sont dans la liste favoris
        private List<PizzaCell> GetPizzaCells(List<Pizza> p, List<string> f)
        {
            List<PizzaCell> ret = new List<PizzaCell>();

            if (p == null)
            {
                return ret;
            }

            foreach (Pizza pizza in p)
            {
                bool isFav = f.Contains(pizza.Nom);

                if (tri == e_tri.TRI_FAV)
                {
                    if (isFav)
                    {
                        ret.Add(new PizzaCell { pizza = pizza, isFavorite = isFav, favChangedAction = OnFavPizzaChanged });
                    }
                }
                else
                {
                    ret.Add(new PizzaCell { pizza = pizza, isFavorite = isFav, favChangedAction = OnFavPizzaChanged });
                }
            }

            return ret;
        }

        //methode pour les favoris
        private void OnFavPizzaChanged(PizzaCell pizzaCell)
        {
            //pizzasFav

            // Ajouter ou supprimer 
            // pizzaCell.pizza.nom
            // pizzaCell.IsFavorite

            bool isInFavList = pizzasFav.Contains(pizzaCell.pizza.Nom);

            if (pizzaCell.isFavorite && !isInFavList)
            {
                pizzasFav.Add(pizzaCell.pizza.Nom);
                SaveFavList();
            }
            else if (!pizzaCell.isFavorite && isInFavList)
            {
                pizzasFav.Remove(pizzaCell.pizza.Nom);
                SaveFavList();
            }

        }

        //Methode pour les favoris
        private void SaveFavList()
        {
            string json = JsonConvert.SerializeObject(pizzasFav);
            Application.Current.Properties[KEY_FAV] = json;
            Application.Current.SavePropertiesAsync();
        }

        private void LoadFavList()
        {
            if (Application.Current.Properties.ContainsKey(KEY_FAV))
            {
                string json = Application.Current.Properties[KEY_FAV].ToString();
                pizzasFav = JsonConvert.DeserializeObject<List<string>>(json);
            }
        }

    }
}
