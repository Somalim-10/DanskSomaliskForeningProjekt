const BASE_URL = "https://dansksomaliskforeningprojekt-production.up.railway.app";
const baseUrl = `${BASE_URL}/api/Donation`;

// Sæt Authorization header hvis token findes
const token = localStorage.getItem('token');
if (token) {
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}

Vue.createApp({
  data() {
    return {
      donation: {
        id: null,
               text: "",

        mobilePay: ""
      },
      loading: false,
      error: null,
      message: "",
      isAdmin: false
    };
  },

  methods: {
    checkAdminStatus() {
      const user = JSON.parse(localStorage.getItem('user') || '{}');
      this.isAdmin = user.role === 'Admin';
    },

    async getAll() {
      try {
        const response = await axios.get(baseUrl);
        const data = Array.isArray(response.data) ? response.data[0] : response.data;
        this.donation = data;
      } catch (error) {
        console.error("Fejl ved hentning:", error);
        this.error = "Donationsinfo ikke tilgængelig lige nu.";
      } finally {
        this.loading = false;
      }
    },

    editItem(id) {
      window.location.href = `Edit.html?id=${id}`;
    }
  },

  mounted() {
    this.checkAdminStatus();
    this.getAll();

    window.addEventListener('scroll', function () {
      const navbar = document.querySelector('.navbar');
      if (window.scrollY > 50) {
        navbar.classList.add('scrolled');
      } else {
        navbar.classList.remove('scrolled');
      }
    });

    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
      anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
          target.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
      });
    });
  }
}).mount("#app");
