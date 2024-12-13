using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Yams{
    // Structure représentant un défi
    public struct Challenge
    {
        public string nom;
        public string nomAfficher; // Nom affiché du défi
        public string description; // Description du défi
        public Challenge(string n, string nA, string desc)
        {
            nom = n;
            nomAfficher = nA;
            description = desc;
        }
    }
    // Structure représentant un joueur
    public struct Joueur
    {
        public int id;                       // ID unique du joueur
        public string pseudo;                // Nom du joueur
        public int[] scoreParTour;           // Tableau des scores par tour
        public int[][] desParTour;           // Historique des dés par tour
        public int score;                    // Score total
        public int bonus;                    // Bonus éventuel
        public List<int> challengeDispo;     // Défis disponibles
        public List<int> challengeUtiliser;  // Défis déjà utilisés
        public Joueur(int i, string pse)
        {
            id=i;
            pseudo=pse;
            scoreParTour = new int[13];
            desParTour = new int[13][];
            bonus = 0;
            score = 0;
            challengeDispo = new List<int>();
            for(int j=0; j<CHALLENGE.Length; j++)
            {
                challengeDispo.Add((j+1));
            }
            challengeUtiliser = new List<int>();
        }
    }

    // Liste des défis disponibles
    public static Challenge[] CHALLENGE = new Challenge[]
                                                {
                                                    // Challenges mineurs
                                                    new Challenge("nombre1", "Nombre de 1", "Obtenir le maximum de 1  ;  Somme des dés ayant obtenu 1"),
                                                    new Challenge("nombre2", "Nombre de 2", "Obtenir le maximum de 2  ;  Somme des dés ayant obtenu 2"),
                                                    new Challenge("nombre3", "Nombre de 3", "Obtenir le maximum de 3  ;  Somme des dés ayant obtenu 3"),
                                                    new Challenge("nombre4", "Nombre de 4", "Obtenir le maximum de 4  ;  Somme des dés ayant obtenu 4"),
                                                    new Challenge("nombre5", "Nombre de 5", "Obtenir le maximum de 5  ;  Somme des dés ayant obtenu 5"),
                                                    new Challenge("nombre6", "Nombre de 6", "Obtenir le maximum de 6  ;  Somme des dés ayant obtenu 6"),

                                                    // Challenges majeurs
                                                    new Challenge("brelan", "Brelan", "Obtenir 3 dés de même valeur  ;  Somme des 3 dés identiques"),
                                                    new Challenge("carre", "Carré", "Obtenir 4 dés de même valeur  ;  Somme des 4 dés identiques"),
                                                    new Challenge("full", "Full", "Obtenir 3 dés de même valeur + 2 dés de même valeur  ;  25 points"),
                                                    new Challenge("petite", "Petite suite", "Obtenir 1-2-3-4 ou 2-3-4-5 ou 3-4-5-6  ;  30 points"),
                                                    new Challenge("grande", "Grande suite", "Obtenir 1-2-3-4-5 ou 2-3-4-5-6  ;  40 points"),
                                                    new Challenge("yams", "Yam's", "Obtenir 5 dés de même valeur  ;  50 points"),
                                                    new Challenge("chance", "Chance", "Obtenir le maximum de points  ;  Le total des dés obtenus")
                                                };

    private static readonly Random rnd = new Random(); // Instance partagée pour générer des nombres aléatoires
    static void Main()
    {
        bool encoreJouer=true;
        while(encoreJouer)
        {
            jeu();
            Console.WriteLine("Voulez vous relancer une partie ? o/n");
            if(Console.ReadKey().KeyChar=='n')
            {
                encoreJouer=false;
            }
        }
    }
    public static void jeu(){
        Joueur[] joueur = initialisationJeu();                // Initialisation des joueurs
        for(int i=0; i<CHALLENGE.Length; i++)                 // Autant de tour qu'il y a de Challenge
        {
            Console.WriteLine();
            Console.WriteLine($"Tour n°{i+1}");
            Console.WriteLine();

            for(int j=0; j<joueur.Length; j++)               // Tour de chaque joueur
            {
                Console.WriteLine($"Tour de {joueur[j].pseudo} :");
                tour(ref joueur[j],i);

                Console.WriteLine($"vous avez choisi le challenge \"{CHALLENGE[joueur[j].challengeUtiliser[i]-1].nomAfficher}\"");
                Console.WriteLine("vous aviez les des suivant : ");
                afficheDes(joueur[j].desParTour[i]);
                Console.WriteLine($"Vous avez gagné {joueur[j].scoreParTour[i]} points");

                joueur[j].score += joueur[j].scoreParTour[i];
                if(joueur[j].bonus != 35 && joueur[j].challengeUtiliser[i] <= 6)
                {
                    joueur[j].bonus = verifBonus(joueur[j]);
                }

                Console.WriteLine($"Pour l'instant vous avez un total de {joueur[j].score} point + {joueur[j].bonus} bonus");
                Console.WriteLine();
            }
        }
        affichageFinPartie(ref joueur);        //Affiche les scores finaux
        creaJson(joueur);                      // Génère un fichier JSON avec les résultats
    }
    public static void tour(ref Joueur joueur, int tours){
        int[] des = new int[5];                                           // Tableau contenant les résultats des 5 dés
        bool[] relancerDes = new bool[5] {true,true,true,true,true};      // Indique si chaque dé doit être relancé
        int i=0;                                                          // Compteur du nombre de relances
        bool fin=false;                                                   // Variable de contrôle pour arrêter la boucle

        while(i<3 && !fin)                  // Boucle permettant jusqu'à 3 lancers de dés ou jusqu'à ce que le joueur décide d'arrêter
        {
            for(int j=0; j<des.Length; j++) // Parcourt chaque dé
            {
                if(relancerDes[j]==true)    // Relance le dé uniquement si marqué pour relance
                {
                    des[j]=rnd.Next(1,7);   // Génère un nouveau résultat pour le dé (entre 1 et 6)
                }
            }
            if(i<2)                         // Permet au joueur de choisir les dés à relancer si ce n'est pas le dernier tour
            {
                choixDes(des,relancerDes);
            }
            fin = finRelance(relancerDes);  // Vérifie si tous les dés sont conservés et met fin à la boucle si c'est le cas
            i++;                            // Incrémente le compteur de tours
        }
        afficheDes(des);                              // Affiche les résultats finaux des dés
        int codeChallenge = choixChallenge(joueur);   // Demande au joueur de choisir un challenge

        joueur.challengeUtiliser.Add(codeChallenge);              // Ajoute le challenge choisi à la liste des défis utilisés par le joueur
        supprimeChallenge(joueur.challengeDispo, codeChallenge);  // Supprime le challenge choisi de la liste des défis disponibles

        joueur.scoreParTour[tours]=calculScore(ref joueur,des,codeChallenge);  // Calcule et enregistre le score du joueur pour ce tour
        joueur.desParTour[tours] = des;                                        // Enregistre les dés du joueur pour ce tour
    }
    public static void choixDes(int[] des, bool[] relancerDes)
    {
        Console.WriteLine("Voici vos dès :");
        afficheDes(des);       // Affiche les résultats actuels des dés
        char reponse;          // Stocke la réponse de l'utilisateur
        for(int i=0; i<5; i++) // Parcourt chaque dé pour demander si le joueur souhaite le conserver
        {
            Console.Write("Voulez vous gardez le {0} ? y/n  ", des[i]);
            reponse = Console.ReadKey().KeyChar;  // Lit la réponse du joueur
            Console.WriteLine();
            if(reponse=='y')    // Si le joueur répond 'y', le dé est conservé
            {
                relancerDes[i]=false;  // Le dé ne sera pas relancé
            }
            else
            {
                relancerDes[i]=true;   // Le dé sera relancé
            }      
        }
        Console.WriteLine();           // Ajoute une ligne vide pour plus de lisibilité
    }
    public static bool finRelance(bool[] tab)
    {
        for(int i=0; i<tab.Length; i++)  // Vérifie si tous les dés sont conservés
        {
            if(tab[i]==true)             // Si un dé est encore marqué pour relance
            {
                return false;            // La relance n'est pas terminée
            }
        }
        return true;                     // Tous les dés sont conservés, la relance est terminée
    }
    public static int calculScore(ref Joueur j, int[] des, int codeChallenge)
    {
        int nbPoint=0;          // Variable pour stocker le score calculé
        if(codeChallenge <= 6)  // Cas des challenges 1 à 6 : score basé sur les occurrences d'un numéro spécifique
        {
            nbPoint = codeChallenge * nbOcc(des,codeChallenge);  // Multiplie le numéro par son nombre d'occurrences
            return nbPoint;                                      // Retourne le score
        }
        // Gère les autres types de challenges à l'aide d'un switch
        switch(codeChallenge)
        {
            case 7:
            nbPoint = verifBrelan(des);  // Vérifie si un brelan est présent
            break;

            case 8:
            nbPoint = verifCarre(des);  // Vérifie si un carré est présent
            break;

            case 9:
            nbPoint = verifFull(des);  // Vérifie si un Full House est présent
            break;

            case 10:
            if(verifPetiteSuite(des))  // Vérifie si une petite suite est présente
            {
                nbPoint = 30;          // Attribue 30 points
            }
            break;

            case 11:
            if(verifGrandeSuite(des))  // Vérifie si une grande suite est présente
            {
                nbPoint = 40;          // Attribue 40 points
            }
            break;

            case 12:
            if(nbOcc(des,des[0])==5)   // Vérifie si tous les dés affichent le même numéro
            {
                nbPoint=50;            // Attribue 50 points pour un Yahtzee
            }
            break;

            case 13:
            nbPoint = somme(des);      // Calcule la somme de tous les dés
            break;

            default:
            Console.WriteLine("ya une erreur la mon reuf");  // Message d'erreur pour un codeChallenge invalide
            break;
        }
        return nbPoint;  // Retourne le score calculé
    }
    public static int nbOcc(int[] t, int nb)
    {
        int cpt = 0;   // Compteur d'occurrences
        for(int i=0; i<t.Length;i++)  // Parcourt le tableau pour compter les occurrences du numéro donné
        {
            if (t[i]==nb)
            {
                cpt++;
            }
        }
        return cpt;  // Retourne le nombre d'occurrences
    }   
    public static int somme(int[] tab)
    {
        int somme=0;  // Initialise la somme à 0
        for(int i=0; i<tab.Length; i++)  // Parcourt le tableau pour calculer la somme des valeurs
        {
            somme+=tab[i];
        }
        return somme;  // Retourne la somme calculée
    }
    public static void echange(int[] t,int i, int j  )
    {
        // Échange deux éléments dans un tableau
        int sauv = t[j];
        t[j]=t[i];
        t[i]=sauv;
    }
    public static void tri(int[] t)
    {
        // Tri simple par sélection
        for(int i=0;i<t.Length;i++)
        {
            for(int j=i+1;j<t.Length;j++)
            {
                if(t[i]>t[j])
                {
                    echange(t,i,j); // Échange les éléments mal placés
                }
            }
        }
    }
    public static int verifBrelan(int[] des)
    {
        List<int> nbATester = listeNombreDifferent(des);  // Liste des valeurs uniques à tester
        if(nbATester.Count>3)  // Trop de valeurs différentes pour un brelan
        {
            return 0;
        }
        else
        {
            int nbOccurence;
            for(int i=0; i<nbATester.Count; i++)  // Vérifie si une valeur est présente au moins 3 fois
            {
                nbOccurence = nbOcc(des,nbATester[i]);
                if(nbOccurence >=3)
                {
                    return nbATester[i]*3;  // Retourne le score basé sur la valeur du brelan
                }
            }
        }
        return 0;  // Aucun brelan trouvé
    }
    public static int verifCarre(int[] des)
    {
        List<int> nbATester = listeNombreDifferent(des);  // Liste des valeurs uniques à tester
        if(nbATester.Count>2)                             // Trop de valeurs différentes pour un carré
        {
            return 0;
        }
        else
        {
            int nbOccurence;
            for(int i=0; i<nbATester.Count; i++)          // Vérifie si une valeur est présente au moins 4 fois
            {
                nbOccurence =nbOcc(des,nbATester[i]);
                if(nbOccurence >=4)
                {
                    return nbATester[i]*4;                // Retourne le score basé sur la valeur du carré
                }
            }
        }
        return 0;                                         // Aucun carré trouvé
    }
    public static int verifFull(int[] des)
    {
        List<int> nbATester = listeNombreDifferent(des);  // Liste des valeurs uniques à tester
        if(nbATester.Count>2)                             // Trop de valeurs différentes pour un Full
        {
            return 0;
        }
        else
        {
            int nbOccurence =nbOcc(des,nbATester[0]);
            if(nbOccurence ==3 || nbOccurence ==2)        // Vérifie si les valeurs sont présentes en 3+2 ou 2+3
            {
                return 25;                                // Retourne le score pour un Full
            }
        }
        return 0;                                         // Aucun Full trouvé
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
        // Génère une liste de nombres uniques présents dans les dés
        List<int> listeNombre = new List<int>();
        for(int i=1; i<=6; i++)
        {
            if(des.Contains(i))
            {
                listeNombre.Add(i);
            }
        }
        return listeNombre;  // Retourne la liste des nombres uniques
    }
    public static int choixChallenge(Joueur j)
    {
        bool choixValide = false;   // Indique si le choix est valide
        int choix = 0;              // Stocke le choix du joueur

        afficheChallenges(j);       // Affiche la liste des challenges disponibles
        Console.WriteLine("Quel challenge voulez vous choisir ?");

        while(!choixValide)         // Boucle jusqu'à ce que le joueur fasse un choix valide
        {
            try
            {
                choix = int.Parse(Console.ReadLine());  // Lit l'entrée utilisateur et convertit en entier
                Console.WriteLine();

                if(choix==0)            //Si l'utilisateur veut afficher les descriptions des challenges
                {
                    afficheChallengesDetaillé(j);  // Affiche les descriptions détaillées des challenges
                    Console.WriteLine("Quel challenge voulez vous choisir ?");
                    choix = int.Parse(Console.ReadLine());  // Lit un nouveau choix de challenge

                    if(!j.challengeDispo.Contains(choix))   // Vérifie si le choix est valide
                    {
                        Console.WriteLine("Veullez choisir un challenge valide");  // Message d'erreur
                    }
                    else
                    {
                        choixValide = true;  // Le choix est valide
                        return choix;        // Retourne le choix
                    }  
                }
                else if(!j.challengeDispo.Contains(choix))  // Si le choix n'est pas dans les challenges disponibles
                {
                    Console.WriteLine("Veullez choisir un challenge valide");  // Message d'erreur
                }
                else
                {
                    choixValide = true;  // Le choix est valide
                    return choix;        // Retourne le choix
                }  
            }
            catch (FormatException) // Capture l'erreur si l'entrée n'est pas un entier valide
            {
                Console.WriteLine("Erreur : veuillez entrer un nombre entier valide.");
            }
            catch (Exception ex) // Capture toute autre erreur éventuelle
            {
                Console.WriteLine($"Une erreur inattendue est survenue : {ex.Message}");
            }
        }
        return choix;  // Retourne le choix (ne sera jamais atteint dans la logique actuelle)
    }
    public static void supprimeChallenge(List<int> challenge, int choix)
    {
        for(int i=0; i<challenge.Count; i++)  // Parcourt la liste des challenges disponibles
        {
            if(challenge[i]==choix)           // Si le choix correspond à un élément dans la liste
            {
                challenge.RemoveAt(i);        // Supprime cet élément de la liste
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
        Console.WriteLine("0. Description des challenges");  // Affiche une option pour afficher les descriptions des challenges
        for(int i=0; i<j.challengeDispo.Count; i++)          // Affiche les challenges disponibles avec leurs noms courts
        {
            Console.WriteLine($"{j.challengeDispo[i]}. {CHALLENGE[j.challengeDispo[i]-1].nomAfficher}");
        }
        Console.WriteLine();
    }
    public static void afficheChallengesDetaillé(Joueur j)
    {
        for(int i=0; i<j.challengeDispo.Count; i++)  // Affiche les challenges disponibles avec leurs descriptions détaillées
        {
            Console.WriteLine($"{j.challengeDispo[i]}. {CHALLENGE[j.challengeDispo[i]-1].nomAfficher} ; {CHALLENGE[j.challengeDispo[i]-1].description} ;");
        }
        Console.WriteLine();
    }
    public static Joueur[] initialisationJeu()
    {
        int nbJoueurs = 0; // Déclare nbJoueurs avant le try
        bool fin = false;

        while (!fin)
        {
            try
            {
                Console.WriteLine("Combien de joueurs ?");
                nbJoueurs = int.Parse(Console.ReadLine());
            
                if (nbJoueurs <= 0)
                {
                    Console.WriteLine("Erreur : le nombre de joueurs doit être supérieur à zéro.");
                    continue;
                }

                fin = true; // Sort de la boucle si l'entrée est correcte
            }
            catch (FormatException) // Capture l'erreur si l'entrée n'est pas un entier valide
            {
                Console.WriteLine("Erreur : veuillez entrer un nombre entier valide.");
            }
            catch (Exception ex) // Capture toute autre erreur éventuelle
            {
                Console.WriteLine($"Une erreur inattendue est survenue : {ex.Message}");
            }
        }

        Joueur[] j = new Joueur[nbJoueurs]; // Crée un tableau de joueurs
        for (int i = 0; i < nbJoueurs; i++)
        {
            Console.Write($"Entrez le pseudo du joueur {i + 1} : ");
            string ps = Console.ReadLine();
            j[i] = new Joueur(i + 1, ps); // Initialise chaque joueur avec un ID et un pseudo
        }

        return j; // Retourne le tableau contenant les joueurs
    }
    public static void affichageFinPartie(ref Joueur[] joueur)
    {
        int[] points = new int[joueur.Length];         // Tableau pour stocker les scores totaux des joueurs
        
        // Affiche le score de chaque joueur et calcule leur total (score + bonus)
        for(int i = 0; i<joueur.Length; i++)
        {
            points[i] = (joueur[i].score + joueur[i].bonus);  // Calcul du score total (score + bonus)
            Console.WriteLine($"{joueur[i].pseudo} a marqué {joueur[i].score} points et {joueur[i].bonus} points bonus se qui fait un total de {points[i]} points");
            Console.WriteLine();
        }
        
        int indexVictorieux = indexGagnant(points);   // Trouve l'index du gagnant, ou -1 en cas d'égalité

        // Affiche le vainqueur ou déclare une égalité
        if(indexVictorieux != -1)
        {
            Console.WriteLine($"Victoire de {joueur[indexVictorieux].pseudo}");  // Affiche le pseudo du gagnant
        }
        else
        {
            Console.WriteLine("Il y a une égalité");  // Aucun gagnant, égalité
        }
        Console.WriteLine();
    }
    public static int indexGagnant(int[] tab)
    {
        int max = tab[0]; // Initialisation du score maximum avec le premier élément
        int indexMax = 0; // Index du joueur avec le score maximum

        // Parcourt le tableau pour trouver le score maximum et son index
        for (int i = 1; i < tab.Length; i++)
        {
            if (tab[i] > max)
            {
                max = tab[i]; // Met à jour le score maximum
                indexMax = i; // Met à jour l'index correspondant
            }
        }

        return verifEgaliter(tab, indexMax); // Vérifie si le score maximum est unique ou s'il y a égalité
    }

    public static int verifEgaliter(int[] tab, int index)
    {
        int cptMax = 0; // Compteur pour vérifier combien de joueurs ont le score maximum

        // Compte le nombre de joueurs ayant le même score que le maximum
        for (int i = 0; i < tab.Length; i++)
        {
            if (tab[i] == tab[index])
            {
                cptMax++;
            }
        }

        // Si plus d'un joueur a le score maximum, il y a égalité
        if (cptMax > 1)
        {
            return -1; // Retourne -1 pour indiquer une égalité
        }

        return index; // Retourne l'index du joueur gagnant s'il est unique
    }
    public static int verifBonus(Joueur j)
    {
        int scoreChallengeMineure=0;  // Cumul des scores des challenges mineurs
        for(int i=0; i<j.challengeUtiliser.Count; i++)  // Parcourt les challenges utilisés pour calculer le score des challenges mineurs (1 à 6)
        {
            if(j.challengeUtiliser[i]<=6)  
            {
                scoreChallengeMineure += j.scoreParTour[i];
            }
        }
        if(scoreChallengeMineure >= 63)  // Si le score des challenges mineurs atteint ou dépasse 63, un bonus de 35 points est attribué
        {
            return 35;
        }
        return 0;  // Pas de bonus
    }
    public static void creaJson(Joueur[] joueur)
    {
        // Génère un nom de fichier JSON unique basé sur les pseudos des joueurs et la date actuelle
        string date = DateTime.Today.ToString("yyyy-MM-dd");
        string nomJson = "";

        for(int i = 0; i<joueur.Length; i++)
        {
            nomJson += joueur[i].pseudo+"_";
        }
        nomJson += date+".json";


        // Crée un flux pour écrire dans un fichier
        FileStream fs = new FileStream(nomJson, FileMode.Create, FileAccess.Write);
        StreamWriter leFichier = new StreamWriter(fs);

        // Écriture des métadonnées dans le fichier JSON
        leFichier.WriteLine("{");
        leFichier.WriteLine("   \"parameters\": {");
        leFichier.WriteLine("       \"code\": \"groupe8-003\",");
        leFichier.WriteLine($"       \"date\": \"{date}\"");
        leFichier.WriteLine("   },");

        // Ajout des informations des joueurs
        leFichier.WriteLine("   \"players\": [");
        for (int i = 0; i < joueur.Length; i++)
        {
            leFichier.WriteLine("       {");
            leFichier.WriteLine($"           \"id\": {joueur[i].id},");
            leFichier.WriteLine($"           \"pseudo\": \"{joueur[i].pseudo}\"");
            leFichier.WriteLine(i == joueur.Length - 1 ? "       }" : "       },");
        }
        leFichier.WriteLine("   ],");

        // Ajout des résultats des rounds
        leFichier.WriteLine("   \"rounds\": [");
        for (int i = 0; i < CHALLENGE.Length; i++)
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
            leFichier.WriteLine(i == CHALLENGE.Length - 1 ? "       }" : "       },");
        }
        leFichier.WriteLine("   ],");

        // Ajout des résultats finaux
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

        leFichier.Close();  // Ferme le fichier
    }
}