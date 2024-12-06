const apiUrl = 'http://yams.iutrs.unistra.fr:3000/api/games/kl3r67pybl/rounds/';



//Fonction de requete a l'API

async function APIrequest(player, round){

    //Selection des balises ou seront affichees les donnees renvoyees par l'API
    const currentRound = document.getElementById('ntour')
    const playerDices = document.getElementById('player'+ player + 'Dices');
    const playerChallenge = document.getElementById('player' + player + 'Challenges');
    const playerScore = document.getElementById('player' + player + 'Score');

    //Requete a l'API
    const response = await fetch(apiUrl + round);

    //Check des erreurs
    if(!response.ok){
        alert('Erreur de récupération des données : ${response.status}');

    }

    //Preparation des donnees renvoyees par l'API
    const data = await response.json();
    const playerDatas = data.results[player];

    //Affichage des donnes sur la page web (details)
    currentRound.textContent = JSON.stringify('Tour n°' + data.id);
    playerDices.textContent = JSON.stringify(playerDatas.dice);
    playerChallenge.textContent = JSON.stringify(playerDatas.challenge);
    playerScore.textContent = JSON.stringify(playerDatas.score);






}

APIrequest(0, 1);
APIrequest(1, 1);