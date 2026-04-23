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
      eventId: null,
      formData: {
        title: "",
        date: "",
        startTime: 12,
        duration: 2,
        description: "",
        imageUrl: ""
      },
      message: "",
      isSuccess: false,
      isLoading: true,
      imageError: false
    };
  },

  methods: {
    getEventIdFromUrl() {
      const urlParams = new URLSearchParams(window.location.search);
      return urlParams.get('id');
    },

    async loadEvent() {
      this.eventId = this.getEventIdFromUrl();
      
      if (!this.eventId) {
        this.message = "❌ Ingen event ID fundet!";
        this.isLoading = false;
        return;
      }

      try {
        const response = await axios.get(`${baseUrl}/${this.eventId}`);
        const event = response.data;
        
        const date = new Date(event.date);
        const formattedDate = date.toISOString().split('T')[0];
        
        this.formData = {
          title: event.title,
          date: formattedDate,
          startTime: event.startTime,
          duration: event.duration,
          description: event.description,
          imageUrl: ""
        };
        const images = JSON.parse(localStorage.getItem('eventImages') || '{}');
        this.formData.imageUrl = images[this.eventId] || "";
        
        this.isLoading = false;
        
      } catch (error) {
        console.error("Fejl ved hentning:", error);
        this.message = " Kunne ikke hente event";
        this.isLoading = false;
      }
    },

    async updateItem() {
      try {
        await axios.put(`${baseUrl}/${this.eventId}`, this.formData);

        const images = JSON.parse(localStorage.getItem('eventImages') || '{}');
        if (this.formData.imageUrl) {
          images[this.eventId] = this.formData.imageUrl;
        } else {
          delete images[this.eventId];
        }
        localStorage.setItem('eventImages', JSON.stringify(images));

        this.message = "✅ Event opdateret succesfuldt! Sender dig tilbage...";
        this.isSuccess = true;
        
        setTimeout(() => {
          window.location.href = "Event.html";
        }, 2000);
        
      } catch (error) {
        console.error("Fejl ved opdatering:", error);
        this.message = " Fejl: " + (error.response?.data || error.message);
        this.isSuccess = false;
      }
    },

    goBack() {
      window.location.href = "Event.html";
    }
  },

  mounted() {
    this.loadEvent();
  }
}).mount("#app");