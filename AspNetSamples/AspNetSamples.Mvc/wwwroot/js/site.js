const loginModal = new bootstrap.Modal(document.getElementById('loginModal'), {
    keyboard: true
});

function loginModalShow() {
    loginModal.show();
};

async function login() {
    let email = document.getElementById('email').value;
    let pswd = document.getElementById('password').value;

    let data = {
        email: email,
        password: pswd
    };

    const response = await fetch("/account/LoginWithJS", {
        method: "POST", // or 'PUT'
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
    });

    if (response.status == 200) {
        loginModal.hide();
        let loginBtns = document.getElementsByClassName('btn-group')[0];

        let logoutBtn = '<button type="button" class="btn btn-primary" aria-expanded="false" id="logout-btn">Logout</button >';

        loginBtns.innerHTML = logoutBtn;

        document.getElementById('logout-btn').addEventListener("click", async () => {
            console.log('logout attempt');
        });
    }


};