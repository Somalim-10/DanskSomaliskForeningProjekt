const baseUrl = "http://localhost:5271/api/Contact";

Vue.createApp({
  data() {
    return {
      formData: {
        address: "",
        phone: "",
        email: "",
        googleMapsUrl: null
      },
      message: "",
      isSuccess: false
    };
  },

  methods: {
    async addItem() {
      try {
        const payload = {
          ...this.formData,
          googleMapsUrl: this.formData.googleMapsUrl || null
        };
        await axios.post(baseUrl, payload);
        this.message = "✅ Kontaktoplysninger oprettet succesfuldt! Sender dig tilbage...";
        this.isSuccess = true;

        setTimeout(() => {
          window.location.href = "Kontakt.html";
        }, 2000);

      } catch (error) {
        console.error("Fejl ved oprettelse:", error);
        this.message = "❌ Fejl: " + (error.response?.data || error.message);
        this.isSuccess = false;
      }
    },

    goBack() {
      window.location.href = "Kontakt.html";
    }
  }
}).mount("#app");
