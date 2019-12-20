<template>
    <div class="row">
        <div class="col-4 col-offset-3">
            <div class="form-group">
                <label>Email</label>
                <input class="form-control" v-model="Email" />
            </div>

            <div class="form-group">
                <label>Password</label>
                <input class="form-control" v-model="Password" />
            </div>

            <button class="btn btn-block btn-primary" v-on:click="SignIn">Sign In</button>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                Email: null,
                Password: null
            };
        },
        mounted() {
            
        },
        methods: {
            SignIn: function() {
                var self = this;

                self.$http.post("/login", {
                    Email: self.Email,
                    Password: self.Password
                }).then((response) => {
                    console.log(response.data);
                }).catch(function (error) {
                    console.log(error);

                    if (error.response) {
                        console.log(error.response.data);
                    }
                });
            }
        }
    };
</script>

<style scoped>
    
</style>