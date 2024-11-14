using System;
class Yams{
    struct Challenge{
        string nom;
        string objectif;
        string nbPoint;
    }
    struct joueur{
        int id;
        string pseudo;
        int score;
    }
    struct partie{
        Joueur[] joueur;
        int tours;
        
    }
    static void Main(){
        DateTime thisDay = DateTime.Today;
    }

}