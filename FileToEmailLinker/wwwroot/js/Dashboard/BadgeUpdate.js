function decreaseBadge() {
    let elem = document.getElementById('badgeCount');
    let originalVal = parseInt(elem.innerHTML);
    let updatedVal = --originalVal;
    elem.innerHTML = updatedVal.toString();
    return true;
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