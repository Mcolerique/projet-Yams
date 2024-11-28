using System;
using System.IO;

class Yams{
    public struct Challenge
    {
        public string nom;
        public string nomAfficher;
        public string objectif;
        public string nbPoint;
        public Challenge(string n, string nA, string o, string nbP)
        {
            nom = n;
            nomAfficher = nA;
            objectif = o;
            nbPoint = nbP;
        }
    }
    public struct Joueur
    {
        public int id;
        public string pseudo;
        public int[] scoreParTour;
        public string[] challengeParTour;
        public List<Challenge> challengeDispo;
        //public int scoreTotal;
        public Joueur(int i, string pse)
        {
            id=i;
            pseudo=pse;
            scoreParTour = new int[13];
            challengeParTour = new string[13];
            challengeDispo = new List<Challenge>
                                                {
                                                    // Challenges mineurs
                                                    new Challenge("nombre1", "Nombre de 1", "Obtenir le maximum de 1", "Somme des dés ayant obtenu 1"),
                                                    new Challenge("nombre2", "Nombre de 2", "Obtenir le maximum de 2", "Somme des dés ayant obtenu 2"),
                                                    new Challenge("nombre3", "Nombre de 3", "Obtenir le maximum de 3", "Somme des dés ayant obtenu 3"),
                                                    new Challenge("nombre4", "Nombre de 4", "Obtenir le maximum de 4", "Somme des dés ayant obtenu 4"),
                                                    new Challenge("nombre5", "Nombre de 5", "Obtenir le maximum de 5", "Somme des dés ayant obtenu 5"),
                                                    new Challenge("nombre6", "Nombre de 6", "Obtenir le maximum de 6", "Somme des dés ayant obtenu 6"),
                                                    new Challenge("bonus", "Bonus", "Si la somme de la partie mineure atteint 63", "35 points"),

                                                    // Challenges majeurs
                                                    new Challenge("brelan", "Brelan", "Obtenir 3 dés de même valeur", "Somme des 3 dés identiques"),
                                                    new Challenge("carre", "Carré", "Obtenir 4 dés de même valeur", "Somme des 4 dés identiques"),
                                                    new Challenge("full", "Full", "Obtenir 3 dés de même valeur + 2 dés de même valeur", "25 points"),
                                                    new Challenge("petite", "Petite suite", "Obtenir 1-2-3-4 ou 2-3-4-5 ou 3-4-5-6", "30 points"),
                                                    new Challenge("grande", "Grande suite", "Obtenir 1-2-3-4-5 ou 2-3-4-5-6", "40 points"),
                                                    new Challenge("yams", "Yam's", "Obtenir 5 dés de même valeur", "50 points"),
                                                    new Challenge("chance", "Chance", "Obtenir le maximum de points", "Le total des dés obtenus")
                                                };
        }
    }
    public struct Partie
    {
        public Joueur[] joueur;
        public int tours;
        public string date;
        public Partie(Joueur j1, Joueur j2, DateTime d)
        {
            joueur = new Joueur[2] {j1,j2};
            tours=0;
            date=d.toString("yyyy-MM-dd");
        }
    }
    static void Main()
    {
        bool encoreJouer=true;
        while(encoreJouer)
        {
            jeu()
            Console.WriteLine("Voulez vous relancer une partie ? o/n");
            if(Console.ReadLine=="n")
            {
                encoreJouer=false;
            }
        }
    }
    public static void jeu(){
        Partie game = initialisationJeu();
        for(int i=0; i<13; i++)
        {
            tour(game.joueur[0]);
            tour(game.joueur[1]);
        }
    }
    public static void tour(Joueur j){
        Random rnd = new Random();
        int[] des = new int[5];
        bool[] relancerDes = new bool[5] {true,true,true,true,true};
        int i=0;
        bool fin=false;

        while(i<3 || !fin)
        {
            for(int j=0; j<des.Length; j++)
            {
                if(relancerDes[j]==true)
                {
                    des[j]=rnd.Next(1,6);
                }
            }
            choixDes(des,relancerDes);
            i++;
        }
    }
    public static void choixDes(int[] des, bool[] relancerDes)
    {
        Console.WriteLine("Quelle des voulez vous gardez ?");
        afficheDes(des);
        int index=0;
        while(index<6){
            index=int.Parse(Console.ReadLine())
            relancerDes[index-1]=false;
        }
    }
    public static void afficheDes(int[] des)
    {
        for(int i=0; i<des.Length; i++)
        {
            Console.Write("{0}. {1} ; ",i+1,des[i]);
        }
        Console.WriteLine("{0}. Aucun",des.Length+1);
    }
    public static void afficheChallenges(Joueur j)
    {
        for(int i=0; i<j.challengeDispo.Count; i++)
        {
            Console.WriteLine("{0}. {1} ; {2} ; {3}", i+1, j.challengeDispo[i].nomAfficher, j.challengeDispo[i].objectif, j.challengeDispo[i].nbPoint);
        }
    }
    public static Partie initialisationJeu()
    {
        DateTime thisDay = DateTime.Today;
        Console.Write("Entrez le pseudo du joueur 1 : ");
        string ps1 = Console.ReadLine();
        Console.Write("Entrez le pseudo du joueur 2 : ");
        string ps2 = Console.ReadLine();
        return new Partie(new Joueur(1,ps1),new Joueur(2,ps2),thisDay);
    }

}