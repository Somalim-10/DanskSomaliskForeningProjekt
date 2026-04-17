// frontend/Login/login.js
const baseUrl = "http://localhost:5271/api/Auth";

Vue.createApp({
    data() {
        return {
            formData: {
                username: "",
                password: ""
            },
            message: "",
            isSuccess: false
        };
    },
    
    methods: {
        async login() {
            try {
                const response = await axios.post(`${baseUrl}/login`, this.formData);
                
                // Gem user data i localStorage
                localStorage.setItem('user', JSON.stringify(response.data));
                localStorage.setItem('token', response.data.token);
                
                this.message = "✅ Du er logget ind!";
                this.isSuccess = true;
                
                // Omdirigér til admin panel
                setTimeout(() => {
                    window.location.href = "../Admin/index.html";
                }, 1500);
                
            } catch (error) {
                this.message = "❌ " + (error.response?.data || error.message);
                this.isSuccess = false;
            }
        }
    }
}).mount("#app");