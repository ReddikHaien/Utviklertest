const request = require("./js/network/request.js");
const kalkulator = require("./js/kontroll/Kalkulator.js");
const kunde = require("./js/kontroll/Kunde.js");

window.onload = () =>{
    kalkulator.init();
    kunde.init();
};


