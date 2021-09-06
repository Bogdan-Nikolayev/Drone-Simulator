function showError(error) {
  alert(error.name + ": " + error.message);
}

// https://stackoverflow.com/a/27725393
function escapeCRLF(string) {
  return string
    .replace(/\r/g, "\\r")
    .replace(/\n/g, "\\n");
}
