function showError(error) {
  alert(error.name + ": " + error.message);
}

// https://stackoverflow.com/a/27725393
function escapeJson(string) {
  return string
      .replace(/\n/g, "\\n")
      .replace(/\r/g, "\\r");
}
