//DECLARATION DES VARIABLES ET DES CONSTANTES   
let currentRound = 1;
let apiUrl;

//FONCTION CREATION URL
function makeURl(gameID){
    apiUrl = 'http://yams.iutrs.unistra.fr:3000/api/games/' + gameID + '/';

}

//FONCTION VALIDATION DU FORMULAIRE ID DE PARTIE
function formSubmit(){
    //Recuperation de la saisie de l'utilisateur
    const form = document.getElementById('gameID');
    //Mise a jour de apiURL
    makeURl(form.elements['gameIDstring'].value);
    //Nouvelle requete avec apiURL a jour
    start();
}

//FONCTION REQUETE API POUR DETAILS TOUR PAR TOUR

function roundDetails(round){

  //Selection des balises ou seront affichees les donnees renvoyees par l'API
  const currentRound = document.getElementById('ntour')

  const player1Dices = document.getElementById('player'+ 0 + 'Dices');
  const player1Challenge = document.getElementById('player' + 0 + 'Challenges');
  document.getElementById('player' + 0 + 'Challenges');
  const player1Score = document.getElementById('player' + 0 + 'Score');

  const player2Dices = document.getElementById('player'+ 1 + 'Dices');
  const player2Challenge = document.getElementById('player' + 1 + 'Challenges');
  const player2Score = document.getElementById('player' + 1 + 'Score');

  //Requete a l'API
  fetch(apiUrl + 'rounds/' + round)
  .then((response) => {
    if(!response.ok){
        throw new Error("Erreur: Requête échouée!");
    }
    return response.json();

  })
  //Utilisation des données renvoyées par l'API
  .then((data => {

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

    //Affichage des donnees sur la page web (details)
    currentRound.textContent = 'Tour n°' + idTour;

    player1Dices.textContent = player1dices;
    player1Challenge.textContent = player1challenges;
    player1Score.textContent = player1score + ' pts';

    player2Dices.textContent = player2dices;
    player2Challenge.textContent = player2challenges;
    player2Score.textContent = player2score + ' pts';

  }))
  
}

//FONCTIONS MAJ DETAILS CHANGEMENT DE TOUR

function previousRound(){
    if(currentRound > 1){
        currentRound = currentRound - 1;
        roundDetails(currentRound);

    }
    else{
        alert("Erreur : le tour à afficher doit être compris entre 1 et 13!")

    }

}

function nextRound(){
    if(currentRound < 13){
        currentRound = currentRound + 1;
        roundDetails(currentRound);

    }
    else{
        alert("Erreur : le tour à afficher doit être compris entre 1 et 13!")

    }

}


//FONCTION REQUETE API POUR APERCU DE LA PARTIE
function mainInfos(){

    //Selection des balises ou seront affichees les donnees renvoyees par l'API
    const playersNames = document.getElementById('playersNames')

    const bestScore = document.getElementById('bestScore');
    const player1Challenge = document.getElementById('player' + 0 + 'Challenges');
    const player1Score = document.getElementById('player1Score');
    const player2Score = document.getElementById('player2Score');

    const date = document.getElementById('date');
    const playedCombs = document.getElementById('playedCombs');
    const winner = document.getElementById('winner');

    //TRAITEMENT DE LA DATE
    //Requete a l'API
    fetch(apiUrl + 'parameters/')
      .then((response) => {
        if(!response.ok){
            throw new Error("Erreur: Requête échouée!");
        }
        return response.json();

      })
      //Affichage des donnees sur la page web
      .then((data => {
        date.textContent = data.date;

      }))

    //TRAITEMENT DES NOMS DE JOUEURS
    //Requete a l'API
    fetch(apiUrl + 'players/')
      .then((response) => {
        if(!response.ok){
            throw new Error("Erreur: Requête échouée!");
        }
        return response.json();

      })
      //Affichage des donnees sur la page web
      .then((data => {
        playersNames.textContent = data[0].pseudo + ' et ' + data[1].pseudo;

      }))

    //TRAITEMENT DES SCORES ET DU GAGNANT
    //Requete a l'API
    fetch(apiUrl + 'final-result/')
    .then((response) => {
      if(!response.ok){
          throw new Error("Erreur: Requête échouée!");
      }
      return response.json();

    })
    //Affichage des donnees sur la page web
    .then((data => {
      player1Score.textContent = data[0].score + ' pts';
      player2Score.textContent = data[1].score + ' pts';
      if(data[0].score < data[1].score){
        winner.textContent = 'Joueur ' + data[1].id_player;
      }
      if(data[0].score > data[1].score){
        winner.textContent = 'Joueur ' + data[0].id_player;
      }
      if(data[0].score == data[1].score){
        winner.textContent = 'Egalité! ';
      }

    }))

}

function start(){
  if(document.title == 'Aperçu des résultats'){
      mainInfos();
  }
  if(document.title == 'Résultats tour par tour'){
      roundDetails(currentRound);

  }
}

makeURl('kl3r67pybl');
start();
