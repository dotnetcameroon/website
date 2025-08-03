const navbarThreshold = 900;
function toggleNavbar() {
  const hamburger = document.getElementById("hamburger");
  const navLinks = document.getElementById("nav-links");

  hamburger.classList.toggle("active");
  navLinks.classList.toggle("active");
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

function toggleExpand() {
  const sidebar = document.getElementById("sidebar");
  sidebar.classList.toggle("collapsed");
}

function onPageSizeSelected() {
  const submitButton = document.getElementById("page-size-submit");
  submitButton.click();
}

function toggleActivityPopup() {
  const popup = document.getElementById("activity-popup");
  const body = document.getElementById("body");
  popup.classList.toggle("active");
  body.scrollTo(0, 0);
}

function adjustTextareaHeight(id) {
  const textarea = document.getElementById(id);
  textarea.style.height = "auto";
  textarea.style.height = textarea.scrollHeight + "px";
}

function saveDraft() {
  try {
    const form = document.getElementById("event-form");
    if (!form.checkValidity()) {
      form.reportValidity();
      return;
    }

    form.submit();
  } catch (error) {
    console.error("An error occurred while publishing the event:", error);
    window.location.href = "/errors";
  }
}

function backToTop() {
  window.scrollTo({ top: 0, behavior: "smooth" });
}

function copySecretToClipboard() {
  const secret = document.getElementById("secret");
  const secret_button = document.getElementById("secret-button");
  navigator.clipboard.writeText(secret.value);
  secret_button.innerHTML = `<i class="fa-solid fa-check"></i>`;
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
