using PizzaApp.extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaApp.Model
{
     public class Pizza
    {
        //proprieté Nom 
        public string Nom { get; set;  }

        //image url de notre lsite de piza
        public string ImageUrl { get; set; }

        // prorpieté Prix
        public int Prix { get; set; }

        // Proprieté Ingreditiens
        public string [] Ingredients { get; set; }

        // prix Euros 
        public string PrixEuros 
        { get
            { 
                return Prix + "e"; 
            } 
        }

        // IngredientsStr
        public string IngredientsStr 
        { get 
            { 
                return String.Join(",", Ingredients); 
                
            }
        }

        // Mise en majuscule de la première lettre sans le mot clé this dans la classe Extensions

        //public string Titre
        //{
        //    get
        //    {
        //        return StringExtensions.PremiereLettreMaj(Nom);
        //    }
        //}

        public string Titre
        {
            get
            {
                return Nom.PremiereLettreMaj(); ;
            }
        }

        public Pizza()
        {

        }

        // Constructeur
        //public Pizza(string nom , string imageUrl ,int prix , string [] ingredients)
        //{
        //    this.Nom = nom;
        //    this.ImageUrl = imageUrl;
        //    this.Prix = prix;
        //    this.Ingredients = ingredients;
        //}
    }
}
