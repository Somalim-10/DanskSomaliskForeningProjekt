// frontend/Login/login.js
const baseUrl = "https://localhost:7261/api/Auth";

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
                
                console.log("✅ Login success!");
                console.log("Token:", response.data.token);
                console.log("User:", response.data);
                
                // Sæt authorization header straks
                axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
                
                this.message = "✅ Du er logget ind!";
                this.isSuccess = true;
                
                // Omdirigér til forsiden
                setTimeout(() => {
                    window.location.href = "../Index/index.html";
                }, 1500);
                
            } catch (error) {
                console.error("Login error:", error.response?.data || error.message);
                this.message = "❌ " + (error.response?.data || error.message);
                this.isSuccess = false;
            }
        }
    }
}).mount("#app");