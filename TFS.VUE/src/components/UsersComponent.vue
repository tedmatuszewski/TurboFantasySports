<template>
    <div>
        <h2>Manage System Users</h2>
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Email</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="user in users">
                    <td>{{user.firstName}}</td>
                    <td>{{user.lastName}}</td>
                    <td>{{user.email}}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                users: []
            };
        },
        mounted() {
            var self = this;

            // self.$http.post("/users", {
            //     FirstName: "Ted",
            //     LastName: "Matuszewski",
            //     Email: "ted.matuszewski@outlook.com",
            //     Password: "runwalk2014"
            // }).then((response) => {
            //     console.log(response.data)
            // });

            self.$http.get("/users").then((response) => {
                self.users = response.data;
            });
        },
        methods: {
            
        }
    };
</script>

<style scoped>
    
</style>