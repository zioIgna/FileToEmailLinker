let form = document.getElementById("formId");

form.addEventListener("submit", populatePartialView);

async function populatePartialView(event) {
    event.preventDefault();

    const response = await fetch(
        form.action,
        {
            method: "post",
            body: new FormData(form)
        }
    );
    const view = await response.text();
    const receiverNumber = parseInt(view.split('#receiver_')[1].split('" href=')[0]);
    const receiverId = "receiver_" + receiverNumber;
    document.getElementById(receiverId).innerHTML = view;
}