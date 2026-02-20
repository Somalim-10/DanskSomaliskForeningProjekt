const baseUrl = "http://localhost:5271/api/Event";

Vue.createApp({
  data() {
    return {
      eventId: null,
      formData: {
        title: "",
        date: "",
        startTime: 12,
        duration: 2,
        description: ""
      },
      message: "",
      isSuccess: false,
      isLoading: true
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
          description: event.description
        };
        
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