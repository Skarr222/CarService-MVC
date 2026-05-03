const sidebar = document.querySelector('.sidebar');
const wrapper = document.querySelector('.main-wrapper');
const overlay = document.getElementById('sidebarOverlay');

function closeMobileSidebar() {
    sidebar.classList.remove('open');
    overlay.classList.remove('active');
}

document.getElementById('sidebarToggle')?.addEventListener('click', function () {
    if (window.innerWidth >= 992) {
        sidebar.classList.toggle('collapsed');
        wrapper.classList.toggle('expanded');
    } else {
        sidebar.classList.toggle('open');
        overlay.classList.toggle('active');
    }
});

document.getElementById('sidebarClose')?.addEventListener('click', closeMobileSidebar);
overlay?.addEventListener('click', closeMobileSidebar);

document.addEventListener('click', function (e) {
    if (window.innerWidth >= 992) return;
    if (!sidebar.classList.contains('open')) return;
    if (!sidebar.contains(e.target) && e.target.id !== 'sidebarToggle') {
        closeMobileSidebar();
    }
});