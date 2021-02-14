const request = require("../network/request.js");

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
};

function BeregnButton(){
    let år = parameterInput.år.value;
    let sum = parameterInput.sum.value;
    let type = parameterInput.type.options[parameterInput.type.selectedIndex].value;
    request.getBetalingsPlan(Number(år),Number(sum),Number(type),(d) =>{
        
        let res = JSON.parse(d);
        console.log(res);



        let theader = document.createElement("table");
        theader.style.width = "100%";
        
        let h = document.createElement("thead");
        
        let headerRow = document.createElement("tr");
        
        let headerDato = document.createElement("td");
        headerDato.innerHTML = "Dato";
        headerDato.style.width = "50%";

        let headerSum = document.createElement("td");
        headerSum.innerHTML = "Sum";
        

        headerRow.appendChild(headerDato);
        headerRow.appendChild(headerSum);
        h.appendChild(headerRow);

        let b = document.createElement("tbody");
        b.style.width = "100%";

        for (let i = 0; i < res.totalMndPris.length; i++){
            let rad = document.createElement("tr");
            
            let rdato = document.createElement("td");
            rdato.innerHTML = i;
            rdato.style.width = "50%";
            let rsum = document.createElement("td");
            rsum.innerHTML = res.totalMndPris[i].toFixed(2);


            rad.appendChild(rdato);
            rad.appendChild(rsum);
            b.appendChild(rad);
        }

        theader.appendChild(h);
        
        //Wrapper for å kunne skrolle tabellen
        let tbodycontainer = document.createElement("div");
        tbodycontainer.style.overflow = "auto";
        tbodycontainer.style.width = "100%";
        tbodycontainer.style.height = "150px";

        let tbodytable = document.createElement("table");
        tbodytable.style.width = "100%";

        tbodytable.appendChild(b);
        tbodycontainer.appendChild(tbodytable);

        document.getElementById("resultat").innerHTML = "";
        document.getElementById("resultat").appendChild(theader);
        document.getElementById("resultat").appendChild(tbodycontainer);

    },(e) =>{
        document.getElementById("resultat").innerText = e;
    });
}

window.onload = () =>{
};

exports.init = function(){
        
    //Henter inn inputobjektene 
    parameterInput.type = document.getElementById("lånetype");
    parameterInput.sum = document.getElementById("sum");
    parameterInput.år = document.getElementById("år");

    //Setter opp knapper
    document.getElementById("beregn").onclick = BeregnButton;
    

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
        },(error) =>{

            document.getElementById("resultat").innerText = error;
        }
    );
};