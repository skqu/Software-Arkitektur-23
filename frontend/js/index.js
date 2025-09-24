var btn = document.getElementById("github");
btn.addEventListener("click", login);

async function login()
{
        const res = await fetch("http://localhost:5066/login", {
            headers: { "Content-Type": "application/json" }
        });

        
        if (res.ok) {
             location.href = "/secret/";
        } else if (res.status === 401) {
            alert("Invalid username or password.");
        } else {
            const text = await res.text();
            console.error("Login failed:", res.status, text);
            alert("Something went wrong. Try again.");
        }
}