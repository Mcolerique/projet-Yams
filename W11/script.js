function Yams() {
  fetch("http://yams.iutrs.unistra.fr:3000/api")
    .then((response) => {
      if (!response.ok) {
        //Code pour gérer le cas où la réponse n'est pas correcte
        throw new Error("Erreur !"); //Le fetch s'arrête !
      }
      // Retourner le résultat au format JSON si réponse correcte
      return response.json();
    })
    .then((data) => {
      // Code pour traiter les données récupérées
      const body = document.getElementById(`body`);
      codeHTML = `
        <form></form>
        <nav>Tour n°${data.round[0].id}</nav>
        <ul>
            <li>Joueur ${data.players[0].id} : ${data.players[0].pseudo} </li>
            <li>Dés obtenue : `;
      for (i = 0; i < data.round[0].results[0].dice.length; i++)
        codeHTML = codeHTML + data.round[0].results[0].dice[i];
      codeHTML = `
            </li>
            <li>Challenge choisi : ${data.round[0].results[0].challenge}</li>
            <li>Nombre de point gagnés : ${data.round[0].results[0].score} </li>
        </ul>

        <ul>
            <li>Joueur ${players[1].id} : ${players[1].pseudo} </li>
            <li>Dés obtenue :`;
      for (i = 0; i < round[0].results[1].dice.length; i++)
        codeHTML = codeHTML + round[0].results[1].dice[i];
      codeHTML = `
            </li>
            <li>Challenge choisi : ${round[0].results[1].challenge}</li>
            <li>Nombre de point gagnés : ${round[0].results[1].score} </li>
        </ul>

        <footer>Score finale : ${final_result[0].score}</footer>
        `;
    })
    .catch((error) => {
      // Code pour gérer les erreurs
      console.error("");
    });
}
