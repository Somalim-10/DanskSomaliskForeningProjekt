const baseUrl = "http://localhost:5271/api/Event";

Vue.createApp({
  data() {
    return {
      items: [],
      message: ""
    };
  },

  computed: {
    upcomingEvents() {
      const today = new Date();
      today.setHours(0, 0, 0, 0);

      // Find de næste 3 kommende events (fra i dag og frem)
      return this.items
        .filter(event => {
          if (!event.date) return false;
          const eventDate = new Date(event.date);
          if (Number.isFinite(event.startTime)) {
            eventDate.setHours(event.startTime, 0, 0, 0);
          }
          return !isNaN(eventDate) && eventDate >= today;
        })
        .sort((a, b) => {
          const aDate = new Date(a.date);
          const bDate = new Date(b.date);

          if (Number.isFinite(a.startTime)) {
            aDate.setHours(a.startTime, 0, 0, 0);
          }
          if (Number.isFinite(b.startTime)) {
            bDate.setHours(b.startTime, 0, 0, 0);
          }

          return aDate - bDate;
        })
        .slice(0, 3);
    }
  },

  methods: {
    async getAll() {
      try {
        const response = await axios.get(baseUrl);
        this.items = response.data;
        console.log("Hentede events til forsiden:", this.items);
      } catch (error) {
        console.error("Fejl ved hentning af events:", error);
        this.message = "Kunne ikke hente events.";
      }
    },

    formatDay(dateString) {
      const date = new Date(dateString);
      return date.getDate();
    },

    formatMonth(dateString) {
      const date = new Date(dateString);
      const months = ['JAN', 'FEB', 'MAR', 'APR', 'MAJ', 'JUN', 'JUL', 'AUG', 'SEP', 'OKT', 'NOV', 'DEC'];
      return months[date.getMonth()];
    }
  },
  

  mounted() {
    this.getAll();
  }
}).mount("#app");