const BASE_URL = "https://dansksomaliskforeningprojekt-production.up.railway.app";
const baseUrl = `${BASE_URL}/api/Donation`;

// Sæt Authorization header hvis token findes
const token = localStorage.getItem('token');
if (token) {
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}

Vue.createApp({
  data() {
    return {
      formData: {
        mobilePay: "",
        text: ""
      },
      message: "",
      isSuccess: false
    };
  },

  methods: {
    async addItem() {
      try {
        await axios.post(baseUrl, this.formData);
        this.message = "✅ MobilePay oprettet succesfuldt! Sender dig tilbage...";
        this.isSuccess = true;

        setTimeout(() => {
          window.location.href = "Donation.html";
        }, 2000);

      } catch (error) {
        console.error("Fejl ved oprettelse:", error);
        this.message = "❌ Fejl: " + (error.response?.data || error.message);
        this.isSuccess = false;
      }
    },

    goBack() {
      window.location.href = "Donation.html";
    }
  }
}).mount("#app");
