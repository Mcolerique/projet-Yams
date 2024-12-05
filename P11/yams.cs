using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

class Yams{
    public struct Challenge
    {
        public int code;
        public string nom;
        public string nomAfficher;
        public string objectif;
        public string nbPoint;
        public Challenge(int c, string n, string nA, string o, string nbP)
        {
            code = c;
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
        public int[][] desParTour;
        public int score;
        public int bonus;
        public List<int> challengeDispo;
        public List<int> challengeUtiliser;
        public Joueur(int i, string pse)
        {
            id=i;
            pseudo=pse;
            scoreParTour = new int[13];
            desParTour = new int[13][];
            bonus = 0;
            score = 0;
            challengeDispo = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
            challengeUtiliser = new List<int>();
        }
    }
    public static Challenge[] CHALLENGE = new Challenge[]
                                                {
                                                    // Challenges mineurs
                                                    new Challenge(1, "nombre1", "Nombre de 1", "Obtenir le maximum de 1", "Somme des dés ayant obtenu 1"),
                                                    new Challenge(2,"nombre2", "Nombre de 2", "Obtenir le maximum de 2", "Somme des dés ayant obtenu 2"),
                                                    new Challenge(3, "nombre3", "Nombre de 3", "Obtenir le maximum de 3", "Somme des dés ayant obtenu 3"),
                                                    new Challenge(4, "nombre4", "Nombre de 4", "Obtenir le maximum de 4", "Somme des dés ayant obtenu 4"),
                                                    new Challenge(5, "nombre5", "Nombre de 5", "Obtenir le maximum de 5", "Somme des dés ayant obtenu 5"),
                                                    new Challenge(6, "nombre6", "Nombre de 6", "Obtenir le maximum de 6", "Somme des dés ayant obtenu 6"),

                                                    // Challenges majeurs
                                                    new Challenge(7, "brelan", "Brelan", "Obtenir 3 dés de même valeur", "Somme des 3 dés identiques"),
                                                    new Challenge(8, "carre", "Carré", "Obtenir 4 dés de même valeur", "Somme des 4 dés identiques"),
                                                    new Challenge(9, "full", "Full", "Obtenir 3 dés de même valeur + 2 dés de même valeur", "25 points"),
                                                    new Challenge(10, "petite", "Petite suite", "Obtenir 1-2-3-4 ou 2-3-4-5 ou 3-4-5-6", "30 points"),
                                                    new Challenge(11, "grande", "Grande suite", "Obtenir 1-2-3-4-5 ou 2-3-4-5-6", "40 points"),
                                                    new Challenge(12, "yams", "Yam's", "Obtenir 5 dés de même valeur", "50 points"),
                                                    new Challenge(13, "chance", "Chance", "Obtenir le maximum de points", "Le total des dés obtenus")
                                                };
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
        Joueur[] joueur = initialisationJeu();
        for(int i=0; i<13; i++)
        {
            Console.WriteLine();
            Console.WriteLine($"Tour n°{i+1}");
            Console.WriteLine();

            for(int j=0; j<joueur.Length; j++)
            {
                Console.WriteLine($"Tour de {joueur[j].pseudo} :");
                tour(ref joueur[j],i);
                Console.WriteLine($"vous avez choisi le challenge \"{CHALLENGE[joueur[j].challengeUtiliser[i]-1].nomAfficher}\"");
                Console.WriteLine("vous aviez les des suivant : ");
                afficheDes(joueur[j].desParTour[i]);
                Console.WriteLine($"Vous avez gagné {joueur[j].scoreParTour[i]} points");
                Console.WriteLine($"Pour l'instant vous avez un total de {somme(joueur[j].scoreParTour)} point + {verifBonus(joueur[0])} bonus");
                Console.WriteLine();
            }
        }
        affichageFinPartie(ref joueur);
        creaJson(joueur);
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
                    des[j]=rnd.Next(1,7);
                    Thread.Sleep(500);
                }
            }
            if(i<2)
            {
                choixDes(des,relancerDes);
            }
            fin = finRelance(relancerDes);
            i++;
        }
        afficheDes(des);
        int codeChallenge = choixChallenge(joueur);

        joueur.challengeUtiliser.Add(codeChallenge);
        supprimeChallenge(joueur.challengeDispo, codeChallenge);

        joueur.scoreParTour[tours]=calculScore(ref joueur,des,codeChallenge);
        joueur.desParTour[tours] = des;
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
    public static int calculScore(ref Joueur j, int[] des, int codeChallenge)
    {
        int nbPoint=0;
        if(codeChallenge <= 6)
        {
            nbPoint = codeChallenge * nbOcc(des,codeChallenge);
            return nbPoint;
        }
        switch(codeChallenge)
        {
            case 7:
            nbPoint = verifBrelan(des);
            break;

            case 8:
            nbPoint = verifCarre(des);
            break;

            case 9:
            nbPoint = verifFull(des);
            break;

            case 10:
            if(verifPetiteSuite(des))
            {
                nbPoint = 30;
            }
            break;

            case 11:
            if(verifGrandeSuite(des))
            {
                nbPoint = 40;
            }
            break;

            case 12:
            if(nbOcc(des,des[0])==5)
            {
                nbPoint=50;
            }
            break;

            case 13:
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
    public static void echange(int[] t,int i, int j  )
    {
        int sauv = t[j];
        t[j]=t[i];
        t[i]=sauv;
    }
    public static void tri(int[] t)
    {
        for(int i=0;i<t.Length;i++)
        {
            for(int j=i+1;j<t.Length;j++)
            {
                if(t[i]>t[j])
                {
                    echange(t,i,j); 
                }
            }
        }
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
    public static bool verifPetiteSuite(int[] des)
    {
        // Tri des dés pour faciliter la recherche de séquences
        tri(des);

        // Retirer les doublons pour éviter les problèmes de séquences incorrectes
        int[] uniqueDes = des.Distinct().ToArray();

        // Vérifier si l'une des trois petites suites possibles est présente
        if (uniqueDes.Contains(1) && uniqueDes.Contains(2) && uniqueDes.Contains(3) && uniqueDes.Contains(4)) return true;
        if (uniqueDes.Contains(2) && uniqueDes.Contains(3) && uniqueDes.Contains(4) && uniqueDes.Contains(5)) return true;
        if (uniqueDes.Contains(3) && uniqueDes.Contains(4) && uniqueDes.Contains(5) && uniqueDes.Contains(6)) return true;

        return false; // Aucune petite suite détectée
    }
    public static bool verifGrandeSuite(int[] des)
    {
        // Tri des dés pour faciliter la recherche de séquences
        tri(des);

        // Retirer les doublons
        int[] uniqueDes = des.Distinct().ToArray();

        // Vérifier si l'une des deux grandes suites possibles est présente
        if (uniqueDes.SequenceEqual(new int[] { 1, 2, 3, 4, 5 })) return true;
        if (uniqueDes.SequenceEqual(new int[] { 2, 3, 4, 5, 6 })) return true;

        return false; // Aucune grande suite détectée
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
        bool choixValide = false;
        int choix = 0;
        afficheChallenges(j);
        Console.WriteLine("Quel challenge voulez vous choisir ?");

        while(!choixValide)
        {
            choix = int.Parse(Console.ReadLine());
            Console.WriteLine();

            if(choix==0)
            {
                afficheChallengesDetaillé(j);
                Console.WriteLine("Quel challenge voulez vous choisir ?");
                choix = int.Parse(Console.ReadLine());
                if(!j.challengeDispo.Contains(choix))
                {
                    Console.WriteLine("Veullez choisir un challenge valide");
                }
                else
                {
                    choixValide = true;
                    return choix;
                }  
            }
            else if(!j.challengeDispo.Contains(choix))
            {
                Console.WriteLine("Veullez choisir un challenge valide");
            }
            else
            {
                choixValide = true;
                return choix;
            }  
        }
        return choix;
    }
    public static void supprimeChallenge(List<int> challenge, int choix)
    {
        for(int i=0; i<challenge.Count; i++)
        {
            if(challenge[i]==choix)
            {
                challenge.RemoveAt(i);
            }
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
            Console.WriteLine("{0}. {1}", j.challengeDispo[i], CHALLENGE[j.challengeDispo[i]-1].nomAfficher);
        }
        Console.WriteLine();
    }
    public static void afficheChallengesDetaillé(Joueur j)
    {
        for(int i=0; i<j.challengeDispo.Count; i++)
        {
            Console.WriteLine("{0}. {1} ; {2} ; {3}", j.challengeDispo[i], CHALLENGE[j.challengeDispo[i]-1].nomAfficher, CHALLENGE[j.challengeDispo[i]-1].objectif, CHALLENGE[j.challengeDispo[i]-1].nbPoint);
        }
        Console.WriteLine();
    }
    public static Joueur[] initialisationJeu()
    {
        Joueur[] j = new Joueur[2];
        Console.Write("Entrez le pseudo du joueur 1 : ");
        string ps1 = Console.ReadLine();
        Console.Write("Entrez le pseudo du joueur 2 : ");
        string ps2 = Console.ReadLine();
        j[0] = new Joueur(1,ps1);
        j[1] = new Joueur(2,ps2);
        return j;
    }
    public static void affichageFinPartie(ref Joueur[] joueur)
    {
        for(int i = 0; i<joueur.Length; i++)
        {
            joueur[i].score = somme(joueur[i].scoreParTour);
            joueur[i].bonus = verifBonus(joueur[i]);
            Console.WriteLine("{0} a marqué {1} points et {2} points bonus se qui fait un total de {3} points",joueur[i].pseudo,joueur[i].score, joueur[i].bonus,joueur[i].score + joueur[i].bonus);
            Console.WriteLine();
        }

        if((joueur[0].score + joueur[0].bonus) < (joueur[1].score + joueur[1].bonus))
        {
            Console.WriteLine("Victoire de {0}",joueur[1].pseudo);
        }
        else if((joueur[0].score + joueur[0].bonus) > (joueur[1].score + joueur[1].bonus))
        {
            Console.WriteLine("Victoire de {0}",joueur[0].pseudo);
        }
        else
        {
            Console.WriteLine("Egalité");
        }
        Console.WriteLine();
    }
    public static int verifBonus(Joueur j)
    {
        int scoreChallengeMineure=0;
        for(int i=0; i<j.challengeUtiliser.Count; i++)
        {
            if(j.challengeUtiliser[i]<=6)
            {
                scoreChallengeMineure += j.scoreParTour[i];
            }
        }
        if(scoreChallengeMineure >= 63)
        {
            return 35;
        }
        return 0;
    }
    public static void creaJson(Joueur[] joueur)
    {
        string date = DateTime.Today.ToString("yyyy-MM-dd");
        string nomJson = joueur[0].pseudo +"_"+joueur[1].pseudo+"_"+date+".json";
        FileStream fs = new FileStream(nomJson, FileMode.Create, FileAccess.Write);
        StreamWriter leFichier = new StreamWriter(fs);

        leFichier.WriteLine("{");
        leFichier.WriteLine("   \"parameters\": {");
        leFichier.WriteLine("       \"code\": \"groupe8-003\",");
        leFichier.WriteLine($"       \"date\": \"{date}\"");
        leFichier.WriteLine("   },");

        leFichier.WriteLine("   \"players\": [");
        for (int i = 0; i < joueur.Length; i++)
        {
            leFichier.WriteLine("       {");
            leFichier.WriteLine($"           \"id\": {joueur[i].id},");
            leFichier.WriteLine($"           \"pseudo\": \"{joueur[i].pseudo}\"");
            leFichier.WriteLine(i == joueur.Length - 1 ? "       }" : "       },");
        }
        leFichier.WriteLine("   ],");

        leFichier.WriteLine("   \"rounds\": [");
        for (int i = 0; i < joueur[0].scoreParTour.Length; i++)
        {
            leFichier.WriteLine("       {");
            leFichier.WriteLine($"           \"id\": {i + 1},");
            leFichier.WriteLine("           \"results\": [");
            for (int j = 0; j < joueur.Length; j++)
            {
                leFichier.WriteLine("               {");
                leFichier.WriteLine($"                   \"id_player\": {joueur[j].id},");
                leFichier.WriteLine($"                   \"dice\": [{string.Join(",", joueur[j].desParTour[i])}],");
                leFichier.WriteLine($"                   \"challenge\": \"{CHALLENGE[joueur[j].challengeUtiliser[i]-1].nom}\",");
                leFichier.WriteLine($"                   \"score\": {joueur[j].scoreParTour[i]}");
                leFichier.WriteLine(j == joueur.Length - 1 ? "               }" : "               },");
            }
            leFichier.WriteLine("           ]");
            leFichier.WriteLine(i == joueur[0].scoreParTour.Length - 1 ? "       }" : "       },");
        }
        leFichier.WriteLine("   ],");

        leFichier.WriteLine("   \"final_result\": [");
        for (int i = 0; i < joueur.Length; i++)
        {
            leFichier.WriteLine("       {");
            leFichier.WriteLine($"           \"id_player\": \"{joueur[i].id}\",");
            leFichier.WriteLine($"           \"bonus\": {joueur[i].bonus},");
            leFichier.WriteLine($"           \"score\": {(joueur[i].score + joueur[i].bonus)}");
            leFichier.WriteLine(i == joueur.Length - 1 ? "       }" : "       },");
        }
        leFichier.WriteLine("   ]");
        leFichier.WriteLine("}");

        leFichier.Close();
    }
}