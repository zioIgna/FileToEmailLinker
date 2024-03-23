let formColl = document.getElementsByTagName('form');
Array.from(formColl).forEach(el => el.addEventListener("submit", populatePartialView));

//let form = document.getElementById("formId");

//form.addEventListener("submit", populatePartialView);

async function populatePartialView(event) {
    event.preventDefault();

    const response = await fetch(
        event.target.action,
        {
            method: "post",
            body: new FormData(event.target)
        }
    );
    const view = await response.text();
    const receiverNumber = parseInt(view.split('#receiver_')[1].split('" href=')[0]);
    const receiverId = "receiver_" + receiverNumber;
    document.getElementById(receiverId).innerHTML = view;
}