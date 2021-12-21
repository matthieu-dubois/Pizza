using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PizzaApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // mise place du système de navigation avec la customisation du background de la barre de navigation

            var navigationPage = new NavigationPage(new MainPage());
            navigationPage.BackgroundColor = Color.FromHex("1abbd4");

            MainPage = navigationPage; 



            // navigation par default

            //MainPage =  new NavigationPage (new MainPage());


        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
