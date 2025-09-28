var graphqlbtn = document.getElementById("graphqlbtn"); 
var restfullbtn = document.getElementById("restfullbtn");
var submit = document.getElementById("submit");

// Mutation
submit.addEventListener("click", () => 
{
    const name = document.getElementById("name").value?.trim();
    fetch("http://localhost:5096/graphql", {
        method: "POST", 
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            query: `mutation { createUser(name: "${name}") { id name } }`
        })
    })
    .then(response => response.json())
    .then(data => {
        console.log("GraphQL response:", data);
    })
});

restfullbtn.addEventListener("click", () => 
{
    var textField = document.getElementById("restfullp");
    textField.innerText = "This is graphQL your lazy fuck!";
});

// Read
graphqlbtn.addEventListener("click", () => {
  fetch("http://localhost:5096/graphql", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      query: "{ users { id name } }"
    })
  })
  .then(response => response.json())
  .then(data => {
    console.log("GraphQL response:", data);

    const users = data.data.users;
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
});

