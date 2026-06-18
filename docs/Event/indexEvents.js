const BASE_URL = (location.hostname === "localhost" || location.hostname === "127.0.0.1")
    ? "http://localhost:5271"
    : "https://dansksomaliskforeningprojekt-production.up.railway.app";
const baseUrl = `${BASE_URL}/api/Event`;

const token = localStorage.getItem('token');
if (token) {
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}

Vue.createApp({
  data() {
    return {
      items: [],
      message: "",
      eventImages: JSON.parse(localStorage.getItem('eventImages') || '{}')
    };
  },

  computed: {
    upcomingEvents() {
      const today = new Date();
      today.setHours(0, 0, 0, 0);
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
          if (Number.isFinite(a.startTime)) aDate.setHours(a.startTime, 0, 0, 0);
          if (Number.isFinite(b.startTime)) bDate.setHours(b.startTime, 0, 0, 0);
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
      } catch (error) {
        console.error("Fejl ved hentning af events:", error);
        this.message = "Kunne ikke hente events.";
      }
    },

    formatDay(dateString) {
      return new Date(dateString).getDate();
    },

    formatMonth(dateString) {
      const months = ['JAN','FEB','MAR','APR','MAJ','JUN','JUL','AUG','SEP','OKT','NOV','DEC'];
      return months[new Date(dateString).getMonth()];
    },

    getImage(eventId) {
      // Egne uploads (localStorage) har forrang; ellers et pænt standardfoto fra nettet
      return this.eventImages[eventId] || ('https://picsum.photos/seed/sdf-event-' + eventId + '/800/450');
    }
  },

  mounted() {
    this.getAll();
  }
}).mount("#app");