function decreaseBadge() {
    let elem = document.getElementById('badgeCount');
    let originalVal = parseInt(elem.innerHTML);
    let updatedVal = --originalVal;
    elem.innerHTML = updatedVal.toString();
}

function swapAlert(id) {
    fetch("Dashboard/CheckAlert/" + id, { method: "get" })
        .then((response) => {
            if (response.ok) {
                response.text().then((viewResp) => {
                    document.getElementById('segnalazioniRows').innerHTML = viewResp;
                });
            }
            fetch("Dashboard/GetVisualizedAlerts", { method: "get" })
                .then((response) => {
                    if (response.ok) {
                        response.text().then((viewResp2) => {
                             document.getElementById('visualizedAlertRows').innerHTML = viewResp2;
                        });
                        decreaseBadge();
                    }
                })
        })
}
