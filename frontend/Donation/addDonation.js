const baseUrl = "https://localhost:7261/api/Donation";

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
