const baseUrl = "https://localhost:7261/api/Contact";

Vue.createApp({
  data() {
    return {
      itemId: null,
      formData: {
        address: "",
        phone: "",
        email: "",
        googleMapsUrl: null
      },
      message: "",
      isSuccess: false,
      isLoading: true
    };
  },

  methods: {
    getIdFromUrl() {
      const urlParams = new URLSearchParams(window.location.search);
      return urlParams.get('id');
    },

    async loadItem() {
      this.itemId = this.getIdFromUrl();

      if (!this.itemId) {
        this.message = "❌ Intet ID fundet!";
        this.isLoading = false;
        return;
      }

      try {
        const response = await axios.get(`${baseUrl}/${this.itemId}`);
        this.formData = {
          address: response.data.address,
          phone: response.data.phone,
          email: response.data.email,
          googleMapsUrl: response.data.googleMapsUrl || ""
        };
        this.isLoading = false;

      } catch (error) {
        console.error("Fejl ved hentning:", error);
        this.message = "❌ Kunne ikke hente kontaktoplysninger.";
        this.isLoading = false;
      }
    },

    async updateItem() {
      try {
        const payload = {
          ...this.formData,
          googleMapsUrl: this.formData.googleMapsUrl || null
        };
        await axios.put(`${baseUrl}/${this.itemId}`, payload);
        this.message = "✅ Kontaktoplysninger opdateret succesfuldt! Sender dig tilbage...";
        this.isSuccess = true;

        setTimeout(() => {
          window.location.href = "Kontakt.html";
        }, 2000);

      } catch (error) {
        console.error("Fejl ved opdatering:", error);
        this.message = "❌ Fejl: " + (error.response?.data || error.message);
        this.isSuccess = false;
      }
    },

    goBack() {
      window.location.href = "Kontakt.html";
    }
  },

  mounted() {
    this.loadItem();
  }
}).mount("#app");
