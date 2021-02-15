const ValutaFormatter = new Intl.NumberFormat("no-nb",{maximumFractionDigits: 2, minimumFractionDigits: 2});

exports.formatValuta = function(tall){ return ValutaFormatter.format(tall) + "kr"; };

const ÅrFormatter = new Intl.NumberFormat("no-nb");

exports.formatÅr = function(tall){ return ÅrFormatter.format(tall); };

exports.lagTabellRad = function(...data){
    let width = (100 / data.length).toFixed(2) + "%";
    
    let rad = document.createElement("tr");

    for (let i = 0; i < data.length; i++){
        let k = rad.appendChild(document.createElement("td"));
        k.innerText = data[i];
        k.style.width = width;
    }

    return rad;
};