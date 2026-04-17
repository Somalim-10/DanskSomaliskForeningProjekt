const baseUrl = "http://localhost:5271/api/Contact";

Vue.createApp({
  data() {
    return {
      kontakt: {
        id: null,
        address: "",
        phone: "",
        email: "",
        googleMapsUrl: null
      },
      loading: true,
      error: null
    };
  },

  computed: {
    mapsEmbedUrl() {
      if (this.kontakt.address) {
        return `https://maps.google.com/maps?q=${encodeURIComponent(this.kontakt.address)}&output=embed`;
      }
      return null;
    }
  },

  methods: {
    async getAll() {
      try {
        const response = await axios.get(baseUrl);
        const data = Array.isArray(response.data) ? response.data[0] : response.data;
        if (data) {
          this.kontakt = data;
        }
      } catch (error) {
        console.error("Fejl ved hentning:", error);
        this.error = "Kontaktoplysninger ikke tilgængelige lige nu.";
      } finally {
        this.loading = false;
      }
    }
  },

  mounted() {
    this.getAll();

    window.addEventListener('scroll', function () {
      const navbar = document.querySelector('.navbar');
      if (window.scrollY > 50) {
        navbar.classList.add('scrolled');
      } else {
        navbar.classList.remove('scrolled');
      }
    });
  }
}).mount("#app");
