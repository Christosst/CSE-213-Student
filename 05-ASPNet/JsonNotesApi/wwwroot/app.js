const form = document.querySelector("#note-form");
const noteId = document.querySelector("#note-id");
const noteText = document.querySelector("#note-text");
const notes = document.querySelector("#notes");
const status = document.querySelector("#status");
const saveButton = document.querySelector("#save");
const cancelButton = document.querySelector("#cancel");

function message(text, error = false) {
  status.textContent = text;
  status.classList.toggle("error", error);
}

async function api(url, options = {}) {
  const response = await fetch(url, options);
  if (response.status === 204) return null;
  const data = await response.json();
  if (!response.ok) {
    const errors = Object.values(data.errors ?? {}).flat().join(" ");
    throw new Error(errors || data.title || `Request failed: ${response.status}`);
  }
  return data;
}

function resetForm() {
  form.reset();
  noteId.value = "";
  saveButton.textContent = "Save note";
  cancelButton.hidden = true;
}

async function loadNotes() {
  const data = await api("/api/notes");
  notes.replaceChildren();
  for (const note of data) {
    const item = document.createElement("li");
    const text = document.createElement("span");
    text.textContent = `#${note.id}: ${note.text}`;
    const edit = document.createElement("button");
    edit.type = "button";
    edit.textContent = "Edit";
    edit.addEventListener("click", () => {
      noteId.value = note.id;
      noteText.value = note.text;
      saveButton.textContent = "Update note";
      cancelButton.hidden = false;
      noteText.focus();
    });
    const remove = document.createElement("button");
    remove.type = "button";
    remove.textContent = "Delete";
    remove.addEventListener("click", async () => {
      remove.disabled = true;
      try {
        await api(`/api/notes/${note.id}`, { method: "DELETE" });
        if (noteId.value === String(note.id)) resetForm();
        await loadNotes();
        message("Note deleted.");
      } catch (error) {
        message(error.message, true);
        remove.disabled = false;
      }
    });
    item.append(text, edit, remove);
    notes.append(item);
  }
}

form.addEventListener("submit", async (event) => {
  event.preventDefault();
  saveButton.disabled = true;
  const editing = noteId.value !== "";
  const url = editing ? `/api/notes/${noteId.value}` : "/api/notes";
  try {
    await api(url, {
      method: editing ? "PUT" : "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ text: noteText.value })
    });
    resetForm();
    await loadNotes();
    message(editing ? "Note updated." : "Note saved on the server.");
  } catch (error) {
    message(error.message, true);
  } finally {
    saveButton.disabled = false;
  }
});

for (const operation of ["uppercase", "trim"]) {
  document.querySelector(`#${operation}`).addEventListener("click", async () => {
    try {
      const result = await api(`/api/text/${operation}`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ text: noteText.value })
      });
      noteText.value = result.text;
      message("Text processed. Choose Save note to keep it.");
    } catch (error) {
      message(error.message, true);
    }
  });
}
cancelButton.addEventListener("click", resetForm);
document.querySelector("#refresh").addEventListener("click", () => {
  loadNotes().then(() => message("Notes reloaded."))
    .catch((error) => message(error.message, true));
});
loadNotes().catch((error) => message(error.message, true));
