using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

class Yams{
    public struct Challenge
    {
        //public int code;
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
        public int totalScoreChallengeMineure;
        public List<Challenge> challengeDispo;
        public Joueur(int i, string pse)
        {
            id=i;
            pseudo=pse;
            scoreParTour = new int[13];
            challengeParTour = new string[13];
            totalScoreChallengeMineure = 0;
            challengeDispo = new List<Challenge>
                                                {
                                                    // Challenges mineurs
                                                    new Challenge("nombre1", "Nombre de 1", "Obtenir le maximum de 1", "Somme des dés ayant obtenu 1"),
                                                    new Challenge("nombre2", "Nombre de 2", "Obtenir le maximum de 2", "Somme des dés ayant obtenu 2"),
                                                    new Challenge("nombre3", "Nombre de 3", "Obtenir le maximum de 3", "Somme des dés ayant obtenu 3"),
                                                    new Challenge("nombre4", "Nombre de 4", "Obtenir le maximum de 4", "Somme des dés ayant obtenu 4"),
                                                    new Challenge("nombre5", "Nombre de 5", "Obtenir le maximum de 5", "Somme des dés ayant obtenu 5"),
                                                    new Challenge("nombre6", "Nombre de 6", "Obtenir le maximum de 6", "Somme des dés ayant obtenu 6"),

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
            tours=1;
            date=d.ToString("yyyy-MM-dd");
        }
    }
    static void Main()
    {
        bool encoreJouer=true;
        while(encoreJouer)
        {
            jeu();
            Console.WriteLine("Voulez vous relancer une partie ? o/n");
            if(Console.ReadLine()=="n")
            {
                encoreJouer=false;
            }
        }
    }
    public static void jeu(){
        Partie game = initialisationJeu();
        for(int i=0; i<13; i++)
        {
            Console.WriteLine("Tour {0}",i+1);
            Console.WriteLine();

            Console.WriteLine("Tour de {0} :", game.joueur[0].pseudo);
            tour(ref game.joueur[0],i);
            Console.WriteLine("Vous avez gagné {0} points",game.joueur[0].scoreParTour[i]);
            Console.WriteLine();

            Console.WriteLine("Tour de {0} :", game.joueur[1].pseudo);
            tour(ref game.joueur[1],i);
            Console.WriteLine("Vous avez gagné {0} points",game.joueur[1].scoreParTour[i]);
            Console.WriteLine();
        }
    }
    public static void tour(ref Joueur joueur, int tours){
        Random rnd = new Random();
        int[] des = new int[5];
        bool[] relancerDes = new bool[5] {true,true,true,true,true};
        int i=0;
        bool fin=false;

        while(i<3 && !fin)
        {
            for(int j=0; j<des.Length; j++)
            {
                if(relancerDes[j]==true)
                {
                    des[j]=rnd.Next(1,6);
                    Thread.Sleep(50);
                }
            }
            choixDes(des,relancerDes);
            fin = finRelance(relancerDes);
            i++;
        }
        int index = choixChallenge(joueur);
        joueur.scoreParTour[tours]=calculScore(ref joueur,des,index-1);
        joueur.challengeDispo.RemoveAt(index-1);
    }
    public static void choixDes(int[] des, bool[] relancerDes)
    {
        Console.WriteLine("Voici vos dès :");
        afficheDes(des);
        char reponse;
        for(int i=0; i<5; i++)
        {
            Console.Write("Voulez vous gardez le {0} ? y/n  ", des[i]);
            reponse = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if(reponse=='y')
            {
                relancerDes[i]=false;
            }
            else
            {
                relancerDes[i]=true;
            }      
        }
        Console.WriteLine();
    }
    public static bool finRelance(bool[] tab)
    {
        for(int i=0; i<tab.Length; i++)
        {
            if(tab[i]==true)
            {
                return false;
            }
        }
        return true;
    }
    public static int calculScore(ref Joueur j, int[] des, int indexChallenge)
    {
        int nbPoint=0;
        switch(j.challengeDispo[indexChallenge].nom)
        {
            case "nombre1":
            nbPoint = 1*nbOcc(des,1);
            j.totalScoreChallengeMineure+=nbPoint;
            break;

            case "nombre2":
            nbPoint = 2*nbOcc(des,2);
            j.totalScoreChallengeMineure+=nbPoint;
            break;
            case "nombre3":
            nbPoint = 3*nbOcc(des,3);
            j.totalScoreChallengeMineure+=nbPoint;
            break;
            case "nombre4":
            nbPoint = 4*nbOcc(des,4);
            j.totalScoreChallengeMineure+=nbPoint;
            break;
            case "nombre5":
            nbPoint = 5*nbOcc(des,5);
            j.totalScoreChallengeMineure+=nbPoint;
            break;
            case "nombre6":
            nbPoint = 6*nbOcc(des,6);
            j.totalScoreChallengeMineure+=nbPoint;
            break;
            case "brelan":
            nbPoint = verifBrelan(des);
            break;
            case "carre":
            nbPoint = verifCarre(des);
            break;
            case "full":
            nbPoint = verifFull(des);
            break;
            case "petite":
            if()
            {
                nbPoint = 30;
            }
            break;
            case "grande":
            if()
            {
                nbPoint = 40;
            }
            break;
            case "yams":
            if(nbOcc(des,des[0])==5)
            {
                nbPoint=50;
            }
            break;
            case "chance":
            nbPoint = somme(des);
            break;
            default:
            Console.WriteLine("ya une erreur la mon reuf");
            break;
        }
        return nbPoint;
    }
    public static int nbOcc(int[] t, int nb)
    {
        int cpt = 0;
        for(int i=0; i<t.Length;i++)
        {
            if (t[i]==nb)
            {
                cpt++;
            }
        }
        return cpt;
    }   
    public static int somme(int[] tab)
    {
        int somme=0;
        for(int i=0; i<tab.Length; i++)
        {
            somme+=tab[i];
        }
        return somme;
    }
    public static int monotonie(int[] listeInitial)
    {
        int cpt=0;
        int tailleMax=0;
        for(int i=0;i<listeInitial.Length-1;i++){
            if(listeInitial[i]<listeInitial[i+1]){
                cpt++;
            }
            else{
                if(cpt>tailleMax){
                    tailleMax=cpt;
                    cpt=0;
                }
            }
        }
        return tailleMax;
    }
    public static int verifBrelan(int[] des)
    {
        List<int> nbATester = listeNombreDifferent(des);
        if(nbATester.Count>3)
        {
            return 0;
        }
        else
        {
            int nbOccurence;
            for(int i=0; i<nbATester.Count; i++)
            {
                nbOccurence = nbOcc(des,nbATester[i]);
                if(nbOccurence >=3)
                {
                    return nbATester[i]*3;
                }
            }
        }
        return 0;
    }
    public static int verifCarre(int[] des)
    {
        List<int> nbATester = listeNombreDifferent(des);
        if(nbATester.Count>2)
        {
            return 0;
        }
        else
        {
            int nbOccurence;
            for(int i=0; i<nbATester.Count; i++)
            {
                nbOccurence =nbOcc(des,nbATester[i]);
                if(nbOccurence >=4)
                {
                    return nbATester[i]*4;
                }
            }
        }
        return 0;
    }
    public static int verifFull(int[] des)
    {
        List<int> nbATester = listeNombreDifferent(des);
        if(nbATester.Count>2)
        {
            return 0;
        }
        else
        {
            int nbOccurence =nbOcc(des,nbATester[0]);
            if(nbOccurence ==3 || nbOccurence ==2)
            {
                return 25;
            }
        }
        return 0;
    }
    public static List<int> listeNombreDifferent(int[] des)
    {
        List<int> listeNombre = new List<int>();
        for(int i=1; i<=6; i++)
        {
            if(des.Contains(i))
            {
                listeNombre.Add(i);
            }
        }
        return listeNombre;
    }
    public static int choixChallenge(Joueur j)
    {
        afficheChallenges(j);
        Console.WriteLine("Quel challenge voulez vous choisir ?");
        int choix = int.Parse(Console.ReadLine());
        Console.WriteLine();
        if(choix==0)
        {
            afficheChallengesDetaillé(j);
            Console.WriteLine("Quel challenge voulez vous choisir ?");
            return int.Parse(Console.ReadLine());
        }
        else
        {
            return choix;
        }
    }
    public static void afficheDes(int[] des)
    {
        for(int i=0; i<des.Length; i++)
        {
            Console.Write("{0} ; ",des[i]);
        }
        Console.WriteLine();
    }
    public static void afficheChallenges(Joueur j)
    {
        Console.WriteLine("0. Description des challenges");
        for(int i=0; i<j.challengeDispo.Count; i++)
        {
            Console.WriteLine("{0}. {1}", i+1, j.challengeDispo[i].nomAfficher);
        }
        Console.WriteLine();
    }
    public static void afficheChallengesDetaillé(Joueur j)
    {
        for(int i=0; i<j.challengeDispo.Count; i++)
        {
            Console.WriteLine("{0}. {1} ; {2} ; {3}", i+1, j.challengeDispo[i].nomAfficher, j.challengeDispo[i].objectif, j.challengeDispo[i].nbPoint);
        }
        Console.WriteLine();
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