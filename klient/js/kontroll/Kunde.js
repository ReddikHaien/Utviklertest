const request = require("../network/request.js");

const parametere = {
    /**
     * @type {HTMLInputElement}
     */
    kundeID: null,
    /**
     * @type {HTMLInputElement}
     */
    fornavn: null,
    /**
     * @type {HTMLInputElement}
     */
    etternavn: null,

    /**
     * @type {HTMLButtonElement}
     */
    kunderegBut: null
};

const display = {
    /**
     * @type {HTMLDivElement}
     */
    aktiveLån: null
};


function finnKunde() {
    let id = Number(parametere.kundeID.value);
    if (isNaN(id)) return;

    request.getKunde(id,
        (data) => {
            parametere.fornavn.setAttribute("disabled", "");
            parametere.etternavn.setAttribute("disabled", "");
            parametere.kunderegBut.style.display = "none";
            let kunde = JSON.parse(data);
            parametere.fornavn.value = kunde.fornavn;
            parametere.etternavn.value = kunde.etternavn;
            console.log("fant kunde " + kunde.fornavn + " " + kunde.etternavn);
            visLån(kunde.id);
        },
        (error) => {
            console.log("Fant ikke kunde");
            parametere.fornavn.removeAttribute("disabled");
            parametere.etternavn.removeAttribute("disabled");
            parametere.fornavn.value = "";
            parametere.etternavn.value = "";
            parametere.kunderegBut.style.display = "inline-block";
            display.aktiveLån.style.display = "none";
        });

}


function registrerKunde() {
    let id = Number(parametere.kundeID.value);
    let fornavn = parametere.fornavn.value;
    let etternavn = parametere.etternavn.value;

    request.registerKunde(id, fornavn, etternavn,
        (data) => {
            console.log("kunde ble registrert");
            finnKunde();
        },
        (error) => {
            console.log("feilet å registrere kunde", error);
        },(code)=>{
            if (code <= 300){
                console.log("kunde ble registrert");
                finnKunde();
            }
        }
    );
}

function visLån(id) {
    display.aktiveLån.style.display = "block";
    request.getLånForKunde(id,
        (data) => {
            let lån = JSON.parse(data);
            if (lån.length === 0){
                IngenLånDisplay();
            }else{
                let t = document.createElement("table");
                let body = document.createElement("tbody");
                t.id = "lånTable";
                t.appendChild(body);
                let headerRow = document.createElement("tr");
                let headerDato = document.createElement("td");
                headerDato.innerHTML = "Lånedato";
                let headerSum = document.createElement("td");
                headerSum.innerHTML = "Lånesum";
                let headerType = document.createElement("td");
                headerType.innerHTML = "Lånetype";


                let headerRente = document.createElement("td");
                headerRente.innerHTML = "Rente";

                let headerInnDato = document.createElement("td");
                headerInnDato.innerHTML = "Forrige innbetaling";

                let headerBetalt = document.createElement("td");
                headerBetalt.innerHTML = "Innbetalt";

                headerRow.id = "headerRow";

                headerRow.appendChild(headerDato);
                headerRow.appendChild(headerSum);
                headerRow.appendChild(headerType);
                headerRow.appendChild(headerRente);
                headerRow.appendChild(headerInnDato);
                headerRow.appendChild(headerBetalt);
                body.appendChild(headerRow);


                for (let i = 0; i < lån.length; i++){
                    let rad = document.createElement("tr");
                    let ldato = document.createElement("td");
                    
                    let dato = new Date(lån[i].laaneDato);
                    ldato.innerHTML = dato.getFullYear() + "." + dato.getMonth() + "." + dato.getDate();
                    rad.appendChild(ldato);

                    let lsum = document.createElement("td");
                    lsum.innerHTML = lån[i].laaneSum;
                    rad.appendChild(lsum);                   

                    let ltype = document.createElement("td");
                    ltype.innerHTML = lån[i].laaneType.navn;
                    rad.appendChild(ltype);

                    let lrente = document.createElement("td");
                    lrente.innerHTML = lån[i].laaneType.rente.toString();
                    rad.appendChild(lrente);

                    let innDato = document.createElement("td");
                    dato = new Date(lån[i].forrigeBetaling);
                    innDato.innerHTML = dato.getFullYear() +"."+dato.getMonth() + "." + dato.getDate();
                    rad.appendChild(innDato);

                    let innBetalt = document.createElement("td");
                    innBetalt.innerHTML = lån[i].innbetalt.toString();
                    rad.appendChild(innBetalt);

                    body.appendChild(rad);
                    console.log(lån[i]);
                }

                display.aktiveLån.innerHTML = "";
                display.aktiveLån.appendChild(t);
            }

        },
        (error) => {
            console.log(error);
        });
}

function IngenLånDisplay(){
    let h = document.createElement("p");
    h.classList.add("melding");
    h.innerText = "Ingen lån å vise";
    display.aktiveLån.innerHTML = "";
    display.aktiveLån.appendChild(h);
}


exports.init = function () {
    parametere.kundeID = document.getElementById("kundeId");
    parametere.fornavn = document.getElementById("Fornavn");
    parametere.etternavn = document.getElementById("Etternavn");
    parametere.kunderegBut = document.getElementById("kunderegBut");

    parametere.kundeID.onchange = finnKunde;

    parametere.kunderegBut.onclick = registrerKunde;

    display.aktiveLån = document.getElementById("aktiveLån");
};


exports.visLån = visLån;