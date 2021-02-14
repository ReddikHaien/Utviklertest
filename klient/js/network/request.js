const { net } = require("electron").remote;


/**
 * sender en request til serveren 
 * @param {string} type 
 * @param {string} uri 
 * @param {string} body
 * @param {Function} cbd callback for dataen mottatt 
 * @param {Function} cbe callback for error
 */
function makeRequest(type,uri,body,cbd,cbe,cbresponse=()=>{}){
    let request = net.request({
        method: type,
        protocol: "https:",
        hostname: "localhost",
        port: 44361,
        path: "api/"+uri,
    });
    console.log("sender request " + type + ": til " + uri);
    request.on("abort",function(){
        console.error("request avslått: ",type,uri);
    });
    request.on("error",cbe);
    request.on("response",(r) =>{
        console.log("fikk respons " + r.statusCode);
        cbresponse(r.statusCode);
        if (r.statusCode >= 400){
            r.on("data", d => cbe(d.toString()) );
            return;
        }
        r.on("data",d => cbd(d.toString()) );
    });

    request.setHeader("Content-type","application/json");
    request.write(body);
    request.end();
}

function getLånetyper(cbd,cbe){
    makeRequest("GET","LaaneType","",cbd,cbe);
}
exports.getLånetyper = getLånetyper;

function getBetalingsPlan(år,sum,type,cbd,cbe){
    let info = {
        LaaneTypeId: type,
        Sum: sum,
        Aar: år,
    };
    makeRequest("POST","BetalingsPlan",JSON.stringify(info),cbd,cbe);
}
exports.getBetalingsPlan = getBetalingsPlan;

function getKunde(value,dbd,dbe){
    makeRequest("GET","Kunder/" + value,"",dbd,dbe);
}
exports.getKunde = getKunde;


function registerKunde(id,fornavn,etternavn,cbd,cbe,cbr){
    let k = {
        id: id,
        fornavn: fornavn,
        etternavn: etternavn,
    };

    makeRequest("POST","Kunder",JSON.stringify(k),cbd,cbe,cbr);
}
exports.registerKunde = registerKunde;


function getLånForKunde(kundeId,cbd,cbe){
    makeRequest("GET","Laan/Kunde/" + kundeId,"",cbd,cbe);
}
exports.getLånForKunde = getLånForKunde;