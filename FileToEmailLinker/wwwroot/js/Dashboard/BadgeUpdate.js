//let rowColl = document.getElementsByClassName('badgeUpdater');
//Array.from(rowColl).forEach(el => el.addEventListener("click", startSequence));

function decreaseBadge() {
    let elem = document.getElementById('badgeCount');
    let originalVal = parseInt(elem.innerHTML);
    let updatedVal = --originalVal;
    elem.innerHTML = updatedVal.toString();
    //return true;
}

function startSequence(url) {
    //event.preventDefault();
    let urlCheckAlert = url; // "/Dashboard/Checkalert";
    let urlVisualizedAlerts = "/Dashboard/GetVisualizedAlerts";
    let index = url.lastIndexOf('/') +1;
    let unvisualizedRowId = url.substring(index);
    let rowname = "unvisualizedRow_" + unvisualizedRowId;
    let row = document.getElementsByName(rowname)[0];
    //let fullUrl = '"/' + url + '"';
    let cleanUrl = url.replace("'", "");
    row.setAttribute("data-ajax", "true");
    row.setAttribute("data-ajax-mode", "replace");
    row.setAttribute("data-ajax-update", "#segnalazioniRows");
    row.setAttribute('href', cleanUrl);
    row.click();
    decreaseBadge();
    setTimeout(() => { document.getElementById('updateVisualizedTable').click() },1000);
    //let resp = await pupulateUnvisualizedAlerts(urlCheckAlert);
    //let resp2 = await populateVisualizedAlerts(urlVisualizedAlerts);
}

async function populateVisualizedAlerts(url) {
    const response = await fetch(
        url,
        {
            method: "get",
        }
    );
    const view = await response.text();
    document.getElementById('unvisualizedAlertRows').innerHTML = view;
}

async function pupulateUnvisualizedAlerts(url) {
    const response = await fetch(
        url,
        {
            method: "get"
        }
    );
    const view = await response.text();
    document.getElementById('segnalazioniRows').innerHTML = view;
}

//let elements = document.querySelectorAll('.badgeUpdater');

//let clickEvent = () =>
////{
////    let elem = document.getElementById('badgeCount');
////    let originalVal = parseInt(elem.innerHTML);
////    let updatedVal = --originalVal;
////    elem.innerHTML = updatedVal.toString();
////};
//{
//    setTimeout(() => {
//            document.getElementById('badgeUpdateAction').click()
//        }
//        , 1000)
//};

//elements.forEach((item) => {
//    console.log("elemento: " + item);
//    item.addEventListener('click', clickEvent)
//});

//document.body.addEventListener("click", function (event) {
//    if (event.target.classList.contains("badgeUpdater")) {
//        let elem = document.getElementById('badgeCount');
//        let originalVal = parseInt(elem.innerHTML);
//        let updatedVal = --originalVal;
//        elem.innerHTML = updatedVal.toString();
//    }
//})