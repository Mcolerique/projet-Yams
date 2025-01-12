function Yams(){
  fetch("http://yams.iutrs.unistra.fr:3000/api/games/kl3r67pybl/players")
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
      const header = document.getElementsByTagName("header")[0];
      codeHTML = `
        <form></form>
        
        `;
        header.innerHTML= codeHTML ;
    })
    .catch((error) => {
      // Code pour gérer les erreurs
      console.error("erreure de chargement de l'API", error);
    });
    
  fetch("http://yams.iutrs.unistra.fr:3000/api/games/kl3r67pybl/players")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Erreur !");
      }
      return response.json();
    })
    .then((data) => {
      const section = document.getElementsByTagName("section")[0];
      codeHTML = `
      
            <div>Joueur ${data[0].id} : ${data[0].pseudo} </div>
            <div>Joueur ${data[1].id} : ${data[1].pseudo} </div>
       `;
      section.innerHTML= codeHTML ;
    })
    .catch((error) => {
      console.error("erreure de chargement de l'API", error);
    });

  fetch("http://yams.iutrs.unistra.fr:3000/api/games/kl3r67pybl/rounds/1")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Erreur !");
      }
      return response.json();
    })
    .then((data) => {
      const section = document.getElementsByTagName("article")[0];
      codeHTML = `
      <p>Tour n°${data.id}</p>
      <ul>   
            <li> Joueur ${data.results[0].id_player}</li>
            <li>Dés obtenue : `;
      for (i = 0; i < data.results[0].dice.length; i++) {
        codeHTML = codeHTML + data.results[0].dice[i];
      section.innerHTML= codeHTML
      }
      codeHTML = `
            </li>
            <li>Challenge choisi : ${data.results[0].challenge}</li>
            <li>Nombre de point gagnés : ${data.results[0].score} </li>
        </ul>

        <ul>
            <li> Joueur ${data.results[1].id_player}</li>
            <li>Dés obtenue :`;
      for (i = 0; i < data.results[1].dice.length; i++) {
        codeHTML = codeHTML + data.results[1].dice[i];
      }
      codeHTML = codeHTML + "</li>";
      section.innerHTML= codeHTML; 
      codeHTML = `
            <li>Challenge choisi : ${data.results[1].challenge}</li>
            <li>Nombre de point gagnés : ${data.results[1].score} </li>
        </ul>
        <p>Tour n°${data.id}</p>`;
        section.innerHTML= codeHTML ;
    })
    .catch((error) => {
      console.error("erreure de chargement de l'API", error);
    });
  fetch("http://yams.iutrs.unistra.fr:3000/api/games/kl3r67pybl/final-result")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Erreur !");
      }
      return response.json();
    })
    .then((data) => {
      const footer = document.getElementsByTagName("footer")[0];
      codeHTML = `
        <p>Score finale : ${data[0].score} / ${data[1].score}</p>
        `;
      footer.innerHTML = codeHTML;
    })
    .catch((error) => {
      console.error("erreure de chargement de l'API", error);
    });
}


