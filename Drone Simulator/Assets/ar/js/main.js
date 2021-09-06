let showError = function (error) {
  alert(error.name + ": " + error.message);
};

// https://stackoverflow.com/a/27725393
function escapeJson(string) {
  // preserve newlines, etc - use valid JSON
  string = string
    .replace(/\\n/g, "\\n")
    .replace(/\\'/g, "\\'")
    .replace(/\\"/g, '\\"')
    .replace(/\\&/g, "\\&")
    .replace(/\\r/g, "\\r")
    .replace(/\\t/g, "\\t")
    .replace(/\\b/g, "\\b")
    .replace(/\\f/g, "\\f");
  // remove non-printable and other non-valid JSON chars
  string = string.replace(/[\u0000-\u0019]+/g, "");
  return string;
}
