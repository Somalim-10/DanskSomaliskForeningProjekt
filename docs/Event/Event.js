const BASE_URL = "https://dansksomaliskforeningprojekt-production.up.railway.app";
const baseUrl = `${BASE_URL}/api/Event`;

// Sæt Authorization header hvis token findes
const token = localStorage.getItem('token');
if (token) {
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}

Vue.createApp({
  data() {
    return {
      items: [],
      message: "",
      isAdmin: false,
      eventImages: JSON.parse(localStorage.getItem('eventImages') || '{}')
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
        this.items = response.data;
      } catch (error) {
        console.error("Fejl ved hentning:", error);
        this.message = "❌ Kunne ikke hente events.";
      }
    },

    async deleteItem(id) {
      if (confirm("Er du sikker på, at du vil slette dette event?")) {
        try {
          await axios.delete(`${baseUrl}/${id}`);
          this.message = "✅ Event slettet!";
          this.getAll();
        } catch (error) {
          console.error("Fejl ved sletning:", error);
          this.message = "❌ Fejl ved sletning";
        }
      }
    },

    sortByDate() {
      this.items.sort((a, b) => new Date(a.date) - new Date(b.date));
      this.message = "📅 Sorteret efter dato";
    },

    sortByTitle() {
      this.items.sort((a, b) => a.title.localeCompare(b.title));
      this.message = "🔤 Sorteret efter titel";
    },

    formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleDateString('da-DK', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      });
    },

    formatBadgeDate(dateString) {
      const date = new Date(dateString);
      if (Number.isNaN(date.getTime())) {
        return "";
      }
      const day = date.getDate();
      const month = date
        .toLocaleDateString('da-DK', { month: 'short' })
        .replace('.', '')
        .toUpperCase();
      const year = date.getFullYear();
      return `${day} ${month} ${year}`;
    },

    getImage(eventId) {
      return this.eventImages[eventId] || null;
    },

    editEvent(id) {
      window.location.href = `Edit.html?id=${id}`;
    },
  },

  mounted() {
    this.checkAdminStatus();
    this.getAll();
  }
}).mount("#app");