//DECLARATION DES VARIABLES ET DES CONSTANTES   
let currentRound = 1;
const apiUrl = 'http://yams.iutrs.unistra.fr:3000/api/games/kl3r67pybl/';



//FONCTION REQUETE API POUR DETAILS TOUR PAR TOUR

async function roundDetails(round){

    //Selection des balises ou seront affichees les donnees renvoyees par l'API
    const currentRound = document.getElementById('ntour')

    const player1Dices = document.getElementById('player'+ 0 + 'Dices');
    const player1Challenge = document.getElementById('player' + 0 + 'Challenges');
    const player1Score = document.getElementById('player' + 0 + 'Score');

    const player2Dices = document.getElementById('player'+ 1 + 'Dices');
    const player2Challenge = document.getElementById('player' + 1 + 'Challenges');
    const player2Score = document.getElementById('player' + 1 + 'Score');

    //Requete a l'API
    const response = await fetch(apiUrl + 'rounds/' + round);

    //Check des erreurs
    if(!response.ok){
        alert('Erreur de récupération des données : ' + response.status);

    }

    //Preparation des donnees renvoyees par l'API
    const data = await response.json();
    const player1Datas = data.results[0];
    const player2Datas = data.results[1];

    //Stockage des donnes utilisees
    const idTour = data.id;

    let player1dices = player1Datas.dice;
    const player1challenges = player1Datas.challenge;
    const player1score = player1Datas.score;

    let player2dices = player2Datas.dice;
    const player2challenges = player2Datas.challenge;
    const player2score = player2Datas.score;



    //Affichage des donnes sur la page web (details)
    currentRound.textContent = 'Tour n°' + idTour;

    player1Dices.textContent = player1dices;
    player1Challenge.textContent = player1challenges;
    player1Score.textContent = player1score + ' pts';

    player2Dices.textContent = player2dices;
    player2Challenge.textContent = player2challenges;
    player2Score.textContent = player2score + ' pts';



}

//FONCTIONS MAJ DETAILS CHANGEMENT DE TOUR

function previousRound(){
    if(currentRound > 1){
        currentRound = currentRound - 1;
        roundDetails(currentRound);

    }
    else{
        alert("Erreur : le tour à afficher doit être compris entre 1 et 6!")

    }

}

function nextRound(){
    if(currentRound < 13){
        currentRound = currentRound + 1;
        roundDetails(currentRound);

    }
    else{
        alert("Erreur : le tour à afficher doit être compris entre 1 et 6!")

    }

}


/*//FONCTION REQUETE API POUR APERCU DE LA PARTIE
async function mainInfos(){


}
*/

roundDetails(1);

