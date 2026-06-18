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
      imageError: false
    };
  },

  methods: {
    async addItem() {
      try {
        const response = await axios.post(baseUrl, this.formData);
        const newEventId = response.data.id;

        // Gem billede i localStorage hvis URL er udfyldt
        if (this.formData.imageUrl && newEventId) {
          const images = JSON.parse(localStorage.getItem('eventImages') || '{}');
          images[newEventId] = this.formData.imageUrl;
          localStorage.setItem('eventImages', JSON.stringify(images));
        }

        this.message = "✅ Event oprettet succesfuldt! Sender dig tilbage...";
        this.isSuccess = true;
        setTimeout(() => { window.location.href = "Event.html"; }, 2000);

      } catch (error) {
        console.error("Fejl ved oprettelse:", error);
        this.message = "❌ Fejl: " + (error.response?.data || error.message);
        this.isSuccess = false;
      }
    },

    previewImage() {
      this.imageError = false;
    },

    goBack() {
      window.location.href = "Event.html";
    }
  }
}).mount("#app");