const baseUrl = "http://localhost:5271/api/Event";

Vue.createApp({
  data() {
    return {
      formData: {
        title: "",
        date: "",
        startTime: 12,
        duration: 2,
        description: ""
      },
      message: "",
      isSuccess: false
    };
  },

  methods: {
    async addItem() {
      try {
        await axios.post(baseUrl, this.formData);
        this.message = "✅ Event oprettet succesfuldt! Sender dig tilbage...";
        this.isSuccess = true;
        
        setTimeout(() => {
          window.location.href = "Event.html";
        }, 2000);
        
      } catch (error) {
        console.error("Fejl ved oprettelse:", error);
        this.message = " Fejl: " + (error.response?.data || error.message);
        this.isSuccess = false;
      }
    },

    goBack() {
      window.location.href = "Event.html";
    }
  }
}).mount("#app");