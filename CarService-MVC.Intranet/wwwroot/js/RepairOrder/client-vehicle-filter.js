(function () {
    var clientSelect  = document.getElementById('clientSelect');
    var vehicleSelect = document.getElementById('vehicleSelect');

    if (!clientSelect || !vehicleSelect) return;

    var allVehicleOptions = Array.from(
        vehicleSelect.querySelectorAll('option[data-client-id]')
    );

    function filterVehicles() {
        var selectedClientId = clientSelect.value;

        allVehicleOptions.forEach(function (option) {
            var belongsToClient = option.dataset.clientId === selectedClientId;
            option.hidden   = !belongsToClient;
            option.disabled = !belongsToClient;
        });

        var currentlySelected = vehicleSelect.querySelector('option:checked');
        if (currentlySelected && currentlySelected.hidden) {
            vehicleSelect.value = '';
        }
    }

    clientSelect.addEventListener('change', filterVehicles);
    filterVehicles();
})();
