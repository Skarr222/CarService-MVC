(function () {
    var input = document.getElementById('iconCssInput');

    function selectIcon(value) {
        document.querySelectorAll('.icon-opt').forEach(function (btn) {
            btn.classList.toggle('icon-opt-selected', btn.dataset.icon === value);
        });
        input.value = value;
    }

    document.querySelectorAll('.icon-opt').forEach(function (btn) {
        btn.addEventListener('click', function () { selectIcon(this.dataset.icon); });
    });

    if (input.value) selectIcon(input.value);
})();
