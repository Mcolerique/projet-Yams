using System;
using System.IO;

class Yams{
    public struct Challenge{
        public string nom;
        public string nomAfficher;
        public string objectif;
        public string nbPoint;
    }
    public struct Joueur{
        public int id;
        public string pseudo;
        public int[] scoreParTour;
        public string[] challengeParTour;
        //public int scoreTotal;
        public Joueur(int i, string pse){
            id=i;
            pseudo=pse;
            scoreParTour = new int[13];
            challengeParTour = new string[13];
        }
    }
    public struct Partie{
        public Joueur[] joueur;
        public int tours;
        public string date;
        public Partie(Joueur j1, Joueur j2, DateTime d){
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
    public static void choixDes(int[] des, bool[] relancerDes){
        Console.WriteLine("Quelle des voulez vous gardez ?");
        afficheDes(des);
        int index=0;
        while(index<6){
            index=int.Parse(Console.ReadLine())
            relancerDes[index]=false;
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