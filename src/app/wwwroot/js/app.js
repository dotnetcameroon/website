const navbarThreshold = 900;
function toggleNavbar() {
  const hamburger = document.getElementById("hamburger");
  const mobileNav = document.getElementById("mobile-nav");

  hamburger.classList.toggle("active");
  mobileNav.classList.toggle("active");
  mobileNav.classList.toggle("hidden");
}

window.addEventListener("scroll", (e) => {
  if (window.scrollY < navbarThreshold) {
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

function backToTop() {
  window.scrollTo({ top: 0, behavior: "smooth" });
}

function showDotnetSessions() {
  const dotnet = document.getElementById("dotnet-sessions");
  const microsoft = document.getElementById("microsoft-sessions");

  const dotnetTab = document.getElementById("dotnet-tab");
  const microsoftTab = document.getElementById("microsoft-tab");

  dotnet.classList.remove("hidden");
  microsoft.classList.remove("hidden");
  microsoft.classList.add("hidden");

  dotnetTab.classList.add("active");
  microsoftTab.classList.remove("active");
}

function showMicrosoftSessions() {
  const dotnet = document.getElementById("dotnet-sessions");
  const microsoft = document.getElementById("microsoft-sessions");

  const dotnetTab = document.getElementById("dotnet-tab");
  const microsoftTab = document.getElementById("microsoft-tab");

  dotnet.classList.remove("hidden");
  microsoft.classList.remove("hidden");
  dotnet.classList.add("hidden");

  dotnetTab.classList.remove("active");
  microsoftTab.classList.add("active");
}
