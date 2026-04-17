// frontend/Admin/admin.js
const baseUrl = "http://localhost:5271/api";

Vue.createApp({
    data() {
        return {
            user: null,
            events: [],
            donations: [],
            users: [],
            contacts: [],
            activeTab: "Brugere",
            tabs: ["Brugere", "Events", "Donationer", "Kontakt"],
            showAddUserForm: false,
            newUser: {
                username: "",
                email: "",
                password: "",
                role: "User"
            },
            message: "",
            isSuccess: false
        };
    },
    
    mounted() {
        this.checkAuth();
        this.loadAllData();
    },
    
    methods: {
        checkAuth() {
            const userData = localStorage.getItem('user');
            if (!userData) {
                window.location.href = "../Login/index.html";
                return;
            }
            
            this.user = JSON.parse(userData);
            
            if (this.user.role !== 'Admin') {
                alert("Du har ikke admin-rettigheder!");
                window.location.href = "../Index/index.html";
            }
        },
        
        async loadAllData() {
            await this.loadUsers();
            await this.loadEvents();
            await this.loadDonations();
            await this.loadContacts();
        },
        
        async loadUsers() {
            try {
                const response = await axios.get(`${baseUrl}/User`);
                this.users = response.data;
            } catch (error) {
                console.error("Fejl ved hentning af brugere:", error);
            }
        },
        
        async loadEvents() {
            try {
                const response = await axios.get(`${baseUrl}/Event`);
                this.events = response.data;
            } catch (error) {
                console.error("Fejl ved hentning af events:", error);
            }
        },
        
        async loadDonations() {
            try {
                const response = await axios.get(`${baseUrl}/Donation`);
                this.donations = response.data;
            } catch (error) {
                console.error("Fejl ved hentning af donationer:", error);
            }
        },
        
        async loadContacts() {
            try {
                const response = await axios.get(`${baseUrl}/Contact`);
                this.contacts = response.data;
            } catch (error) {
                console.error("Fejl ved hentning af kontakter:", error);
            }
        },
        
        async addUser() {
            try {
                await axios.post(`${baseUrl}/User`, this.newUser);
                this.message = "✅ Bruger oprettet!";
                this.isSuccess = true;
                this.showAddUserForm = false;
                this.newUser = { username: "", email: "", password: "", role: "User" };
                this.loadUsers();
            } catch (error) {
                this.message = "❌ Fejl: " + error.response?.data;
                this.isSuccess = false;
            }
        },
        
        async updateUserRole(userId, event) {
            try {
                await axios.put(`${baseUrl}/User/${userId}/role`, { role: event.target.value });
                this.message = "✅ Rolle opdateret!";
                this.isSuccess = true;
            } catch (error) {
                this.message = "❌ Fejl ved opdatering";
                this.isSuccess = false;
            }
        },
        
        async toggleUserStatus(userId) {
            try {
                await axios.put(`${baseUrl}/User/${userId}/status`);
                this.message = "✅ Status opdateret!";
                this.isSuccess = true;
                this.loadUsers();
            } catch (error) {
                this.message = "❌ Fejl";
                this.isSuccess = false;
            }
        },
        
        async deleteUser(userId) {
            if (confirm("Er du sikker?")) {
                try {
                    await axios.delete(`${baseUrl}/User/${userId}`);
                    this.message = "✅ Bruger slettet!";
                    this.isSuccess = true;
                    this.loadUsers();
                } catch (error) {
                    this.message = "❌ Fejl";
                    this.isSuccess = false;
                }
            }
        },
        
        editEvent(id) {
            window.location.href = `../Event/Edit.html?id=${id}`;
        },
        
        async deleteEvent(id) {
            if (confirm("Slet dette event?")) {
                try {
                    await axios.delete(`${baseUrl}/Event/${id}`);
                    this.message = "✅ Event slettet!";
                    this.isSuccess = true;
                    this.loadEvents();
                } catch (error) {
                    this.message = "❌ Fejl";
                    this.isSuccess = false;
                }
            }
        },
        
        editDonation(id) {
            window.location.href = `../Donation/editDonation.html?id=${id}`;
        },
        
        async deleteDonation(id) {
            if (confirm("Slet denne donation?")) {
                try {
                    await axios.delete(`${baseUrl}/Donation/${id}`);
                    this.message = "✅ Donation slettet!";
                    this.isSuccess = true;
                    this.loadDonations();
                } catch (error) {
                    this.message = "❌ Fejl";
                    this.isSuccess = false;
                }
            }
        },
        
        editContact(id) {
            window.location.href = `../Kontakt/editKontakt.html?id=${id}`;
        },
        
        async deleteContact(id) {
            if (confirm("Slet denne kontakt?")) {
                try {
                    await axios.delete(`${baseUrl}/Contact/${id}`);
                    this.message = "✅ Kontakt slettet!";
                    this.isSuccess = true;
                    this.loadContacts();
                } catch (error) {
                    this.message = "❌ Fejl";
                    this.isSuccess = false;
                }
            }
        },
        
        navigateTo(path) {
            window.location.href = path;
        },
        
        formatDate(dateString) {
            const date = new Date(dateString);
            return date.toLocaleDateString('da-DK');
        },
        
        logout() {
            localStorage.removeItem('user');
            localStorage.removeItem('token');
            window.location.href = "../Index/index.html";
        }
    }
}).mount("#app");