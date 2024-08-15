const navbarThreshold = 900;
function toggleNavbar(){
    const hamburger = document.getElementById("hamburger");
    const navLinks = document.getElementById("nav-links");

    hamburger.classList.toggle("active");
    navLinks.classList.toggle("active");
}

window.addEventListener('scroll', (e) => {
    if (window.scrollY < navbarThreshold)
    {
        collapseNavbar();
        hideBackToTop();
    }
    if (window.scrollY >= navbarThreshold) {
        expandNavbar();
        showBackToTop();
    }
});

function collapseNavbar() {
    const navbar = document.getElementById("navbar");
    navbar.classList.remove("scrolled");
}

function expandNavbar() {
    const navbar = document.getElementById("navbar");
    navbar.classList.remove("scrolled");
    navbar.classList.add("scrolled");
}

function showBackToTop() {
    const navbar = document.getElementById("back-to-top");
    navbar.classList.remove("active");
    navbar.classList.add("active");
}

function hideBackToTop() {
    const navbar = document.getElementById("back-to-top");
    navbar.classList.remove("active");
}

function toggleExpand() {
    const sidebar = document.getElementById("sidebar");
    sidebar.classList.toggle("collapsed");
}
