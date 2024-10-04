function LoginAuthenticate() {
    var un = document.getElementById("usernameField").value;
    var pw = document.getElementById("passwordField").value;

    try {
        fetch("api/Authentication/Auth", {
            method: "POST",
            body: JSON.stringify({ username: un, password: pw }),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        })
        .then((response) => {
            if (response.ok) {
                return response.json()
            } else {
                alert("Hiba történt bejelentkezéskor!")
            }
        })
        .then((data) => {
            localStorage.setItem("AuthToken", data["token"]);
            localStorage.setItem("Expires", data["expires"])
            window.location.replace("dashboard.html");
        })
    }
    catch (error) {
        console.log(error)
        alert("Egyéb hiba történt bejelentkezéskor! # " + error)
    }
}

function CheckAuth() {
    if (localStorage.getItem("AuthToken") && localStorage.getItem("Expires")) {
        console.log("Got Token and Expiration")
        var exp = localStorage.getItem("Expires").split('.')[0]
        var expDate = new Date(Date.parse(exp));
        var datenow = new Date(Date.now());

        console.log("Expiration: " + expDate)
        console.log("Now: " + datenow)

        if (expDate < datenow) {
            console.log("Expired token")
            window.location.replace("index.html");
        }

        GetClockStatus();

    } else {
        console.log("No Token or Expiration")
        window.location.replace("index.html");
    }
}

function StartClockUI() {
    var start_time = new Date(document.getElementById("timeFrom").innerText);
    var current_time = new Date();

    var totalSeconds = Math.round(Math.abs(start_time.getTime() - current_time.getTime()) / 1000);
    function pad(val) { return val > 9 ? val : "0" + val; }
    setInterval(function () {
        $("#seconds").html(pad(++totalSeconds % 60));
        $("#minutes").html(pad(parseInt(totalSeconds / 60, 10) % 60));
        $("#hours").html(pad(parseInt(totalSeconds / 3600, 10)));
    }, 1000);
}

function GetClockStatus() {
    try {
        fetch("api/Clock/Status", {
            method: "GET",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem("AuthToken")
            }
        })
        .then((response) => {
            if (response.ok) {
                return response.json()
            } else {
                alert("Hiba történt lekérdezéskor!")
            }
        })
        .then((data) => {
            console.log(data)
            if (data["ticking"] == false) {
                document.getElementById("clockInBtn").hidden = false;
                document.getElementById("clockOutBtn").hidden = true;
            } else {
                document.getElementById("timeFromContainer").hidden = false;
                document.getElementById("timeFromContainer").innerHTML += "<span id='timeFrom'>" + data["startDate"].split('.')[0] + "</span>"

                document.getElementById("clockInBtn").hidden = true;
                document.getElementById("clockOutBtn").hidden = false;

                document.getElementById("diagGUID").hidden = false;
                document.getElementById("diagName").hidden = false;
                document.getElementById("diagDesc").hidden = false;
                document.getElementById("diag").hidden = false;

                document.getElementById("diagGUID").innerHTML += "GUID: " + data["entryGUID"]
                document.getElementById("diagName").innerHTML += "EntryName: " + data["entryName"]
                document.getElementById("diagDesc").innerHTML += "Description: " + data["description"]
            }
            StartClockUI();
        })
    }
    catch (error) {
        console.log(error)
        alert("Egyéb hiba történt lekérdezéskor! # " + error)
    }
}

function ClockInOut() {
    try {
        fetch("api/Clock/StartStop", {
            method: "POST",
            body: JSON.stringify({ entryName: "", description: "" }),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem("AuthToken")
            }
        })
        .then((response) => {
            if (response.ok) {
                window.location.replace("dashboard.html");
            } else {
                alert("Hiba történt küldéskor!")
            }
        })
    }
    catch (error) {
        console.log(error)
        alert("Egyéb hiba történt küldéskor! # " + error)
    }
}

function DownloadClockData() {
    try {
        fetch("api/Data/DownloadClockDataExcel", {
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem("AuthToken")
            }
        })
        .then((response) => response.blob())
        .then((myblob) => {
            resolveAndDownloadBlob(myblob)
        })
    }
    catch (error) {
        console.log(error)
        alert("Egyéb hiba történt letöltéskor! # " + error)
    }
}

// https://stackoverflow.com/questions/34509447/download-file-from-api-using-javascript
function resolveAndDownloadBlob(response) {
    let filename = 'ClockData.xlsx';
    filename = decodeURI(filename);
    const url = window.URL.createObjectURL(response);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', filename);
    document.body.appendChild(link);
    link.click();
    window.URL.revokeObjectURL(url);
    link.remove();
}