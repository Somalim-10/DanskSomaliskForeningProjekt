class Navbar {
  constructor() {
    this.isScrolled = false;
    this.init();
  }

  init() {
    this.createNavbar();
    this.addScrollListener();
    this.setActiveLink();
  }

  createNavbar() {
    const navbarHTML = `
      <header class="navbar">
        <div class="nav-logo">
          <a href="../Index/index.html">
            <img src="../img/Forenings - logo (hvid).png" class="logo-img" alt="Somalisk Dansk Forening logo">
          </a>
        </div>
        <div class="nav-center">
          <ul class="nav-links">
            <li class="nav-item"><a href="../Index/index.html">Forside</a></li>
            <li class="nav-item"><a href="../Event/Event.html">Events</a></li>
            <li class="nav-item"><a href="../Donation/Donation.html">Donation</a></li>
            <li class="nav-item"><a href="../Kontakt/Kontakt.html">Kontakt</a></li>
            <li class="nav-item"><a href="../OmOs/OmOs.html">Om os</a></li>
          </ul>
        </div>
      </header>
    `;

    // Sæt navbar på siden
    document.body.insertAdjacentHTML('afterbegin', navbarHTML);
  }

  addScrollListener() {
    window.addEventListener('scroll', () => {
      const navbar = document.querySelector('.navbar');
      if (window.scrollY > 50) {
        navbar.classList.add('scrolled');
      } else {
        navbar.classList.remove('scrolled');
      }
    });
  }

  setActiveLink() {
    const currentPage = window.location.pathname;
    const navLinks = document.querySelectorAll('.nav-item');
    
    navLinks.forEach(link => {
      const href = link.querySelector('a').getAttribute('href');
      if (currentPage.includes(href.split('/').pop())) {
        link.classList.add('active');
      }
    });
  }
}

// Initialiser navbar når siden loader
document.addEventListener('DOMContentLoaded', () => {
  new Navbar();
});
