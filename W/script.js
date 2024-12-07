//DECLARATION DES VARIABLES ET DES CONSTANTES   
let currentRound = 1;
const apiUrl = 'http://yams.iutrs.unistra.fr:3000/api/games/kl3r67pybl/';



//FONCTION REQUETE API POUR DETAILS TOUR PAR TOUR

async function roundDetails(player, round){

    //Selection des balises ou seront affichees les donnees renvoyees par l'API
    const currentRound = document.getElementById('ntour')
    const playerDices = document.getElementById('player'+ player + 'Dices');
    const playerChallenge = document.getElementById('player' + player + 'Challenges');
    const playerScore = document.getElementById('player' + player + 'Score');

    //Requete a l'API
    const response = await fetch(apiUrl + 'rounds/' + round);

    //Check des erreurs
    if(!response.ok){
        alert('Erreur de récupération des données : ' + response.status);

    }

    //Preparation des donnees renvoyees par l'API
    const data = await response.json();
    const playerDatas = data.results[player];

    //Stockage des donnes utilisees
    const idTour = data.id;
    let dice = playerDatas.dice;
    const challenges = playerDatas.challenge;
    const score = playerDatas.score;

    //Affichage des donnes sur la page web (details)
    currentRound.textContent = 'Tour n°' + idTour;
    playerDices.textContent = dice;
    playerChallenge.textContent = challenges;
    playerScore.textContent = score + ' pts';



}

//FONCTIONS CHANGEMENT DE TOUR

function previousRound(){
    if(currentRound > 1){
        currentRound = currentRound - 1;
        roundDetails(0, currentRound);
        roundDetails(1, currentRound);

    }
    else{
        alert("Erreur : le tour à afficher doit être compris entre 1 et 6!")

    }

}

function nextRound(){
    if(currentRound < 13){
        currentRound = currentRound + 1;
        roundDetails(0, currentRound);
        roundDetails(1, currentRound);

    }
    else{
        alert("Erreur : le tour à afficher doit être compris entre 1 et 6!")

    }

}

roundDetails(0, 1);
roundDetails(1, 1);

/*
LeftArrow = document.getElementById("LeftArrow");
RightArrow = document.getElementById("RightArrow");

LeftArrow.addEventListener('click', previousRound());
RightArrow.addEventListener('click', nextRound());
*/