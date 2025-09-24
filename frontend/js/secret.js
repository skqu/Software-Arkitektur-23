(async () => {
  const res = await fetch("http://localhost:5066/token", { credentials: "include" });

  if (res.ok) {
    const data = await res.json();   // parse JSON
    console.log(data); // now works
    var usr = document.getElementById("content");
    usr.innerText = "Velkommen " + data.user["login"];
  } else {
    location.replace("/");
  }
})();
