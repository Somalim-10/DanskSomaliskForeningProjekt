const baseUrl = "https://localhost:7261/api/Event";

// Sæt Authorization header hvis token findes
const token = localStorage.getItem('token');
if (token) {
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}

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