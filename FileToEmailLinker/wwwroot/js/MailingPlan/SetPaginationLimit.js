function refreshWithNewLimit(selectedObject) {
    var value = selectedObject.value;
    window.location.href = '/MailingPlans/Index?limit=' + value;
}