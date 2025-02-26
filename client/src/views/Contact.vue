<template>
    <div class="container my-5">
        <div class="row">
            <div class="col-md-6">
                <h1>Drop me a line!</h1>
                <p>Fill out the form and press Send Email to send me a message.</p>
                <p>This is a great way to let me know of any riders that you need added, or issues with the app that you have found.</p>
                <p>Alternatively, feel free to email me directly at ted.matuszewski@outlook.com</p>
                <p>No matter how you contact me, or what you subject is, please be sure to include your league name.</p>
            </div>

            <div class="col-md-6">
                <div class="alert alert-danger" role="alert" v-if="error">
                    Please fill out all form fields before clicking send.
                </div>

                <div class="form-group">
                    <label for="name">Name:</label>
                    <input v-model="name" type="text" class="form-control" id="name">
                </div>
                
                <div class="form-group">
                    <label for="body">Message:</label>
                    <textarea v-model="body" class="form-control" id="body"></textarea>
                </div>

                <button v-on:click="sendEmail" class="btn btn-primary">Send Email</button>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { ref, reactive } from "vue";

    let subject = "Turbo Fantasy SMX Contact Form Submission";
    let body = "";
    let name = "";
    let error = ref(false);
    
    function sendEmail() {
        if(body === "" || name === "") {
            error.value = true;
            return;
        }

        let encodedSubject = encodeURIComponent(subject);
        let encodedBody = encodeURIComponent(`From: ${name}`) + "%0A%0A" + encodeURIComponent(`Message: ${body}`);
        let href = `mailto:ted.matuszewski@outlook.com?subject=${encodedSubject}&body=${encodedBody}`;
        let a = document.createElement("a");

        a.style.display = "none";
        document.body.appendChild(a);

        a.href = href;
        a.click();
        document.body.removeChild(a);
    }
</script>