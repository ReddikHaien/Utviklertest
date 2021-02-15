const request = require("../network/request.js");
const Kunde = require("./Kunde.js");
const formatter = require("../util/formatering.js");

const parameterInput = {
    /**
     * @type {HTMLSelectElement}
     */
    type: null,
    /**
     * @type {HTMLInputElement}
     */
    sum: null,
    /**
     * @type {HTMLInputElement}
     */
    år: null,

    /**
     * @type {HTMLInputElement}
     */
    kundeId: null,
    /**
     * @type {HTMLSelectElement}
     */
    skjema: null,
};

function taOppLån(){
    let år = Number(parameterInput.år.value);
    let sum = Number(parameterInput.sum.value);
    let type = Number(parameterInput.type.options[parameterInput.type.selectedIndex].value);
    let kunde = Number(parameterInput.kundeId.value);
    let skjema = Number(parameterInput.skjema.options[parameterInput.skjema.selectedIndex]);

    if (!isNaN(kunde)){
        request.taOppLån(kunde,type,sum,år,(data) =>{
            console.log("data",data);
        },(error) =>{
            console.log("feil",error);
        },(kode) =>{
            if (kode < 400){
                console.log("lån tatt opp");
                Kunde.visLån(kunde);
            }
            else{
                console.log("noe annet " + kode);
            }
        });
    }
}

function BeregnButton(){
    
    let år = Number(parameterInput.år.value);
    let sum = Number(parameterInput.sum.value);
    let type = Number(parameterInput.type.options[parameterInput.type.selectedIndex].value);
    let skjema = Number(parameterInput.skjema.options[parameterInput.skjema.selectedIndex].value);

    request.getBetalingsPlan(år,sum,type,skjema,(d) =>{
        
        let res = JSON.parse(d);
        console.log(res);

        let genInfoT = document.createElement("table");
        genInfoT.style.width = "100%";
        let genInfoB = genInfoT.appendChild(document.createElement("tbody"));
        genInfoB.style.width = "100%";
        let genInfoR = genInfoB.appendChild(document.createElement("tr"));
        genInfoR.style.width = "100%";
        let totalSum = genInfoR.appendChild(document.createElement("td"));
        totalSum.style.width = "100";
        totalSum.innerHTML = "Total sum: " + res.totalSum.toFixed(2);

        let theader = document.createElement("table");
        theader.style.width = "100%";
        
        let h = document.createElement("thead");
        
        h.appendChild(formatter.lagTabellRad("Dato","Avdrag","Rente","Sum","Gjenstående lån"));

        let b = document.createElement("tbody");
        b.style.width = "100%";

        let dato = new Date();

        let gjenstående = sum;
        for (let i = 0; i < res.betalinger.length; i++){
            gjenstående -= res.betalinger[i].avdrag;
            dato.setMonth(dato.getMonth() + 1);
            let rad = formatter.lagTabellRad(
                dato.toLocaleDateString(),
                formatter.formatValuta(res.betalinger[i].avdrag),
                formatter.formatValuta(res.betalinger[i].rente),
                formatter.formatValuta(res.betalinger[i].rente + res.betalinger[i].avdrag),
                formatter.formatValuta(gjenstående)
            );

            b.appendChild(rad);
        }

        theader.appendChild(h);
        
        //Wrapper for å kunne skrolle tabellen
        let tbodycontainer = document.createElement("div");
        tbodycontainer.style.overflow = "auto";
        tbodycontainer.style.width = "100%";
        tbodycontainer.style.height = "200px";

        let tbodytable = tbodycontainer.appendChild(document.createElement("table"));
        tbodytable.style.width = "100%";

        tbodytable.appendChild(b);

        let taOppLånBut = document.createElement("button");
        taOppLånBut.innerHTML = "Ta opp lån";
        taOppLånBut.onclick = taOppLån;
        taOppLånBut.style.marginTop = "10px";
        document.getElementById("resultat").innerHTML = "";
        document.getElementById("resultat").appendChild(genInfoT);
        document.getElementById("resultat").appendChild(theader);
        document.getElementById("resultat").appendChild(tbodycontainer);
        document.getElementById("resultat").appendChild(taOppLånBut);

    },(e) =>{
        document.getElementById("resultat").innerText = e;
    });
}

exports.init = function(){
        
    //Henter inn inputobjektene 
    parameterInput.type = document.getElementById("lånetype");
    parameterInput.sum = document.getElementById("sum");
    parameterInput.år = document.getElementById("år");
    parameterInput.kundeId = document.getElementById("kundeId");
    parameterInput.skjema = document.getElementById("skjema");
    //Setter opp knapper
    document.getElementById("beregn").onclick = BeregnButton;
    parameterInput.type.onchange = selectorOnChange;    
    
    //Henter lånetyper fra server

    request.getLånetyper(
        (data) =>{
            let obj = JSON.parse(data);

            obj.forEach(t =>{

                let child = document.createElement("option");
                child.value = t.id;
                child.text = t.navn; 
                
                parameterInput.type.appendChild(child);

                
            });


            parameterInput.type.onchange();
        },(error) =>{

            document.getElementById("resultat").innerText = error;
        }
    );
    
    request.getSkjemaer(
        data =>{
            console.log(data);
            let typer = JSON.parse(data);
            for (let i = 0; i < typer.length; i++){
                let opt = parameterInput.skjema.appendChild(document.createElement("option"));
                opt.value = typer[i].id;
                opt.innerText = typer[i].name;
            }
        },
        error =>{
            console.log(error);
        }
    );
};

function selectorOnChange(){
    let id = this.options[this.selectedIndex].value;
    request.getLåneType(id,(data) =>{
        let t = JSON.parse(data);
        document.getElementById("renteVisning").innerHTML = "Rente: " + t.rente;
    },
    ()=>{});
}