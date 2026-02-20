const baseUrl = "http://localhost:5271/api/Event";

Vue.createApp({
  data() {
    return {
      items: [],
      message: ""
    };
  },

  methods: {
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

    editEvent(id) {
      window.location.href = `EditEvent.html?id=${id}`;
    }
  },

  mounted() {
    this.getAll();
  }
}).mount("#app");