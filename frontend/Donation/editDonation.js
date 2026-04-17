const baseUrl = "http://localhost:5271/api/Donation";

Vue.createApp({
  data() {
    return {
      itemId: null,
      formData: {
        mobilePay: "",
        text: ""
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
          mobilePay: response.data.mobilePay,
          text: response.data.text
        };
        this.isLoading = false;

      } catch (error) {
        console.error("Fejl ved hentning:", error);
        this.message = "❌ Kunne ikke hente MobilePay-info.";
        this.isLoading = false;
      }
    },

    async updateItem() {
      try {
        await axios.put(`${baseUrl}/${this.itemId}`, this.formData);
        this.message = "✅ MobilePay opdateret succesfuldt! Sender dig tilbage...";
        this.isSuccess = true;

        setTimeout(() => {
          window.location.href = "Donation.html";
        }, 2000);

      } catch (error) {
        console.error("Fejl ved opdatering:", error);
        this.message = "❌ Fejl: " + (error.response?.data || error.message);
        this.isSuccess = false;
      }
    },

    goBack() {
      window.location.href = "Donation.html";
    }
  },

  mounted() {
    this.loadItem();
  }
}).mount("#app");
