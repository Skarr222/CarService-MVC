(function () {
    function calcTotal() {
        var total = 0;
        document.querySelectorAll('[data-service-row]').forEach(function (row) {
            var checkbox = row.querySelector('.svc-check');
            var price    = parseFloat(row.querySelector('.svc-price').value) || 0;
            var qty      = parseInt(row.querySelector('.svc-qty').value)     || 0;
            var lineTotal = price * qty;
            row.querySelector('.svc-line').textContent = checkbox.checked
                ? lineTotal.toFixed(2) + ' zł'
                : '—';
            if (checkbox.checked) total += lineTotal;
        });
        document.getElementById('totalCost').value = total > 0 ? total.toFixed(2) : '';
    }

    document.querySelectorAll('.svc-check, .svc-price, .svc-qty').forEach(function (el) {
        el.addEventListener('change', calcTotal);
        el.addEventListener('input',  calcTotal);
    });

    document.getElementById('recalcBtn')?.addEventListener('click', calcTotal);

    calcTotal();
})();
