// Generic fetch helper (handles 204 No Content, JSON/empty error bodies, and validation errors)
async function api(url, options = {}) {
  const response = await fetch(url, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...(options.headers || {})
    }
  });
  if (response.status === 204) return null;

  const data = await response.json().catch(() => null);
  if (!response.ok) {
    if (data?.errors) {
      const errorStrings = Object.values(data.errors).flat().join(" ");
      throw new Error(errorStrings);
    }
    const message =
      (typeof data === "string" ? data : null) ||
      data?.message ||
      data?.title ||
      `Request failed with status ${response.status}`;
    throw new Error(message);
  }
  return data;
}

// Verification button for HelloWorldController
const testBtn = document.getElementById("test-hello");
const output = document.getElementById("hello-output");

if (testBtn && output) {
  testBtn.addEventListener("click", async () => {
    output.textContent = "Calling GET /api/hello...";
    try {
      const result = await api("/api/hello");
      output.textContent = JSON.stringify(result, null, 2);
    } catch (err) {
      output.textContent = `Error: ${err.message}`;
    }
  });
}

// ===================================================================
// TODO: Student exercise - implement your frontend JavaScript below!
//
// 1. Fetch data from your controllers (e.g. GET /api/students)
// 2. Render student / course / teacher lists in the #app container
// 3. Create forms to POST new records and handle PUT / DELETE actions
// ===================================================================
