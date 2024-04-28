let formColl = document.getElementsByClassName('inlineReceiverForm');
Array.from(formColl).forEach(el => el.addEventListener("submit", populatePartialView));

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