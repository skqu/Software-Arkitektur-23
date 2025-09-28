const graphqlbtn = document.getElementById("graphqlbtnR"); // you can keep or repurpose
const restfullbtn = document.getElementById("restfullbtnR");
const submit = document.getElementById("submitR");

const API_BASE = "http://localhost:5225/api/v2/users";

console.log("RestFULL");

// GET /api/v2/users
function loadUsers() {
    fetch(API_BASE, {
        method: "GET"
    })
  .then(response => response.json())
  .then(data => {
    console.log("RestFULL response:", data);
    console.log("RestFULL response:", data);
    const users = data;
    const tbody = document.querySelector("#usersTable tbody");

    // Clear old rows
    tbody.innerHTML = "";

    // Insert rows
    users.forEach(user => {
    const row = document.createElement("tr");

    const idCell = document.createElement("td");
    idCell.textContent = user.id;
    row.appendChild(idCell);

    const nameCell = document.createElement("td");
    nameCell.textContent = user.name;
    row.appendChild(nameCell);

    tbody.appendChild(row);
    });
  });
}

// POST /api/v2/users
submit.addEventListener("click", () => {
  const name = document.getElementById("name").value?.trim();
  
  const payload = {
    name,
    email: `${name}@iba.dk`,
    pwd: "SuperSecret1",
    uname: name,
    role: "admin"
  };
  fetch(API_BASE, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify( payload )
    })
    .then(response => response.json())
    .then(data => {
        console.log("RestFull response:", data);
    })


    loadUsers();
    document.getElementById("name").value = "";
});

// Hook up your buttons
restfullbtn.addEventListener("click", () => {
  loadUsers();
});

// If you want the “GraphQL” button to also load REST now:
graphqlbtn.addEventListener("click", () => {
    var textField = document.getElementById("graphqlp");
    textField.innerText = "This is RestFULL your lazy fuck!";
});
